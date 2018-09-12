using System.Collections.Generic;
using AlienJust.Support.Identy;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.Contracts;
using DataAbstractionLevel.Low.Storage.Shared;

namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage {
	public class DeviceInformationStorageCustomSingle : IDeviceInformationStorage {
		private readonly string _name;
		private readonly string _description;
		private readonly string _deviceId;

		public DeviceInformationStorageCustomSingle(string name, string description, string deviceId) {
			_name = name;
			_description = description;
			_deviceId = deviceId;
		}

		public IEnumerable<IDeviceInformation> DeviceInformations => new IDeviceInformation[] {
			new DeviceInformationSimple(
				_name,
				_description,
				new IdentifierStringToLowerBased(_deviceId))
		};

		public void Add(IIdentifier id, string name, string description) {
			throw new System.NotImplementedException();
		}

		public void Remove(IIdentifier id) {
			throw new System.NotImplementedException();
		}

		public void Update(IIdentifier id, string setName, string setDescription) {
			throw new System.NotImplementedException();
		}
	}
}
