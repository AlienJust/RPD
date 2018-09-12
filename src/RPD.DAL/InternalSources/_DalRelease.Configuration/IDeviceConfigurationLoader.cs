using RPD.DAL;

namespace RPD.DalRelease.Configuration {
	internal interface IDeviceConfigurationLoader {
		IDeviceConfiguration Load();
	}
}