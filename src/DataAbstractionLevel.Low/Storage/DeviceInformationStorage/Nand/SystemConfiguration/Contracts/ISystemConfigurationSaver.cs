namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts {
	public interface ISystemConfigurationSaver {
		void SaveConfiguration(ISystemConfiguration configuration);
	}
}