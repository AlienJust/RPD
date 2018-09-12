using System.Collections.Generic;

namespace RPD.DAL.PsnConfiguration.SimpleReleaseAndExtensions {
	class PsnVirtualDeviceSimple : IPsnVirtualDevice {
		private readonly string _name;
		private readonly IEnumerable<IPsnVirtualParameter> _parameters;
		public PsnVirtualDeviceSimple(string name, IEnumerable<IPsnVirtualParameter> parameters) {
			_name = name;
			_parameters = parameters;
		}

		public string Name {
			get { return _name; }
		}

		public IEnumerable<IPsnVirtualParameter> Parameters {
			get { return _parameters; }
		}
	}
}