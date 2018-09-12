using System.Collections.ObjectModel;
using RPD.DAL;

namespace RPD.Presentation.Mocks.DAL {
	class PsnMeterMock : IPsnMeter {
		public PsnMeterMock() {
			Channels = new ObservableCollection<IPsnChannel>();
		}

		#region Implementation of IPsnMeter

		public string Name { get; set; }
		public ObservableCollection<IPsnChannel> Channels { get; }

		#endregion
	}
}
