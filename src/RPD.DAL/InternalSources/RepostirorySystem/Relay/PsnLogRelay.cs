using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using AlienJust.Support.Concurrent.Contracts;
using AlienJust.Support.Identy;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.InternalKitchen.KeyValueStringStorage.Contracts;
using DataAbstractionLevel.Low.PsnConfig.Contracts;
using DataAbstractionLevel.Low.Storage.Contracts;
using DataAbstractionLevel.Low.Storage.Shared;
using RPD.DAL.Common.SimpleRelease;
using RPD.DAL.KeyValueStorageHolders.Contracts;
using RPD.DAL.PsnDataExtraction.BasedOnLowLevel;
using RPD.DAL.RepostirorySystem.SimpleReleaseAndExtensions;
using RPD.Supports;

namespace RPD.DAL.RepostirorySystem.Relay {
	class PsnLogRelay : IPsnLog {
		private readonly IStorage<IPsnProtocolConfiguration> _psnProtocolStorage;
		private readonly IPsnDataCustomConfigurationsStorage _psnDataCustomConfigurationStorage;
		private readonly IPsnDataInformation _psnDataInformation;
		private readonly IPsnDataCustomConfigrationWritable _psnDataCustomConfigration; // TODO: public accessor
		private readonly IPsnData _psnData;
		private readonly IPsnDataPaged _psnDataPaged;
		private readonly IThreadNotifier _uiNotifier;
		private readonly IWorker<Action> _bworker; // to provide async loading of trend

		private IPsnConfiguration _psnConfiguration;
		private IPsnProtocolConfiguration _psnConfigurationLowLevel;

		private readonly ObservableCollection<IPsnMeter> _meters;
		private PsnLogIntegrity _logIntegrity;

		private readonly IKeyValueStringStorage _unicSignalIdStorage;

		public PsnLogRelay(
			IPsnDataInformation psnDataInformation, 
			IPsnData psnData, 
			IPsnConfiguration psnConfiguration, 
			IPsnProtocolConfiguration psnConfigurationLowLevel, 
			IPsnDataCustomConfigration psnDataCustomConfigration, 
			IThreadNotifier uiNotifier, 
			IWorker<Action> bworker,
			IStorage<IPsnProtocolConfiguration> psnProtocolStorage,
			IPsnDataCustomConfigurationsStorage psnDataCustomConfigurationStorage, 
			IKeyValueStorageHolder repoStorageHolder)
		{
			_logIntegrity = PsnLogIntegrity.Unknown;
			_psnDataInformation = psnDataInformation;
			_psnData = psnData;
			_psnDataPaged = _psnData.PagesInformation;

			_psnConfiguration = psnConfiguration;
			_psnConfigurationLowLevel = psnConfigurationLowLevel;

			_psnDataCustomConfigration = new PsnDataCustomConfigurationSimple(
				psnDataCustomConfigration.Id,
				psnDataCustomConfigration.PsnConfigurationId,
				psnDataCustomConfigration.CustomLogName);
			
			_uiNotifier = uiNotifier;
			_bworker = bworker;
			_psnProtocolStorage = psnProtocolStorage;
			_psnDataCustomConfigurationStorage = psnDataCustomConfigurationStorage;

			_meters = new ObservableCollection<IPsnMeter>();

			// TODO: i get logs for FTP logs also
			//_unicSignalIdStorage = new KeyValueStringStorageXmlMemCache(Path.Combine(Support.GetAppDataDirectoryPathAndCreateItIfNeeded(), Id.UnicString + ".Id.xml"));
			_unicSignalIdStorage = repoStorageHolder.GetStorage(Id.UnicString);
			//_unicSignalIdStorage = new KvStorageDbreezed(Id.UnicString);
			
			RebuildMetersUnsafe();
		}


