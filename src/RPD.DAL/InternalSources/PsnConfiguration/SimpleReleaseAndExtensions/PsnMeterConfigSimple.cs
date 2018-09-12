using System.Collections.Generic;

namespace RPD.DAL.PsnConfiguration.SimpleReleaseAndExtensions {
	class PsnMeterConfigSimple:IPsnMeterConfig {
		private readonly string _name;
		private readonly IEnumerable<IPsnChannelConfig> _psnChannelConfigs;
		public PsnMeterConfigSimple(string name, IEnumerable<IPsnChannelConfig> psnChannelConfigs) {
			_name = name;
			_psnChannelConfigs = psnChannelConfigs;
		}

		public string Name {
			get { return _name; }
		}

		public IEnumerable<IPsnChannelConfig> PsnChannelConfigs {
			get { return _psnChannelConfigs; }
		}
	}
}