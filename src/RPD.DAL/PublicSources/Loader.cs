using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.FtpClient;
using AlienJust.Support.Concurrent;
using AlienJust.Support.Concurrent.Contracts;
using AlienJust.Support.Identy;
using AlienJust.Support.Reflection;
using DataAbstractionLevel.Low.InternalKitchen.KeyValueStringStorage;
using DataAbstractionLevel.Low.PsnConfig;
using DataAbstractionLevel.Low.Storage.DeviceInformationStorage;
using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand;
using DataAbstractionLevel.Low.Storage.FtpFilesStorage;
using DataAbstractionLevel.Low.Storage.PsnDataInformationStorage;
using DataAbstractionLevel.Low.Storage.PsnDataStorage;
using DataAbstractionLevel.Low.Storage.PsnLogCustomConfigStorage;
using DataAbstractionLevel.Low.Storage.Shared;
using DataAbstractionLevel.Low.Storage.StreamableData;
using DataAbstractionLevel.Low.Storage.StreamableData.File;
using RPD.DAL.KeyValueStorageHolders.Memory;
using RPD.DAL.KeyValueStorageHolders.Xml;
using RPD.DAL.LibraryLoader.SimpleRelease;
using RPD.DAL.PsnConfiguration.Contracts.Internal;
using RPD.DAL.PsnConfiguration.RelayOnLowLevel;
using RPD.DAL.Storage.PsnLogCustomConfigStorage;
using RPD.DalRelease.Configuration;
using RPD.EventArgs;
using RPD.Supports;

namespace RPD.DAL {
	/// <summary>
	/// Позволяет получить ссылки на корневые интерфейсы сборки RPD.DAL.
	/// Для каждого экземпляра Loader многократный вызов метода GetRepository будет возвращать ссылку на один и тот же объект.
	/// Но для каждого экземпляра Loader многократный вызов метода GetDataReaderForDevice будет всегда возвращать ссылку на новый объект.
	/// </summary>
	public class Loader : ILoader {
		private readonly Lazy<List<IPsnInternalConfiguration>> _psnInternalConfigurations;
		private readonly IWorker<Action> _backWorker;
		private readonly IThreadNotifier _uiNotifier;

		/// <summary>
		/// Инициализирует новый экземпляр класса
		/// </summary>
		public Loader() {
			_psnInternalConfigurations = new Lazy<List<IPsnInternalConfiguration>>(() => LoadPsnConfigurations(Path.Combine(typeof(Loader).GetAssemblyDirectoryPath(), "defaults")).ToList());
			var bqw = new BackgroundQueueWorker<Action>(a=>a());
			_backWorker = bqw;
			_uiNotifier = bqw;
		}

		private IEnumerable<IPsnInternalConfiguration> LoadPsnConfigurations(string psnConfigurationsPath)
		{
			var defaultsDirInfo = new DirectoryInfo(psnConfigurationsPath);
			var psnConfigFiles = defaultsDirInfo.GetFiles("psn.*.xml");
			var psnConfiguratios = new List<IPsnInternalConfiguration>();

			var assemblyKvStorageForPsnSignals = new KeyValueStringStorageXmlMemCache(Path.Combine(Support.GetAppDataDirectoryPathAndCreateItIfNeeded(), "PsnParameters.xml")); // TODO ???
			
			foreach (var psnConfigFile in psnConfigFiles) {
				try {
					var configLaoder = new PsnProtocolConfigurationLoaderFromXml(psnConfigFile.FullName);
					var psnProtocolConfig = configLaoder.LoadConfiguration();
					IPsnInternalConfigrationBuilder cfgBuilder = new PsnConfigurationBuilderFromLowLevel(psnProtocolConfig);
					IPsnInternalConfiguration psnConfig = cfgBuilder.BuildConfig();
					psnConfiguratios.Add(psnConfig);
				}
				catch {
					continue;
				}
			}
			return psnConfiguratios;
		}

		/// <summary>
		/// Создает описание конфигурации
		/// </summary>
		/// <returns>Ссылка на новое описание конфигурации</returns>
		public IDeviceConfiguration CreateDeviceConfiguration() {
			var bw = new BackgroundQueueWorker<Action>(a => a());
			return new DeviceConfiguration(bw, bw);
		}

