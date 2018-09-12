using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using AlienJust.Support.Concurrent.Contracts;
using AlienJust.Support.Identy;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.PsnConfig.Contracts;
using DataAbstractionLevel.Low.Storage.Contracts;
using RPD.DAL.Common.SimpleRelease;
using RPD.DAL.KeyValueStorageHolders.Contracts;
using RPD.DAL.LibraryLoader.Contracts.Internal;
using RPD.DAL.PsnConfiguration.RelayOnLowLevel;
using RPD.EventArgs;

namespace RPD.DAL.RepostirorySystem.Relay {
	internal sealed class RelayRepository : IRepository {
		private readonly object _sync;
		private readonly Func<IReposetoryOpenedResult> _openStorageFunc;
		private readonly IKeyValueStorageHolder _kvStorageHolder;

		private IPsnDataStorage _psnDataStorage;
		private IPsnDataInformationStorage _psnDataInformtationStorage;
		private IDeviceInformationStorage _deviceInformationStorage;
		private IPsnDataCustomConfigurationsStorage _psnDataCustomConfigurationsStorage;
		private IStorage<IPsnProtocolConfiguration> _psnConfigurationsStorage;

		private bool _isOpened;

		private readonly IWorker<Action> _backWorker;
		private readonly string _repoName;
		private readonly IThreadNotifier _uiNotifier;
		private readonly ObservableCollection<ILocomotive> _locomotives;


		public RelayRepository(Func<IReposetoryOpenedResult> openStorageFunc, IKeyValueStorageHolder kvStorageHolder, IThreadNotifier uiNotifier, IWorker<Action> bgNotifier, string repoName) {
			_openStorageFunc = openStorageFunc;
			_kvStorageHolder = kvStorageHolder;
			_sync = new object();
			_backWorker = bgNotifier;
			_repoName = repoName;
			_uiNotifier = uiNotifier;
			_locomotives = new ObservableCollection<ILocomotive>(); // we assumed that ctor called from UI
			_locomotives.CollectionChanged += LocomotivesOnCollectionChanged;
		}

		private void LocomotivesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs) {
			Console.WriteLine(_repoName + " > _locomotives changed, count now is " + _locomotives.Count);
		}

