using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using RPD.DAL;

namespace RPD.Presentation.Mocks.DAL {
	class PsnLogMock : IPsnLog {
		private PsnLogIntegrity _logIntegrity;

		public PsnLogMock(ISection owner, PsnLogType logType, IPsnConfiguration psnConfiguration) {
			Meters = new ObservableCollection<IPsnMeter>();
			BeginTime = DateTime.Now - TimeSpan.FromDays(10);
			EndTime = BeginTime + TimeSpan.FromMinutes(10);

			SaveTime = DateTime.Now;

			LogType = logType;
			PsnConfiguration = psnConfiguration;
		}


		public void Update(IPsnConfiguration psnConfig, string customName) {
			throw new NotImplementedException();
		}

		public void GetStatisticAsync(Action<Exception, IEnumerable<string>> callback) {
			throw new NotImplementedException();
		}

		public DateTime? BeginTime { get; }
		public DateTime? EndTime { get; }
		public DateTime? SaveTime { get; }
		public PsnLogType LogType { get; }
		public bool IsLastDeviceLog { get; private set; }
		public ObservableCollection<IPsnMeter> Meters { get; }
		public IPsnConfiguration PsnConfiguration { get; }

		public PsnLogIntegrity LogIntegrity {
			get => _logIntegrity;
			set {
				_logIntegrity = value;

				var h = IsSomethingWrongWithLogChanged;
				h?.Invoke(null, new System.EventArgs());
			}
		}

		public string Name { get; private set; }

		public event EventHandler IsSomethingWrongWithLogChanged;
		public Stream GetStreamForReading() {
			throw new NotImplementedException();
		}

		public IUid Id { get; private set; }
	}
}
