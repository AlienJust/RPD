using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts;

namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Common {
	public sealed class SystemConfigurationBuilderDefault : ISystemConfigurationBuilder {
		public ISystemConfiguration BuildConfiguration() {
			return new SystemConfigurationSimple();
		}
	}
}