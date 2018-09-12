using System.Collections.Generic;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.PsnConfig.Contracts;

namespace DataAbstractionLevel.Low.PsnConfig {
	internal class PsnProtocolConfigurationSimple : IPsnProtocolConfiguration {
		private readonly IPsnProtocolConfigurationInformtaion _information;
		private readonly IEnumerable<IPsnProtocolDeviceConfiguration> _psnDevicesConfigurations;
		private readonly IEnumerable<IPsnProtocolCommandPartConfigurationCycle> _cycleCommandParts;
		private readonly IEnumerable<IPsnProtocolCommandPartConfiguration> _commandParts;
		private readonly IEnumerable<IPsnProtocolCommandConfiguration> _commands;
		private readonly IEnumerable<IPsnMergedDevice> _mergedDevices;

		public PsnProtocolConfigurationSimple(
			IPsnProtocolConfigurationInformtaion information,
			IEnumerable<IPsnProtocolDeviceConfiguration> psnDevicesConfigurations,
			IEnumerable<IPsnProtocolCommandPartConfigurationCycle> cycleCommandParts,
			IEnumerable<IPsnProtocolCommandPartConfiguration> commandParts,
			IEnumerable<IPsnProtocolCommandConfiguration> commands,
			IEnumerable<IPsnMergedDevice> mergedDevices) {
			_information = information;
			_psnDevicesConfigurations = psnDevicesConfigurations;
			_cycleCommandParts = cycleCommandParts;
			_commandParts = commandParts;
			_commands = commands;
			_mergedDevices = mergedDevices;
		}

		public IPsnProtocolConfigurationInformtaion Information {
			get { return _information; }
		}

		public IEnumerable<IPsnProtocolDeviceConfiguration> PsnDevices {
			get { return _psnDevicesConfigurations; }
		}

		public IEnumerable<IPsnProtocolCommandPartConfigurationCycle> CycleCommandParts {
			get { return _cycleCommandParts; }
		}

		public IEnumerable<IPsnProtocolCommandPartConfiguration> CommandParts {
			get { return _commandParts; }
		}

		public IEnumerable<IPsnProtocolCommandConfiguration> Commands {
			get { return _commands; }
		}

		public IEnumerable<IPsnMergedDevice> MergedDevices {
			get { return _mergedDevices; }
		}

		public override string ToString() {
			return _information.ToString();
		}

		public IIdentifier Id {
			get { return _information.Id; }
		}
	}
}