namespace RPD.DAL.PsnConfiguration.SimpleReleaseAndExtensions {
	class PsnDeviceConfigurationSimple : IPsnDeviceConfiguration {
		private readonly string _name;
		private readonly int _address;

		public PsnDeviceConfigurationSimple(string name, int address) {
			_name = name;
			_address = address;
		}

		public string Name {
			get { return _name; }
		}

		public int Address {
			get { return _address; }
		}
	}
}