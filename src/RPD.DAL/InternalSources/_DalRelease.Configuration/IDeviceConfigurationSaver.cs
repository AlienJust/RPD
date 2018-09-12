using RPD.DAL;

namespace RPD.DalRelease.ConfigurationSaver.Contracts {
	internal interface IDeviceConfigurationSaver {
		void SaveConfiguration(IDeviceConfiguration configuration);
	}
}