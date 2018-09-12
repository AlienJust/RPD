using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using AlienJust.Support.Identy;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.Contracts;
using DataAbstractionLevel.Low.Storage.Shared;

namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand {
	// TODO: create IStreamable object to get stream and use it below (need to merge classes for zipStorage and fileStorage)
	public sealed class DeviceInformationStorageNand : IDeviceInformationStorage {
		private readonly string _directoryPath;
		private readonly string _deviceId;

		public DeviceInformationStorageNand(string nandDirectoryPath, string deviceId) {
			_directoryPath = nandDirectoryPath;
			_deviceId = deviceId;
		}

		public IEnumerable<IDeviceInformation> DeviceInformations {
			get {
				ICustomConfiguration customConfig;
				try {
					var customConfigLoader = new CustomConfigurationBuilderFromRpdConfFile(Path.Combine(_directoryPath, "RPDCONF.BIN"));
					customConfig = customConfigLoader.BuildConfiguration();
				}
				catch {
					// Если произошла ошибка при работе со своей конфигурацией - загружается конфигурация по умолчанию
					customConfig = new CustomConfigurationBuilderDefault().BuildConfiguration();
				}
				return new IDeviceInformation[] {
					new DeviceInformationSimple(
						customConfig.LocomotiveName,
						customConfig.SectionNumber.ToString(CultureInfo.InvariantCulture),
						new IdentifierStringToLowerBased(_deviceId))
				};
			}
		}

		public void Add(IIdentifier id, string name, string description) {
			throw new NotSupportedException("Не возможно добавить новую конфигурацию на устройство");
		}

		public void Remove(IIdentifier id) {
			throw new NotSupportedException("Не возможно удалить конфигурацию на устройстве");
		}

		public void Update(IIdentifier id, string setName, string setDescription) {
			if (id.IdentyString == _deviceId) {
				ICustomConfiguration customConfig;
				try {
					var customConfigLoader = new CustomConfigurationBuilderFromRpdConfFile(Path.Combine(_directoryPath, "RPDCONF.BIN"));
					customConfig = customConfigLoader.BuildConfiguration();
				}
				catch {
					// Если произошла ошибка при работе со своей конфигурацией - загружается конфигурация по умолчанию
					customConfig = new CustomConfigurationBuilderDefault().BuildConfiguration();
				}
				customConfig.LocomotiveName = setName;
				customConfig.SectionNumber = int.Parse(setDescription);

				var saver = new CustomConfigurationSaverToRpdConfFile(Path.Combine(_directoryPath, "RPDCONF.BIN"));
				saver.SaveConfiguration(customConfig);
			}
			// TODO: update RPDCONF.BIN :)
			//throw new NotSupportedException("Еще не сделано");
		}
	}
}