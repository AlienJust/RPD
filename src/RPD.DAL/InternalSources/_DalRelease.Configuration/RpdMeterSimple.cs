using System.Collections.ObjectModel;
using RPD.DAL;

namespace RPD.DalRelease.Configuration {
	class RpdMeterSimple : IRpdMeter {
		public string Name { get; set; }
		public RpdMeterType Type { get; set; }
		public int Address { get; set; }
		public ObservableCollection<IRpdChannel> Channels { get; set; }
		public IFaultLog OwnerFault { get; set; }
	}
}