using DataAbstractionLevel.Low.PsnConfig.Contracts;

namespace DataAbstractionLevel.Low.PsnConfig {
	class PsnSignalAddressSimple : IPsnProtocolDeviceSignalAddress {
		private readonly IPsnProtocolParameterConfiguration _parameterConfiguration;
		private readonly IPsnProtocolCommandPartConfiguration _commandPart;

		public PsnSignalAddressSimple(IPsnProtocolParameterConfiguration parameterConfiguration, IPsnProtocolCommandPartConfiguration commandPart)
		{
			_parameterConfiguration = parameterConfiguration;
			_commandPart = commandPart;
		}

		public IPsnProtocolParameterConfiguration ParameterConfiguration
		{
			get { return _parameterConfiguration; }
		}

		public IPsnProtocolCommandPartConfiguration CommandPart
		{
			get { return _commandPart; }
		}
	}
}