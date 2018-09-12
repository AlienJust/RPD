using System.Collections.Generic;
using System.Linq;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.Contracts;
using DataAbstractionLevel.Low.Storage.Shared;

namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage
{
	public class DeviceInformationStorageRelayMemoryCache : IDeviceInformationStorage
	{
		private readonly IDeviceInformationStorage _relayOnStorage;
		private readonly List<IDeviceInformationWritable> _deviceInformations;

		public DeviceInformationStorageRelayMemoryCache(IDeviceInformationStorage relayOnStorage)
		{
			_relayOnStorage = relayOnStorage;
			_deviceInformations = _relayOnStorage.DeviceInformations.Select(di=>(IDeviceInformationWritable)new DeviceInformationSimple(di.Name, di.Description, di.Id)).ToList();
		}

		public IEnumerable<IDeviceInformation> DeviceInformations {
			get { return _deviceInformations; }
		}

		public void Add(IIdentifier id, string name, string description)
		{
			_relayOnStorage.Add(id, name, description);
			_deviceInformations.Add(new DeviceInformationSimple(name, description, id));
		}

		public void Remove(IIdentifier id)
		{
			_relayOnStorage.Remove(id);
			var devInfosToRemove = _deviceInformations.Where(di => di.Id.IdentyString == id.IdentyString).ToList();
			foreach (var deviceInformation in devInfosToRemove) {
				_deviceInformations.Remove(deviceInformation);
			}
		}

		public void Update(IIdentifier id, string setName, string setDescription)
		{
			_relayOnStorage.Update(id, setName, setDescription);
			var devInfosToUpdate = _deviceInformations.Where(di => di.Id.IdentyString == id.IdentyString);
			foreach (var devInfo in devInfosToUpdate) {
				devInfo.SetName(setName);
				devInfo.SetDescription(setDescription);
			}
		}
	}
}
