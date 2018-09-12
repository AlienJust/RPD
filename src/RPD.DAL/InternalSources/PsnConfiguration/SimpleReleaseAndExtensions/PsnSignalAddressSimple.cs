namespace RPD.DAL.PsnConfiguration.SimpleReleaseAndExtensions {
	class PsnSignalAddressSimple : IPsnSignalAddress {
		private readonly IPsnParameterConfiguration _parameterConfiguration;
		private readonly IPsnCommandPartInfo _commandPart;
		public PsnSignalAddressSimple(IPsnParameterConfiguration parameterConfiguration, IPsnCommandPartInfo commandPart) {
			_parameterConfiguration = parameterConfiguration;
			_commandPart = commandPart;
		}

		public IPsnParameterConfiguration ParameterConfiguration {
			get { return _parameterConfiguration; }
		}

		public IPsnCommandPartInfo CommandPart {
			get { return _commandPart; }
		}
	}
}