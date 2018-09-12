using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using RPD.Presentation.Contracts.Model;
using RPD.Presentation.Contracts.Repositories;
using RPD.Presentation.Model;

namespace RPD.Presentation.Repositories {
	/// <summary>
	/// Реализует IDeviceInfoRepository для хранения в файле.
	/// </summary>
	internal class DeviceInfoRepository : IDeviceInfoRepository {
		private readonly string _fileName;
		readonly IList<DeviceInfo> _list;

		public DeviceInfoRepository(string fileName) {
			_fileName = fileName;
			_list = File.Exists(fileName) ? JsonConvert.DeserializeObject<IList<DeviceInfo>>(File.ReadAllText(fileName)) : new List<DeviceInfo>();
		}

		public IDeviceInfo GetByDeviceNumber(int deviceNumber) {
			return _list.FirstOrDefault(x => x.DeviceNumber == deviceNumber);
		}

		public void AddOrUpdate(IDeviceInfo deviceInfo) {
			var entity = _list.FirstOrDefault(x => x.DeviceNumber == deviceInfo.DeviceNumber);
			if (entity == null) {
				entity = new DeviceInfo();
				_list.Add(entity);
			}

			entity.DeviceNumber = deviceInfo.DeviceNumber;
			entity.LocomotiveName = deviceInfo.LocomotiveName;
			entity.SectionNumber = deviceInfo.SectionNumber;

			var outputJson = JsonConvert.SerializeObject(_list, Formatting.Indented);
			File.WriteAllText(_fileName, outputJson);
		}
	}
}
