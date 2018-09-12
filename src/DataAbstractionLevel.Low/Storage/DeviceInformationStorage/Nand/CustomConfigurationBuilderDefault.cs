using System.Collections.Generic;
using DataAbstractionLevel.Low.Storage.Contracts;

namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand {
	public class CustomConfigurationBuilderDefault : ICustomConfigurationBuilder {
		public ICustomConfiguration BuildConfiguration() {
			return new CustomConfigurationSimple {LocomotiveName = "Локомотив", SectionNumber = 1, PsnVersion = 5, RpdMetersCustomInfos = new List<IRpdMeterCustomInfo>()};
		}
	}
}