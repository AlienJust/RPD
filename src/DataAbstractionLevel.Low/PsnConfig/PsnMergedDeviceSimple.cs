using System.Collections.Generic;
using DataAbstractionLevel.Low.PsnConfig.Contracts;

namespace DataAbstractionLevel.Low.PsnConfig
{
	class PsnMergedDeviceSimple : IPsnMergedDevice {
		private readonly string _name;
		private readonly IEnumerable<IPsnMergedParameter> _parameters;
		public PsnMergedDeviceSimple(string name, IEnumerable<IPsnMergedParameter> parameters) {
			_name = name;
			_parameters = parameters;
		}

		public string Name {
			get { return _name; }
		}

		public IEnumerable<IPsnMergedParameter> Parameters {
			get { return _parameters; }
		}
	}
}