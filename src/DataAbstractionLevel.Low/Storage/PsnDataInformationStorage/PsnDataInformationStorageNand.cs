using System;
using System.Collections.Generic;
using System.IO;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.PsnData;
using DataAbstractionLevel.Low.Storage.Contracts;
using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.ReleaseNand;
using DataAbstractionLevel.Low.Storage.Shared;

namespace DataAbstractionLevel.Low.Storage.PsnDataInformationStorage
{
	/// <summary>
	/// Хранилище данных о ПСН логе в формате NAND памяти блока РПД
	/// </summary>
	public class PsnDataInformationStorageNand : IPsnDataInformationStorage {
		private readonly string _directoryPath;
		private readonly string _deviceInformationId;

		/// <summary>
		/// Создает новый экземпляр класса
		/// </summary>
		/// <param name="directoryPath">Путь к устройству</param>
		/// <param name="deviceInformationId">Идентфикатор инфомрации об устройстве</param>
		public PsnDataInformationStorageNand(string directoryPath, string deviceInformationId) {
			_directoryPath = directoryPath;
			_deviceInformationId = deviceInformationId;
		}


		/// <summary>
		/// Перечисление хранимых данных
		/// No cache, loads directly from XML, do not use without really needs
		/// </summary>
		public IEnumerable<IPsnDataInformation> PsnDataInformations
		{
			get {
				var psnDatas = new List<IPsnDataInformation>();

				var systemConfigLoader = new SystemConfigurationBuilderFromBinaryFile(Path.Combine(_directoryPath, "SYSCONF.BIN"));
				var systemConfig = systemConfigLoader.BuildConfiguration();
				var psnFileInfo = new FileInfo(Path.Combine(_directoryPath, "PSN.BIN")); // Информация о файле лога магистрали ПСН
				if (psnFileInfo.Exists)
				{
					var fixedFragments = PsnBinParser.ApplyAdvancedFragmentationFixed(
						psnFileInfo.FullName,
						PsnBinParser.GetPsnPagesIntervalsFixed(
							psnFileInfo.FullName,
							systemConfig.PsnLogPredefinedFragmentInfos,
							systemConfig.LastWrittenPageAddress,
							systemConfig.PsnLogStartPageNumber));

					var powerFragments = PsnBinParser.ApplyAdvancedFragmentationStack(
						psnFileInfo.FullName,
						PsnBinParser.GetPsnPagesIntervalsStack(
							psnFileInfo.FullName,
							systemConfig.PsnLogPowerUpFragmentInfos,
							systemConfig.LastWrittenPageAddress,
							systemConfig.PsnLogStartPageNumber));

					// TODO: нужно добавить к фрагментам части логов сформированные по включению
					foreach (PsnBinLogAdvancedLocationInfo t in fixedFragments) {
						var id1 = t.FirstPageInfo.Number.ToString("X4");
						var id2 = t.LastPageInfo.Number.ToString("X4");
						var id3 = systemConfig.NetAddress.ToString("X4");

						var beginTime = t.FirstPageInfo.Time.HasValue ? t.FirstPageInfo.Time.Value : t.FirstDatedPageInfo.HasValue ? t.FirstDatedPageInfo.Value.Time : null;
						var endTime = t.LastPageInfo.Time.HasValue ? t.LastPageInfo.Time : null;

						var id4 = beginTime.HasValue ? beginTime.Value.Ticks.ToString("X8") : string.Empty;
						var id5 = endTime.HasValue ? endTime.Value.Ticks.ToString("X8") : string.Empty;

						psnDatas.Add(new PsnDataInformationSimple(id1 + id2 + id3 + id4 + id5, beginTime, endTime, null, PsnDataFragmentType.FixedLength, t.IsLastPsnLogOnDevice, _deviceInformationId));
					}

					foreach (PsnBinLogAdvancedLocationInfo t in powerFragments)
					{
						var id1 = t.FirstPageInfo.Number.ToString("X4");
						var id2 = t.LastPageInfo.Number.ToString("X4");
						var id3 = systemConfig.NetAddress.ToString("X4");

						var beginTime = t.FirstPageInfo.Time.HasValue ? t.FirstPageInfo.Time.Value : t.FirstDatedPageInfo.HasValue ? t.FirstDatedPageInfo.Value.Time : null;
						var endTime = t.LastPageInfo.Time.HasValue ? t.LastPageInfo.Time : null;

						var id4 = beginTime.HasValue ? beginTime.Value.Ticks.ToString("X8") : string.Empty;
						var id5 = endTime.HasValue ? endTime.Value.Ticks.ToString("X8") : string.Empty;

						psnDatas.Add(new PsnDataInformationSimple(id1 + id2 + id3 + id4 + id5, beginTime, endTime, null, PsnDataFragmentType.PowerDepended, t.IsLastPsnLogOnDevice, _deviceInformationId));
					}
				}
				return psnDatas;
			}
		}

		/// <summary>
		/// Добавляет новые данные в хранилище (операция не поддерживается)
		/// </summary>
		/// <param name="psnLogInformationId">Идентификатор записи</param>
		/// <param name="beginTime">Время начала лога</param>
		/// <param name="endTime">Время конца лога</param>
		/// <param name="saveTime">Время сохранения данных</param>
		/// <param name="psnDataType">Тип лога</param>
		/// <param name="isLastDeviceLog">Индикатор того, что лог является последним на устройстве</param>
		/// <param name="deviceInformationId">Идентификатор информации об устройстве</param>
		public void Add(IIdentifier psnLogInformationId, DateTime? beginTime, DateTime? endTime, DateTime? saveTime, PsnDataFragmentType psnDataType, bool isLastDeviceLog, IIdentifier deviceInformationId)
		{
			throw new NotSupportedException("Операция добавления данных на устройство не поддерживается");
		}

		/// <summary>
		/// Удаляет данные из хранилища (операция на самом деле не поддерживается, нельзя удалять произвольные данные из блока RPD (пока что))
		/// </summary>
		/// <param name="psnDataInformationId">Идентификатор данных в хранилище, которые необходимо удалить</param>
		public void Remove(IIdentifier psnDataInformationId)
		{
			throw new NotSupportedException("Операция удаления данных устройства не поддерживается");
		}
	}
}