		private void RebuildMetersUnsafe() {
			_meters.Clear();
			foreach (var dev in _psnConfigurationLowLevel.PsnDevices) {
				var channels = new ObservableCollection<IPsnChannel>();
				var meter = new PsnMeterSimple(dev.Name, channels);
				
				_psnConfigurationLowLevel.ForeachPsnMeterSignal(dev.Address, (cmdPartCfg, paramCfg) => {
					Guid chId;
					try
					{
						chId = Guid.Parse(_unicSignalIdStorage.GetValue(paramCfg.Id.IdentyString));
					}
					catch
					{
						// no value in storage or wrong value format:
						chId = Guid.NewGuid();
						_unicSignalIdStorage.SetValue(paramCfg.Id.IdentyString, chId.ToString());
					}
					try
					{
						channels.Add(
							new PsnChannelSimple(
								chId,
								Guid.Parse(paramCfg.Id.IdentyString), // can trhow exception
								paramCfg.Name,
								paramCfg.IsBitSignal ? TrendType.Discrete : TrendType.Analogue,
								new PsnChannelTrendLoaderSimple(paramCfg, cmdPartCfg, _psnData, _psnConfigurationLowLevel, _psnDataInformation),
								_uiNotifier,
								_bworker,
								meter));
					}
					catch
					{
						// TODO: remove empty catch
					}
					return false; // no stops needed
				});
				_meters.Add(meter);
			}

			if (_psnConfigurationLowLevel.MergedDevices == null) return;
			foreach (var dev in _psnConfigurationLowLevel.MergedDevices) {
				var channels = new ObservableCollection<IPsnChannel>();
				var meter = new PsnMeterSimple(dev.Name, channels);
				foreach (var parameter in dev.Parameters) {
					Guid chId;
					try
					{
						chId = Guid.Parse(_unicSignalIdStorage.GetValue(parameter.Id.IdentyString));
					}
					catch
					{
						// no value in storage or wrong value format:
						chId = Guid.NewGuid();
						_unicSignalIdStorage.SetValue(parameter.Id.IdentyString, chId.ToString());
					}
					channels.Add(
							new PsnChannelSimple(
								chId,
								Guid.Parse(parameter.Id.IdentyString), // can throw exception
								parameter.Name,
								TrendType.Analogue,
								new PsnChannelTrendLoaderMerged(parameter, _psnData, _psnConfigurationLowLevel, _psnDataInformation),
								_uiNotifier,
								_bworker,
								meter));
				}
				_meters.Add(meter);
			}
		}


		public IUid Id => new UidStringToLower(_psnData.Id.IdentyString);

		public DateTime? BeginTime => _psnDataInformation.BeginTime;

		public DateTime? EndTime => _psnDataInformation.EndTime;

		public DateTime? SaveTime => _psnDataInformation.SaveTime;

		public PsnLogType LogType => new PsnLogTypeBuilderFromLowLevel(_psnDataInformation.DataFragmentType).Build();

		public bool IsLastDeviceLog => _psnDataInformation.IsLastDeviceLog;

		public ObservableCollection<IPsnMeter> Meters => _meters;

		public void Update(IPsnConfiguration psnConfig, string customName)
		{
			// TODO: improve threading
			// Предполагается, что метод вызывается из потока UI

			_psnConfiguration = psnConfig;
			
			_psnDataCustomConfigration.SetPsnConfigurationId(new IdentifierStringToLowerBased(_psnConfiguration.Id.ToString().ToLower()));
			_psnDataCustomConfigration.SetCustomLogName(customName);

			_bworker.AddToQueueAndWaitExecution(() => {
				_psnDataCustomConfigurationStorage.Update(_psnDataCustomConfigration.Id, new IdentifierStringToLowerBased(_psnConfiguration.Id.ToString().ToLower()), customName);
				_psnConfigurationLowLevel = _psnProtocolStorage.StoredItems.First(lowConfig => lowConfig.Id.IdentyString == _psnConfiguration.Id.ToString().ToLower());
			});

			RebuildMetersUnsafe();
		}

		public IPsnConfiguration PsnConfiguration => _psnConfiguration;

