using DataAbstractionLevel.Low.PsnConfig.Contracts;

namespace DataAbstractionLevel.Low.PsnConfig {
	class PsnSignalConfigurationSimple : IPsnProtocolDeviceSignalConfiguration {
		private readonly string _name;
		private readonly bool _isBooleanValued;
		private readonly IPsnProtocolDeviceSignalAddress _address;

		public PsnSignalConfigurationSimple(string name, bool isBooleanValued, IPsnProtocolDeviceSignalAddress address)
		{
			_name = name;
			_isBooleanValued = isBooleanValued;
			_address = address;
		}

		public string Name {
			get { return _name; }
		}

		public bool IsBooleanValued {
			get { return _isBooleanValued; }
		}

		public IPsnProtocolDeviceSignalAddress Address
		{
			get { return _address; }
		}
	}
}