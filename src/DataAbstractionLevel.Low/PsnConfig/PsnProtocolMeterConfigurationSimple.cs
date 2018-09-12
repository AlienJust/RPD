using DataAbstractionLevel.Low.PsnConfig.Contracts;

namespace DataAbstractionLevel.Low.PsnConfig {
	internal sealed class PsnProtocolMeterConfigurationSimple : IPsnProtocolMeterConfiguration {
		public string Name { get; set; }
		public int Address { get; set; }
	}
}