		public void GetStatisticAsync(Action<Exception, IEnumerable<string>> callback) {
			_bworker.AddWork(() => {
				try {
					//var beginTime = _psnDataInformation.BeginTime.HasValue ? _psnDataInformation.BeginTime.Value : (_psnDataInformation.SaveTime.HasValue ? _psnDataInformation.SaveTime.Value : DateTime.Now);
					var pagesIndex = _psnDataPaged.GetPagesIndex();

					int totalPagesInLog = 0;
					int badPagesCount = 0;
					int notDatedPagesCount = 0;
					int normalPagesCount = 0;
					var timeBackPairs = new List<Tuple<IPsnPageIndexRecord, IPsnPageIndexRecord>>();
					var numberSkipPairs = new List<Tuple<IPsnPageIndexRecord, IPsnPageIndexRecord>>();

					IPsnPageIndexRecord prevRecord = null;
					foreach (var pageInfo in pagesIndex) {
						totalPagesInLog++;
						switch (pageInfo.PageInfo) {
							case PsnPageInfo.BadPage:
								badPagesCount++;
								break;
							case PsnPageInfo.NormalPage:
								normalPagesCount++;
								break;
						}
						if (!pageInfo.PageTime.HasValue) {
							notDatedPagesCount++;
						}

					
						if (prevRecord != null) {
							if (prevRecord.PageInfo == PsnPageInfo.NormalPage && pageInfo.PageInfo == PsnPageInfo.NormalPage) {
								if (pageInfo.PageNumber - prevRecord.PageNumber != 1 && !(pageInfo.PageNumber == 0 && prevRecord.PageNumber == 255))
								{
									numberSkipPairs.Add(new Tuple<IPsnPageIndexRecord, IPsnPageIndexRecord>(prevRecord, pageInfo));
								}

								if (prevRecord.PageTime.HasValue && pageInfo.PageTime.HasValue) {
									if (pageInfo.PageTime.Value < prevRecord.PageTime.Value)
										timeBackPairs.Add(new Tuple<IPsnPageIndexRecord, IPsnPageIndexRecord>(prevRecord, pageInfo));
								}
							}
						}

						prevRecord = pageInfo;
					}
					var result = new List<string> {
						"Страниц в логе: " + totalPagesInLog, 
						"Страниц, не распознанных как страницы данных ПСН, в логе: " + badPagesCount, 
						"Страниц с данными ПСН: " + normalPagesCount, 
						"Страниц без временной метки (из числа страниц с данными): " + notDatedPagesCount
					};


					var lineTime = "Число обратных временных переходов в логе: " + timeBackPairs.Count + ":  ";
					foreach (var pair in timeBackPairs) {
						lineTime += " 0x" + pair.Item1.AbsolutePositionInStream.ToString("X") + "-0x" + pair.Item2.AbsolutePositionInStream.ToString("X");
					}
					result.Add(lineTime);

					var lineBack = "Число скачков номера страницы: " + numberSkipPairs.Count + ":  ";
					foreach (var pair in numberSkipPairs)
					{
						lineBack += " 0x" + pair.Item1.AbsolutePositionInStream.ToString("X") + "-0x" + pair.Item2.AbsolutePositionInStream.ToString("X");
					}
					result.Add(lineBack);

					var isPagesFlowErrorAccured = timeBackPairs.Count > 1 || (timeBackPairs.Count <= 1 && numberSkipPairs.Count != timeBackPairs.Count);

					if (isPagesFlowErrorAccured) {
						if (_logIntegrity != PsnLogIntegrity.PagesFlowError) {
							_logIntegrity = PsnLogIntegrity.PagesFlowError;
							_uiNotifier.Notify(() => IsSomethingWrongWithLogChanged.SafeInvoke(this, new System.EventArgs()));
						}
					}
					else {
						if (_logIntegrity != PsnLogIntegrity.Ok) {
							_logIntegrity = PsnLogIntegrity.Ok;
							_uiNotifier.Notify(() => IsSomethingWrongWithLogChanged.SafeInvoke(this, new System.EventArgs()));
						}
					}
					_uiNotifier.Notify(() => {
						callback?.Invoke(null, result);
					});
				}
				catch (Exception ex) {
					_uiNotifier.Notify(() => {
						callback?.Invoke(ex, null);
					});
				}
				
			});
		}

		public PsnLogIntegrity LogIntegrity => _logIntegrity;

		public event EventHandler IsSomethingWrongWithLogChanged;

		public string Name => _psnDataCustomConfigration.CustomLogName;

		public Stream GetStreamForReading() {
			return _psnData.GetStreamForReading();
		}
	}
}