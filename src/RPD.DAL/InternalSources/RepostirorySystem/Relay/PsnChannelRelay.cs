using System;
using System.Collections.Generic;
using System.Threading;
using AlienJust.Support.Concurrent.Contracts;
using RPD.DAL.Common.SimpleRelease;
using RPD.DAL.PsnDataExtraction.Contracts;
using RPD.EventArgs;

namespace RPD.DAL.RepostirorySystem.Relay {
	class PsnChannelSimple : IPsnChannel, IObjectWithId {
		private readonly object _sync;
		//private readonly IPsnProtocolDeviceSignalConfiguration _signalConfiguration;
		//private readonly IPsnData _psnData;
		//private readonly IPsnProtocolConfiguration _psnConfiguration;
		//private readonly IPsnDataInformation _psnDataInformation;

		private readonly IThreadNotifier _uiNotifier;
		private readonly IWorker<Action> _bworker;

		
		private readonly IPsnChannelTrendLoader _trendLoader;

		private bool _isTrendLoaded;
		private List<IDataPoint> _points;

		public PsnChannelSimple(Guid unicId, Guid configurationId, string channelName, TrendType chanleType, IPsnChannelTrendLoader trendLoader, IThreadNotifier uiNotifier, IWorker<Action> bworker, IPsnMeter ownerMeter)
		{
			_sync = new object();

			UnicId = unicId;
			ConfigurationId = configurationId;
			Name = channelName;
			Type = chanleType;
			_trendLoader = trendLoader;

			//_signalConfiguration = signalConfiguration;
			//_psnData = psnData;
			//_psnConfiguration = psnConfiguration;
			//_psnDataInformation = psnDataInformation;

			_uiNotifier = uiNotifier;
			_bworker = bworker;
			OwnerPsnMeter = ownerMeter;


			_points = new List<IDataPoint>();
			_isTrendLoaded = false;
		}

		public List<IDataPoint> Trend => _points;

		public TrendType Type { get; }

		public bool IsTrendLoaded {
			get { lock (_sync) return _isTrendLoaded; }
		}

		public void LoadTrendAsync(Action<OnCompleteEventArgs> onComplete) {
			_bworker.AddWork(
				() => {
					try
					{
						if (!IsTrendLoaded) {

							//var beginTime = _psnDataInformation.BeginTime.HasValue ? _psnDataInformation.BeginTime.Value : (_psnDataInformation.SaveTime.HasValue ? _psnDataInformation.SaveTime.Value : DateTime.Now);
							//var trend = _psnData.LoadTrend(_psnConfiguration, _signalConfiguration.Address, beginTime);
							var trend = _trendLoader.LoadTrend();
							lock (_sync) {
								_points = trend;
								_isTrendLoaded = true;
							}
						}
						if (onComplete != null) _uiNotifier.Notify(() => onComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Ok, "Тренд сигнала ПСН " + Name + " загружен")));
					}
					catch (Exception ex){
						lock (_sync)
						{
							_points.Clear();
							_isTrendLoaded = false;
						}
						if (onComplete != null) _uiNotifier.Notify(() => onComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Error, "Тренд сигнала ПСН " + Name + " не был загружен, причина: " + ex)));
					}
				});
		}

		public void FreeTrend() {
			//SYNC method:
			var waiter = new ManualResetEvent(false);
			Exception bwException = null;
			_bworker.AddWork(() => {
				try {
					lock (_sync) {
						_points.Clear();
						_points = null;
						_isTrendLoaded = false;
					}
					_trendLoader.FreeTrend();
				}
				catch (Exception ex) {
					bwException = ex;
				}
				finally {
					waiter.Set();
				}
			});
				
			waiter.WaitOne();
			if (bwException != null) throw bwException;
		}

		public string Name { get; }


		public Guid UnicId { get; }

		public Guid ConfigurationId { get; }


		public IUid Id => new UidStringToLower(UnicId.ToString());


		// TODO: below props must be setted somehow
		public bool IsEnabled => true;

		public bool IsInput => false;

		public bool CanBeFaultSign => true;

		public bool IsFaultSign => false;

		public IPsnMeter OwnerPsnMeter { get; }
	}
}