		/// <summary>
		/// Получает список доступных программе PSN конфигураций
		/// </summary>
		public IEnumerable<IPsnProtocolConfig> AvailablePsnConfigruations {
			get {
				var result = new List<IPsnProtocolConfig>();
				foreach (var psnIntrnalConfig in _psnInternalConfigurations.Value) {
					var configBuilder = new PsnProtocolConfigBuilderFromPsnInternalConfiguration(psnIntrnalConfig);
					result.Add(configBuilder.Build());
				}
				return result;
			}
		}

		/// <summary>
		/// Получает ссылку на репозиторий в локальной директории
		/// </summary>
		/// <param name="directoryPath">Путь к директории</param>
		/// <returns>Ссылка на репозиторий</returns>
		public IRepository GetLocalDirectoryRepository(string directoryPath) {
			var streamableFileForPsnDataInfoStorage = new StreamedFile(Path.Combine(directoryPath, "PsnDataInformationStorage.xml"));
			var streamableFileForDeviceInformationStorage = new StreamedFile(Path.Combine(directoryPath, "DeviceInformationStorage.xml"));
			var streamableFileForPsnDataCustomConfigStorage = new StreamedFile(Path.Combine(directoryPath, "PsnDataCustomConfigrationStorage.xml"));
			var bw = new BackgroundQueueWorker<Action>(a => a());
			return new RelayRepository(_psnInternalConfigurations.Value,
				() => {
					try {
						return new ReposetoryOpenedResultSimple(
							new StreamedDataStorageRelayMemoryCache(new StreamReadableDataStorageLocalDir(directoryPath)),
							new PsnDataInformationStorageRelayMemoryCache(new PsnDataInformationStorageXDocument(streamableFileForPsnDataInfoStorage, streamableFileForPsnDataInfoStorage)),
							new DeviceInformationStorageRelayMemoryCache(new DeviceInformationStorageXDocument(streamableFileForDeviceInformationStorage, streamableFileForDeviceInformationStorage)),
							new PsnDataCustomConfigurationsStorageRelayMemoryCache(new PsnDataCustomConfigurationsStorageXDocument(streamableFileForPsnDataCustomConfigStorage, streamableFileForPsnDataCustomConfigStorage)),
							new PsnInternalConfigurationsStorageRelay(_psnInternalConfigurations.Value));
					}
					catch (Exception ex) {
						throw new Exception("Не удалось открыть репозиторий по указанному пути: " + directoryPath, ex);
					}

				}
				, new KeyValueStorageHolderXml()
				, bw
				, bw
				, "DIR@" + directoryPath);
		}

		/// <summary>
		/// Получает ссылку на архивированный репозиторий
		/// </summary>
		/// <param name="zipFileName">Путь к файлу-архиву репозитория</param>
		/// <returns>Ссылка на репозиторий</returns>
		public IRepository GetZippedRepository(string zipFileName) {
			var bw = new BackgroundQueueWorker<Action>(a => a());

			return new RelayRepository(_psnInternalConfigurations.Value,
				() => {
					try {
						var zipStreamForPscCustomStorage = new StreamedZippedFile(zipFileName, "PsnDataCustomConfigrationStorage.xml");
						var zipStreamForPsnDataInformationStorage = new StreamedZippedFile(zipFileName, "PsnDataInformationStorage.xml");
						var zipStreamForDeviceInformationStorage = new StreamedZippedFile(zipFileName, "DeviceInformationStorage.xml");

						var deviceInformationStorage = new DeviceInformationStorageXDocument(zipStreamForDeviceInformationStorage, zipStreamForDeviceInformationStorage);
						var deviceInformationStorageCache = new DeviceInformationStorageRelayMemoryCache(deviceInformationStorage);

						var psnDataStorage = new StreamReadableDataStorageZip(zipFileName);
						var psnDataStorageCache = new StreamedDataStorageRelayMemoryCache(psnDataStorage);

						var psnDataInfosStorage = new PsnDataInformationStorageXDocument(zipStreamForPsnDataInformationStorage, zipStreamForPsnDataInformationStorage);
						var psnDataInfosStorageCache = new PsnDataInformationStorageRelayMemoryCache(psnDataInfosStorage);

						var psnDataCustomInfoStorage = new PsnDataCustomConfigurationsStorageXDocument(zipStreamForPscCustomStorage, zipStreamForPscCustomStorage);
						var psnDataCustomInfoStorageCache = new PsnDataCustomConfigurationsStorageRelayMemoryCache(psnDataCustomInfoStorage);

						return new ReposetoryOpenedResultSimple(
							psnDataStorageCache,
							psnDataInfosStorageCache,
							deviceInformationStorageCache,
							psnDataCustomInfoStorageCache,
							new PsnInternalConfigurationsStorageRelay(_psnInternalConfigurations.Value));
					}
					catch (Exception ex) {
						throw new Exception("Ошибка, проверьте правильность имени файла " + zipFileName + Environment.NewLine + "Если имя файла указано верно, то, возможно файл не доступен для чтения или записи (например, файл поврежден или у Вас нет прав на доступ к файлу)", ex);
					}
				}
				, new KeyValueStorageHolderMemory()
				, bw
				, bw
				, "ZIP@" + zipFileName);
		}

