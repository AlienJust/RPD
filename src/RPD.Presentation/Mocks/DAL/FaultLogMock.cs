using System;
using System.Collections.ObjectModel;
using RPD.DAL;

namespace RPD.Presentation.Mocks.DAL {
	class FaultLogMock : IFaultLog {
		public FaultLogMock(ISection owner) {
			OwnerSection = owner;
			Signals = new ObservableCollection<ISignal>();
			RpdMeters = new ObservableCollection<IRpdMeter>();
			PsnFaultLog = new PsnLogMock(owner, PsnLogType.FixedLength,
					new PsnConfigurationMock("1.0", "Буровая", "Буровая прошивка", Guid.NewGuid()));
			AccuredAt = DateTime.Now - TimeSpan.FromDays(10);
		}

		public string Name { get; set; }
		public ObservableCollection<ISignal> Signals { get; }
		public ObservableCollection<IRpdMeter> RpdMeters { get; }
		public IPsnLog PsnFaultLog { get; }
		public DateTime AccuredAt { get; }
		public ISection OwnerSection { get; }
		public bool SavedToRepository { get; private set; }
		public IDeviceConfiguration DeviceConfig { get; private set; }
		public string LogReason { get; private set; }
	}
}
