using System;
using System.IO;
using DataAbstractionLevel.Low.Storage.Contracts;
using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand;
using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts;
using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.ReleaseNand;
using RPD.DAL.RpdConfiguration;
using RPD.DAL.WholeDeviceConfiguration.RelayOnLowLevel;
using RPD.DalRelease.ConfigurationSaver.Contracts;

namespace RPD.DAL.DalRelease.Configuration
{
	class DeviceConfigurationSaverToStorage: IDeviceConfigurationSaver {
		private readonly string _configurationDestinationDirectoryPath;
		public DeviceConfigurationSaverToStorage(string configurationDestinationDirectoryPath) {
			_configurationDestinationDirectoryPath = configurationDestinationDirectoryPath;
		}

		public void SaveConfiguration(IDeviceConfiguration deviceConfiguration) {
			var sysConfFileName = Path.Combine(_configurationDestinationDirectoryPath, "SYSCONF.BIN");
			if (File.Exists(sysConfFileName)) throw new Exception("SYSCONF.BIN file allready exist in destination directory");
			
			var rpdConfFileName = Path.Combine(_configurationDestinationDirectoryPath, "RPDCONF.BIN");
			if (File.Exists(rpdConfFileName)) throw new Exception("RPDCONF.BIN file allready exist in destination directory");

			/*using (var stream = File.Create(sysConfFileName)) {
				var fileMask = new byte[106496]; // 104kB for SYSCONF.BIN file
				stream.Write(fileMask, 0, fileMask.Length); 
				stream.Seek(0, SeekOrigin.Begin);
				stream.Close();
			}*/

			ISystemConfiguration sysconfig = new SystemConfigurationBuilderFromHighLevelDeviceConfiguration(deviceConfiguration, 0).BuildConfiguration();
			ISystemConfigurationSaver sysconfigSaver = new SystemConfigurationSaverBinary(sysConfFileName, true);
			sysconfigSaver.SaveConfiguration(sysconfig);

			ICustomConfiguration cusconfig = new CustomConfigurationBuilderFromHighDeviceConfig(deviceConfiguration).BuildConfiguration();
			ICustomConfigurationSaver cusconfigSaver = new CustomConfigurationSaverToRpdConfFile(rpdConfFileName);
			cusconfigSaver.SaveConfiguration(cusconfig);
		}
	}
}