		/// <summary>
		/// Получает ссылку на репозиторий устройства РПД
		/// </summary>
		/// <param name="nandPath">Путь к устройству РПД</param>
		/// <param name="psnConfiguration">Конфигурация ПСН применяемая ко всем данным NAND</param>
		/// <returns>Ссылка на репозиторий</returns>
		public IRepository GetNandFlashRepository(string nandPath, IPsnProtocolConfig psnConfiguration)
		{
			var bw = new BackgroundQueueWorker<Action>(a => a());
			var deviceId = new IdentifierGuidBased(Guid.NewGuid());
			return new RelayRepository(_psnInternalConfigurations.Value,
				() => {
					try {
						if (psnConfiguration == null) throw new Exception("Ошибка, при обращении к устройству необходимо вручную указать конфигурацию ПСН");
						var deviceInformationStorage = new DeviceInformationStorageNand(nandPath, deviceId.ToString());
						var deviceInformationStorageCache = new DeviceInformationStorageRelayMemoryCache(deviceInformationStorage);

						var psnDataStorage = new StreamReadableDataStorageNand(nandPath);
						var psnDataStorageCache = new StreamedDataStorageRelayMemoryCache(psnDataStorage);

						var psnDataInfosStorage = new PsnDataInformationStorageNand(nandPath, deviceId.ToString());
						var psnDataInfosStorageCache = new PsnDataInformationStorageRelayMemoryCache(psnDataInfosStorage);

						var psnDataCustomInfoStorage = new PsnDataCustomConfigurationsStoragePredefined(
							psnDataInfosStorageCache.PsnDataInformations.Select(
								pdi => new PsnDataCustomConfigurationSimple(pdi.Id, new IdentifierStringBased(psnConfiguration.Id.ToString()), string.Empty)));
						var psnDataCustomInfoStorageCache = new PsnDataCustomConfigurationsStorageRelayMemoryCache(psnDataCustomInfoStorage);

						return new ReposetoryOpenedResultSimple(
							psnDataStorageCache,
							psnDataInfosStorageCache,
							deviceInformationStorageCache,
							psnDataCustomInfoStorageCache,
							new PsnInternalConfigurationsStorageRelay(_psnInternalConfigurations.Value));
					}
					catch (Exception ex) {
						throw new Exception("Не удалось открыть устройство по указанному пути: " + nandPath, ex);
					}
				}
				, new KeyValueStorageHolderMemory()
				, bw
				, bw
				, "NAND@" + nandPath);
		}

