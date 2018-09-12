using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using RPD.Presentation.Contracts.Repositories;

namespace RPD.Presentation.Repositories {
	class DeviceNumberToPsnConfigurationRepository : IDeviceNumberToPsnConfigurationRepository {
		private readonly string _fileName;

		public DeviceNumberToPsnConfigurationRepository(string fileName) {
			_fileName = fileName;
			Configurtions = new Dictionary<int, string>();

			if (File.Exists(fileName))
				Load();
		}

		private void Load() {
			Configurtions = JsonConvert.DeserializeObject<Dictionary<int, string>>(File.ReadAllText(_fileName));
		}

		public IDictionary<int, string> Configurtions { get; private set; }

		public void Save() {
			var outputJson = JsonConvert.SerializeObject(Configurtions, Formatting.Indented);
			File.WriteAllText(_fileName, outputJson);
		}
	}
}