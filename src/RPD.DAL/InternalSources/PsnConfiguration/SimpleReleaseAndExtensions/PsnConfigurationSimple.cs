using System;
using System.Collections.Generic;

namespace RPD.DAL.PsnConfiguration.SimpleReleaseAndExtensions {
	class PsnConfigurationSimple : IPsnConfiguration {
		private readonly Guid _id;
		private readonly string _name;
		private readonly string _version;
		private readonly string _description;
		private readonly IEnumerable<IPsnMeterConfig> _psnMeterConfigs;
		private readonly int _rpdId;

		public PsnConfigurationSimple(Guid id, int rpdId, string name, string version, string description, IEnumerable<IPsnMeterConfig> psnMeterConfigs) {
			_id = id;
			_rpdId = rpdId;
			_name = name;
			_version = version;
			_description = description;
			_psnMeterConfigs = psnMeterConfigs;
		}

		public Guid Id {
			get { return _id; }
		}

		public int RpdId {
			get { return _rpdId; }
		}

		public string Name {
			get { return _name; }
		}

		public string Version {
			get { return _version; }
		}

		public string Description {
			get { return _description; }
		}

		public IEnumerable<IPsnMeterConfig> PsnMeterConfigs {
			get { return _psnMeterConfigs; }
		}
	}
}