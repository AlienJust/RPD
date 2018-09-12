namespace RPD.DAL.PsnConfiguration.SimpleReleaseAndExtensions {
	class PsnSignalConfigurationSimple : IPsnSignalConfiguration {
		private readonly string _name;
		private readonly bool _isBooleanValued;
		private readonly IPsnSignalAddress _address;
		public PsnSignalConfigurationSimple(string name, bool isBooleanValued, IPsnSignalAddress address) {
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

		public IPsnSignalAddress Address {
			get { return _address; }
		}
	}
}