using System;
using System.Collections.Generic;
using RPD.DAL;

namespace RPD.Presentation.Mocks.DAL {
	public class PsnConfigurationMock : IPsnConfiguration {
		public PsnConfigurationMock(string version, string name, string desc, Guid id) {
			Name = name;
			Version = version;
			Description = desc;
			Id = id;
		}

		public Guid Id { get; }
		public int RpdId { get; private set; }
		public string Name { get; }
		public string Version { get; }
		public string Description { get; }

		public IEnumerable<IPsnMeterConfig> PsnMeterConfigs { get; private set; }
	}
}