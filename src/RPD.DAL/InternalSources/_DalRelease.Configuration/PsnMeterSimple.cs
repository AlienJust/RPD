using System.Collections.ObjectModel;
using RPD.DAL;

namespace RPD.DalRelease.Configuration
{
	class PsnMeterSimple : IPsnMeter
	{
		public string Name { get; set; }
		public ObservableCollection<IPsnChannel> Channels { get; set; }
	}
}
