using System.Collections.Generic;
using DataAbstractionLevel.Low.Storage.Contracts;

namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand {
	public class CustomConfigurationSimple : ICustomConfiguration {
		public string LocomotiveName { get; set; }
		public int SectionNumber { get; set; }
		public int PsnVersion { get; set; }
		public IList<IRpdMeterCustomInfo> RpdMetersCustomInfos { get; set; }
	}
}