		public void Open(Action<OnCompleteEventArgs> onComplete, Action<OnProgressChangeEventArgs> onProgressChange) {
			_uiNotifier.Notify(() => onProgressChange(new OnProgressChangeEventArgs(0)));
			if (!IsOpened) {
				IsOpened = true;
				_backWorker.AddWork(
					() => {
						try {
							var openResult = _openStorageFunc();
							_psnDataStorage = openResult.PsnDataStorage;
							_psnDataInformtationStorage = openResult.PsnDataInformationStorage;
							_psnDataCustomConfigurationsStorage = openResult.PsnDataCustomConfigurationsesStorage;
							_deviceInformationStorage = openResult.DeviceInformationStorage;
							_psnConfigurationsStorage = openResult.PsnConfigurationsStorage;
							InitialTreeBuildUnsafe(pp => _uiNotifier.NotifyAndWait(() => onProgressChange(new OnProgressChangeEventArgs((int)pp))));

							_uiNotifier.NotifyAndWait(() => onProgressChange(new OnProgressChangeEventArgs(100)));
							_uiNotifier.NotifyAndWait(() => onComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Ok, "Репозиторий успешно открыт")));
						}
						catch (Exception ex) {
							_uiNotifier.NotifyAndWait(() => onComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Error, ex.ToString())));
							IsOpened = false;
						}
					});
			}
			else {
				_uiNotifier.Notify(() => onProgressChange(new OnProgressChangeEventArgs(100)));
				_uiNotifier.Notify(() => onComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Ok, "Внимание, репозиторий уже открыт")));
			}

		}


		private void InitialTreeBuildUnsafe(Action<double> progressChangeAction) {
			// Этот метод, в отличие от UpdateLocomotivesUnsafe() не оперирует хэшами,
			// что позволяет выиграть производительность в медленных хранилищах (например, NAND)

			var devInfos = _deviceInformationStorage.DeviceInformations.ToList();
			var psnDatas = _psnDataStorage.PsnDatas.ToList();
			var psnDataInfos = _psnDataInformtationStorage.PsnDataInformations.ToList();

			var signal = new AutoResetEvent(false);
			//Exception exc = null;
			//_uiNotifier.Notify(
			//() => {
			//try {
			int totalDevInfosCount = devInfos.Count;
			int currentDevInfoZbIndex = 0;
			foreach (var devInfo in devInfos) {
				var loc = new Locomotive(devInfo.Name, _backWorker, _deviceInformationStorage);

				_uiNotifier.NotifyAndWait(() => _locomotives.Add(loc));


				var section = new Section(new UidStringToLower(devInfo.Id.IdentyString), devInfo.Description, _backWorker, _deviceInformationStorage);

				_uiNotifier.NotifyAndWait(() => loc.Sections.Add(section));


				IDeviceInformation info = devInfo;

				var psnDatasInfosForDev = psnDataInfos.Where(pdi => pdi.DeviceInformationId.ToString() == info.Id.ToString()).ToList();
				var totalPsnDatasCount = psnDatasInfosForDev.Count;
				int currentPsnDataInfoZbIndex = 0;
				foreach (var psnDataInfo in psnDatasInfosForDev) {
					try {
						var psnData = psnDatas.First(pl => pl.Id.IdentyString == psnDataInfo.Id.IdentyString);
						var psnCustomConfig = _psnDataCustomConfigurationsStorage.Configurations.First(pc => pc.Id.IdentyString == psnDataInfo.Id.IdentyString);

						var psnConfig = _psnConfigurationsStorage.StoredItems.First(pc => pc.Id.IdentyString.ToLower() == psnCustomConfig.PsnConfigurationId.IdentyString.ToLower());

						var builder = new PsnProtocolConfigBuilderFromLowLevel(psnConfig);
						var psnProtocolConfig = builder.Build();

						IPsnDataInformation dataInfo = psnDataInfo;
						var psnLog = new PsnLogRelay(
							psnDataInfo
							, psnData
							, psnProtocolConfig
							, psnConfig
							, psnCustomConfig
							, _uiNotifier
							, _backWorker
							, _psnConfigurationsStorage
							, _psnDataCustomConfigurationsStorage
							, _kvStorageHolder);

						_uiNotifier.NotifyAndWait(() => section.Psns.Add(psnLog));
					}
					catch (Exception ex) {
						// TODO: catch exception and deliver it to user as warning message
						Console.WriteLine(ex);
					}
					currentPsnDataInfoZbIndex++;
					progressChangeAction((currentDevInfoZbIndex * 100.0 + currentPsnDataInfoZbIndex * 100.0 / totalPsnDatasCount) / totalDevInfosCount);
				}
				currentDevInfoZbIndex++;
				progressChangeAction(currentDevInfoZbIndex * 100.0 / totalDevInfosCount);
			}
		}


		private void UpdateLocomotivesIfAddedUnsafe() {
			var devInfos = _deviceInformationStorage.DeviceInformations.ToList();
			var psnDatas = _psnDataStorage.PsnDatas.ToList();
			var psnDataInfos = _psnDataInformtationStorage.PsnDataInformations.ToList();
			var psnDataCustomInfos = _psnDataCustomConfigurationsStorage.Configurations.ToList();

			// TODO: нужно работать максимально в фоне: в потоке UI только операции изменения ObservableCollection коллекций

			// 1. Check for newly things:
			foreach (var devInfo in devInfos) {
				var loc = _locomotives.FirstOrDefault(l => l.Name == devInfo.Name);
				if (loc == null) {
					loc = new Locomotive(devInfo.Name, _backWorker, _deviceInformationStorage);

					_uiNotifier.NotifyAndWait(() => _locomotives.Add(loc));
				}
				// at that point loc exist

				var section = loc.Sections.FirstOrDefault(s => s.Name == devInfo.Description);
				if (section == null) {
					section = new Section(new UidStringToLower(devInfo.Id.IdentyString), devInfo.Description, _backWorker, _deviceInformationStorage);
					_uiNotifier.NotifyAndWait(() => loc.Sections.Add(section));
				}
				// at that point section already exist


				IDeviceInformation currentDeviceInformation = devInfo;

				var possiblePsnDataToAdd = psnDataInfos.Where(pdi => pdi.DeviceInformationId.ToString() == currentDeviceInformation.Id.ToString()).ToList();
				foreach (var psnDataInformation in possiblePsnDataToAdd) {
					// Условие ниже исключает добавление уже присутствующих у секции логов:
					if (section.Psns.All(pl => pl.Id.UnicString != psnDataInformation.Id.IdentyString)) {
						try {
							var data = psnDatas.First(d => d.Id.ToString() == psnDataInformation.Id.ToString()); // TODO: can throw exception if data fail saved
							var dataCustomInfo = psnDataCustomInfos.First(pdci => pdci.Id.ToString() == psnDataInformation.Id.ToString());
							var dataPsnConfig = _psnConfigurationsStorage.StoredItems.First(pc => pc.Information.Id.IdentyString == dataCustomInfo.PsnConfigurationId.IdentyString);
							var builder = new PsnProtocolConfigBuilderFromLowLevel(dataPsnConfig);
							var psnProtocolConfig = builder.Build();


							var logToAdd = new PsnLogRelay(
								psnDataInformation
								, data
								, psnProtocolConfig
								, dataPsnConfig
								, dataCustomInfo
								, _uiNotifier
								, _backWorker
								, _psnConfigurationsStorage
								, _psnDataCustomConfigurationsStorage
								, _kvStorageHolder);

							_uiNotifier.NotifyAndWait(() => section.Psns.Add(logToAdd));
						}
						catch {
							// Значит ошибка получения данных ПСН, либо ошибка получения кастомной информации
							continue;
						}
					}
					// else log exist in list
				}
			}
		}


		private void UpdateLocomotivesIfDeletedUnsafe() {
			var devInfos = _deviceInformationStorage.DeviceInformations.ToList();
			var psnDataInfos = _psnDataInformtationStorage.PsnDataInformations.ToList();

			// 2. Check for deleted things:

			var locomotivesToDelete = new List<ILocomotive>();

			foreach (var locomotive in _locomotives) {
				ILocomotive locomotive1 = locomotive;
				var devInfosForLocomotive = devInfos.Where(di => di.Name == locomotive1.Name).ToList();
				if (devInfosForLocomotive.Count == 0) {
					// Если в привязаном хранилище не найдено не одной информации об устройстве, то нужно удалить такой локомотив
					locomotivesToDelete.Add(locomotive);
				}
				else {
					// Если найдены информации для локомотива, то проверяем каждую его секцию:
					var sectionsToDelete = new List<ISection>();
					foreach (var section in locomotive.Sections) {
						ISection section1 = section; // for closure
						var devInfosForSection = devInfosForLocomotive.Where(di => di.Description == section1.Name).ToList();
						if (devInfosForSection.Count == 0) {
							// Если для секции локомотива не удалось найти информаию об устройстве в хранилище - нужно удалить секцию
							sectionsToDelete.Add(section);
						}
						else {
							// Если в привязанном хранилище есть информация об устройстве для секции, то нужно проверить логи:
							var psnDataInformations = psnDataInfos.Where(pdi => devInfosForSection.Any(di => di.Id.ToString() == pdi.DeviceInformationId.ToString())).ToList();
							var psnLogsToDelete = section.Psns.Where(psnLog => psnDataInformations.All(pdi => pdi.Id.ToString() != psnLog.Id.ToString())).ToList();

							foreach (var psnLog in psnLogsToDelete) {
								_uiNotifier.NotifyAndWait(()=>section.Psns.Remove(psnLog));
							}
						}
					}

					foreach (var section in sectionsToDelete) {
						_uiNotifier.NotifyAndWait(() => locomotive.Sections.Remove(section));
					}
				}
			}

			foreach (var locomotive in locomotivesToDelete) {
				_uiNotifier.NotifyAndWait(()=>_locomotives.Remove(locomotive));

			}
		}


		public ObservableCollection<ILocomotive> Locomotives {
			get {
				if (!IsOpened) throw new Exception("Перед обращением за локомотивами необходимо открыть репозиторий");
				return _locomotives;
			}
		}


		public void Remove(ISection section, Action<OnCompleteEventArgs> onComplete) {
			if (!IsOpened) {
				_uiNotifier.Notify(() => onComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Error, "Сперва необходимо открыть репозиторий!")));
			}
			else {
				_backWorker.AddWork(
					() => {
						try {
							var devInfosToRemove = _deviceInformationStorage.DeviceInformations.Where(di => di.Id.ToString() == section.DeviceInformationId.ToString()).ToList();
							foreach (var devInfo in devInfosToRemove) {
								IDeviceInformation info = devInfo; // for closure
							var idsToRemove = _psnDataInformtationStorage.PsnDataInformations.Where(pdi => pdi.DeviceInformationId == info.Id).Select(pdi => pdi.Id).ToList();
								foreach (var idToRemove in idsToRemove) {
									_psnDataStorage.Remove(idToRemove);
									_psnDataInformtationStorage.Remove(idToRemove);
									_psnDataCustomConfigurationsStorage.Remove(idToRemove);
								}
							// TODO: remove RPD logs
							_deviceInformationStorage.Remove(devInfo.Id);
							}
							UpdateLocomotivesIfDeletedUnsafe();
							_uiNotifier.Notify(() => onComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Ok, "Секция удалена из репозитория")));
						}
						catch (Exception ex) {
							_uiNotifier.Notify(() => onComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Error, ex.ToString())));
						}
					});

			}
		}


		public void Remove(IEnumerable<IPsnLog> psnLogs, Action<OnCompleteEventArgs> onComplete) {
			if (!IsOpened) {
				_uiNotifier.Notify(() => onComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Error, "Сперва необходимо открыть репозиторий!")));
			}
			else {
				_backWorker.AddWork(() => {
					try {
						var idsToRemove = psnLogs.Select(pl => pl.Id);
						foreach (var idToRemove in idsToRemove) {
							_psnDataStorage.Remove(new IdentifierStringToLowerBased(idToRemove.UnicString));
							_psnDataInformtationStorage.Remove(new IdentifierStringToLowerBased(idToRemove.UnicString));
							_psnDataCustomConfigurationsStorage.Remove(new IdentifierStringToLowerBased(idToRemove.UnicString));
						}

						var devIdsToRemove = (from devInfo in _deviceInformationStorage.DeviceInformations where !_psnDataInformtationStorage.PsnDataInformations.Any(pdi => pdi.DeviceInformationId.ToString() == devInfo.Id.ToString()) select devInfo.Id).ToList();
						foreach (var uid in devIdsToRemove) {
							_deviceInformationStorage.Remove(uid);
						}

						UpdateLocomotivesIfDeletedUnsafe();
						_uiNotifier.Notify(() => onComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Ok, "ПСН логи удалены из репозитория")));
					}
					catch (Exception ex) {
						_uiNotifier.Notify(() => onComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Error, ex.ToString())));
					}
				});
			}
		}

		// TODO: remove RPD logs

		public void SaveDataAsync(IRepository sourceRepo, IEnumerable<IPsnLog> psnLogs, IEnumerable<IFaultLog> rpdLogs, Action<OnCompleteEventArgs> onComplete, Action<OnProgressChangeEventArgs> onProgressChange) {
			// TODO: save rpdLogs
			if (!IsOpened) {
				_uiNotifier.Notify(() => onComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Error, "Сперва необходимо открыть репозиторий!")));
			}
			else {
				_backWorker.AddWork(
					() => {
						try {
							var sourceSections = new List<ISection>();
							foreach (var locomotive in sourceRepo.Locomotives) {
								sourceSections.AddRange(locomotive.Sections);
							}

							var pLogs = psnLogs.ToList();

							int totalProceedCount = 0;
							double perLogPercent = 0.0;
							double lastInLogProgress = 0.0;
							double lastInLogProgressRaw = 0.0;
							foreach (var psnLog in pLogs) {
								if (_psnDataInformtationStorage.PsnDataInformations.All(pdi => pdi.Id.ToString() != psnLog.Id.ToString())) {
								// TODO: плохо конечно, что я работаю с секциями и локомотивами репозитория-источника в фоновом потоке данного репозитория :)
								// TODO: работать с ними в UI и ждать результата здесь!
								var sourceSection = sourceSections.First(s => s.Psns.Any(pl => pl.Id.ToString() == psnLog.Id.ToString()));
									var sourceLocomotive = sourceRepo.Locomotives.First(l => l.Sections.Any(s => s == sourceSection));
								//var devInfo = _deviceInformationStorage.DeviceInformations.First(di=>di.parentSection.DeviceInformationId);


								// Пока у меня нет четкого (уникального) идентификатора устройства, поэтому всё определяет название локомотива и номер секции
								if (!_deviceInformationStorage.DeviceInformations.Any(di => di.Name == sourceLocomotive.Name && di.Description == sourceSection.Name)) {
									// если нету описания устройства с такими секцией и локомотивом, то добавим:
									_deviceInformationStorage.Add(new IdentifierStringToLowerBased(sourceSection.DeviceInformationId.UnicString), sourceLocomotive.Name, sourceSection.Name);
									}
								// localDevInfo - необходим для правильного указания deviceInformationId у сохроняемого psnDataInformtation
								var localDevInfo = _deviceInformationStorage.DeviceInformations.First(di => di.Name == sourceLocomotive.Name && di.Description == sourceSection.Name);

									double percent1 = perLogPercent;
									_psnDataStorage.Add(
										new IdentifierStringToLowerBased(psnLog.Id.UnicString),
										new StreamReadableDataBasedOnObject(psnLog),
										progressPercentage => {
										// still the same thread
										lastInLogProgressRaw = progressPercentage;
											var progress = percent1 + progressPercentage / pLogs.Count;
											lastInLogProgress = progress;
											_uiNotifier.Notify(() => onProgressChange(new OnProgressChangeEventArgs((int)progress)));
										});

									_psnDataInformtationStorage.Add(
										new IdentifierStringToLowerBased(psnLog.Id.UnicString),
										psnLog.BeginTime,
										psnLog.EndTime,
										DateTime.Now,
										new PsnDataFragmentTypeBuilderFromHighLevel(psnLog.LogType).Build(),
										psnLog.IsLastDeviceLog,
										localDevInfo.Id);

									_psnDataCustomConfigurationsStorage.Add(
										new IdentifierStringToLowerBased(psnLog.Id.UnicString),
										new IdentifierStringToLowerBased(psnLog.PsnConfiguration.Id.ToString().ToLower()),
										psnLog.Name);
								}
								totalProceedCount++;
								perLogPercent = totalProceedCount * 100.0 / pLogs.Count;
								double percent2 = perLogPercent;
								_uiNotifier.Notify(() => onProgressChange(new OnProgressChangeEventArgs((int)percent2)));
							}
							UpdateLocomotivesIfAddedUnsafe();

							_uiNotifier.Notify(() => onComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Ok, "Данные успешно сохранены")));
						}
						catch (Exception ex) {
							var message = "Ошибка сохранения данных: " + ex.Message;
							_uiNotifier.Notify(() => onComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Error, message)));
						}
					});
			}
		}

		public bool IsExist(IFaultLog rpdLog) {
			return false;
		}

		public bool IsExist(IPsnLog psnLog) {
			if (!IsOpened) {
				throw new Exception("Сперва необходимо открыть репозиторий!");
			}
			// TODO: We assumed calling from UI thread
			return _locomotives.Any(locomotive => locomotive.Sections.Any(section => section.Psns.Any(log => log.Id.ToString() == psnLog.Id.ToString())));
		}

		public bool IsOpened {
			get { lock (_sync) return _isOpened; }
			set { lock (_sync) _isOpened = value; }
		}

		public override string ToString() {
			return _repoName;
		}
	}
}