		/// <summary>
		/// Получает ссылку на локальный репозиторий, имеющий структуру FTP сервера
		/// </summary>
		/// <param name="ftpFilesDirecotyPath">Путь к корневой директории файлов</param>
		/// <param name="locomotiveName">Название локомотива</param>
		/// <param name="sectionName">Название секции</param>
		/// <param name="psnConfiguration">Конфигурация магистрали ПСН</param>
		/// <returns>Ссылка на репозиторий</returns>
		/// <exception cref="Exception">Может возникнуть исключение, если не удается открыть репозиторий по указанному пути</exception>
		public IRepository GetFtpFilesRepository(string ftpFilesDirecotyPath, string locomotiveName, string sectionName, IPsnProtocolConfig psnConfiguration)
		{
			var bw = new BackgroundQueueWorker<Action>(a => a());
			var deviceId = new IdentifierGuidBased(Guid.NewGuid());
			return new RelayRepository(_psnInternalConfigurations.Value,
				() =>
				{
					try
					{
						if (psnConfiguration == null) throw new Exception("Ошибка, при обращении к устройству необходимо вручную указать конфигурацию ПСН");
						var deviceInformationStorage = new DeviceInformationStorageCustomSingle(locomotiveName, sectionName, deviceId.ToString());
						var deviceInformationStorageCache = new DeviceInformationStorageRelayMemoryCache(deviceInformationStorage);

						var psnDataStorage = new StreamReadableDataStorageFtpFiles(ftpFilesDirecotyPath);
						var psnDataStorageCache = new StreamedDataStorageRelayMemoryCache(psnDataStorage);

						var psnDataInfosStorage = new PsnDataInformationStorageFtpFiles(ftpFilesDirecotyPath, deviceId.ToString());
						var psnDataInfosStorageCache = new PsnDataInformationStorageRelayMemoryCache(psnDataInfosStorage);

						var psnDataCustomInfoStorage = new PsnDataCustomConfigurationsStoragePredefined(
							psnDataInfosStorageCache.PsnDataInformations.Select(
								pdi => new PsnDataCustomConfigurationSimple(pdi.Id, new IdentifierStringBased(psnConfiguration.Id.ToString()), string.Empty)));
						var psnDataCustomInfoStorageCache = new PsnDataCustomConfigurationsStorageRelayMemoryCache(psnDataCustomInfoStorage);

						return new ReposetoryOpenedResultSimple(
							psnDataStorageCache,
							psnDataInfosStorageCache,
							deviceInformationStorageCache,
							psnDataCustomInfoStorageCache,
							new PsnInternalConfigurationsStorageRelay(_psnInternalConfigurations.Value));
					}
					catch (Exception ex)
					{
						throw new Exception("Не удалось открыть устройство по указанному пути: " + ftpFilesDirecotyPath, ex);
					}
				}
				, new KeyValueStorageHolderMemory()
				, bw
				, bw
				, "FTPFILES@" + ftpFilesDirecotyPath);
		}

		/// <summary>
		/// Получает ссылку на удаленный репозиторий, хранящийся на FTP сервере
		/// </summary>
		/// <param name="ftpHost">Адрес узла (IP-адрес или доменное имя)</param>
		/// <param name="ftpPort">TCP порт, на котором расположен FTP сервер</param>
		/// <param name="ftpUsername">Имя пользователя FTP сервера</param>
		/// <param name="ftpPassword">Пароль пользователя FTP сервера</param>
		/// <param name="deviceNumber">Номер устройства</param>
		/// <param name="locomotiveName">Название локомотива</param>
		/// <param name="sectionName">Название сервера</param>
		/// <param name="psnConfiguration">Конфигурация магистрали ПСН</param>
		/// <returns>Ссылка на репозиторий</returns>
		/// <exception cref="Exception">Может возникнуть исключение, если не удается открыть репозиторий</exception>
		public IRepository GetFtpClientRepository(string ftpHost, int ftpPort, string ftpUsername, string ftpPassword, int deviceNumber, string locomotiveName, string sectionName, IPsnProtocolConfig psnConfiguration)
		{
			var bw = new BackgroundQueueWorker<Action>(a => a());
			var deviceId = new IdentifierGuidBased(Guid.NewGuid());
			return new RelayRepository(_psnInternalConfigurations.Value,
				() =>
				{
					try
					{
						if (psnConfiguration == null) throw new Exception("Ошибка, при обращении к устройству необходимо вручную указать конфигурацию ПСН");
						var deviceInformationStorage = new DeviceInformationStorageCustomSingle(locomotiveName, sectionName, deviceId.ToString());
						var deviceInformationStorageCache = new DeviceInformationStorageRelayMemoryCache(deviceInformationStorage);

						var ftpFilesStorage = new FtpFilesStorageLazyCached(
							new FtpFilesStorageSimple(
								ftpHost, ftpPort, ftpUsername, ftpPassword, deviceNumber + "/"));

						var psnDataStorage = new StreamReadableDataStorageFtpUrl(ftpFilesStorage, deviceNumber);
						var psnDataStorageCache = new StreamedDataStorageRelayMemoryCache(psnDataStorage);

						var psnDataInfosStorage = new PsnDataInformationStorageFtpUrl(ftpFilesStorage, deviceNumber, deviceId.ToString());
						var psnDataInfosStorageCache = new PsnDataInformationStorageRelayMemoryCache(psnDataInfosStorage);

						var psnDataCustomInfoStorage = new PsnDataCustomConfigurationsStoragePredefined(
							psnDataInfosStorageCache.PsnDataInformations.Select(
								pdi => new PsnDataCustomConfigurationSimple(pdi.Id, new IdentifierStringBased(psnConfiguration.Id.ToString()), string.Empty)));
						var psnDataCustomInfoStorageCache = new PsnDataCustomConfigurationsStorageRelayMemoryCache(psnDataCustomInfoStorage);

						return new ReposetoryOpenedResultSimple(
							psnDataStorageCache,
							psnDataInfosStorageCache,
							deviceInformationStorageCache,
							psnDataCustomInfoStorageCache,
							new PsnInternalConfigurationsStorageRelay(_psnInternalConfigurations.Value));
					}
					catch (Exception ex)
					{
						throw new Exception("Не удалось открыть устройство по указанному пути: " + ftpHost + ":" + ftpPort, ex);
					}
				}
				, new KeyValueStorageHolderMemory()
				, bw
				, bw
				, "FtpClient@" + ftpHost + ":" + ftpPort + "[deviceNumber=" + deviceNumber + "]");
		}

