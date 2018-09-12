using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts;

namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Common {
	public class RpdDumpRuleSimple : IRpdDumpRule {
		public int Type { get; set; }
		public int ControlValue { get; set; }
		public int MaxValue { get; set; }
		public int MinValue { get; set; }
	}
}