		/// <summary>
		/// Получает список репозиториев FTP сервера
		/// </summary>
		/// <param name="ftpHost">Адрес узла (IP-адрес или доменное имя)</param>
		/// <param name="ftpPort">TCP порт, на котором расположен FTP сервер</param>
		/// <param name="ftpUsername">Имя пользователя FTP сервера</param>
		/// <param name="ftpPassword">Пароль пользователя FTP сервера</param>
		/// <param name="callbackAction">Действие обратного вызова по получению списка репозиториев</param>
		public void GetFtpRepositoryInfosAsync(string ftpHost, int ftpPort, string ftpUsername, string ftpPassword, Action<OnCompleteEventArgs, IEnumerable<IFtpRepositoryInfo>> callbackAction) {
			_backWorker.AddWork(
				() => {
					try {
						var result = new List<IFtpRepositoryInfo>();
						using (var conn = new FtpClient()) {
							conn.Host = ftpHost;
							conn.Port = ftpPort;
							conn.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
							var items = conn.GetListing(); // list root items
							foreach (var ftpListItem in items) {
								try {
									if (ftpListItem.Type == FtpFileSystemObjectType.Directory) {
										var deviceNumber = int.Parse(ftpListItem.Name);
										result.Add(new FtpRepositoryInfoSimple {
										                                       	DeviceNumber = deviceNumber,
										                                       	FtpHost = ftpHost,
										                                       	FtpPassword = ftpPassword,
										                                       	FtpPort = ftpPort,
										                                       	FtpUsername = ftpUsername
										                                       });
									}
								}
								catch /*(Exception ex)*/ {
									continue; // FTP server can contain some other directories
								}
							}
						}
						if (callbackAction != null) {
							_uiNotifier.Notify(() => callbackAction(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Error, "Список устройств получен"), result));
						}
					}
					catch (Exception ex) {
						if (callbackAction != null)
							_uiNotifier.Notify(() => callbackAction(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Error, ex.ToString()), null));
					}
				});
		}

		/// <summary>
		/// Получает ссылку на удаленный FTP репозиторий
		/// </summary>
		/// <param name="ftpRepositoryInfo">Информация о FTP репозитории</param>
		/// <param name="locomotiveName">Название локомотива</param>
		/// <param name="sectionName">Назваине секции</param>
		/// <param name="psnConfiguration">Конфигурация магистрали ПСН</param>
		/// <returns>Ссылка на репозиторий</returns>
		public IRepository GetFtpRepository(IFtpRepositoryInfo ftpRepositoryInfo, string locomotiveName, string sectionName, IPsnProtocolConfig psnConfiguration)
		{
			return GetFtpClientRepository(
				ftpRepositoryInfo.FtpHost,
				ftpRepositoryInfo.FtpPort,
				ftpRepositoryInfo.FtpUsername,
				ftpRepositoryInfo.FtpPassword,
				ftpRepositoryInfo.DeviceNumber, locomotiveName, sectionName, psnConfiguration);
		}

		/// <summary>
		/// В будущем будет правильно выгружать все хранилища
		/// </summary>
		public void Dispose() {
			// TODO: dispose all storages here
		}
	}

	class PsnInternalConfigurationsStorageRelay : IPsnInternalConfigurationsStorage {
		private readonly List<IPsnInternalConfiguration> _psnConfigs;

		public PsnInternalConfigurationsStorageRelay(List<IPsnInternalConfiguration> psnConfigs) {
			_psnConfigs = psnConfigs;
		}

		public IEnumerable<IPsnInternalConfiguration> InternalConfigurations {
			get { return _psnConfigs; }
		}
	}
}

