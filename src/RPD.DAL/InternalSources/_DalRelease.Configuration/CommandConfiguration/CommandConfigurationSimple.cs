using System;
using System.Collections.Generic;

namespace RPD.DalRelease.Configuration.CommandConfiguration {
	class CommandConfigurationSimple : ICommandConfiguration
	{
		private readonly DateTime? _deviceTime;
		private readonly bool _isLinkWithFramErrorDetected;
		private readonly bool _isLinkWithNandErrorDetected;
		private readonly bool _isMetersTableCsErrorDetected;
		private readonly bool _isDumpsTableCrc16ErrorDetected;
		private readonly bool _isMetersConfigurationComplete;
		private readonly bool _isMetersRequestingInProgress;
		private readonly bool _isMetersConfigurationWritingInProgress;
		private readonly bool _isMetersConfigurationReadingInProgress;
		private readonly bool _isMetersConfigurationReadedAndReadyInNandForVerification;
		private readonly bool _isMeterLinkErrorDetected;
		private readonly bool _isLinkWithMetersTestingComplete;
		private readonly IEnumerable<IMetersChannelsConfigurationVerificationStatus> _channelsConfigurationVerificationStatuses;
		private ushort _psnProtocolType;

		public CommandConfigurationSimple(DateTime? deviceTime, bool isLinkWithFramErrorDetected, bool isLinkWithNandErrorDetected, bool isMetersTableCsErrorDetected, bool isDumpsTableCrc16ErrorDetected, bool isMetersConfigurationComplete, bool isMetersRequestingInProgress, bool isMetersConfigurationWritingInProgress, bool isMetersConfigurationReadingInProgress, bool isMetersConfigurationReadedAndReadyInNandForVerification, bool isMeterLinkErrorDetected, bool isLinkWithMetersTestingComplete, IEnumerable<IMetersChannelsConfigurationVerificationStatus> channelsConfigurationVerificationStatuses, ushort psnProtocolType) {
			_deviceTime = deviceTime;
			_isLinkWithFramErrorDetected = isLinkWithFramErrorDetected;
			_isLinkWithNandErrorDetected = isLinkWithNandErrorDetected;
			_isMetersTableCsErrorDetected = isMetersTableCsErrorDetected;
			_isDumpsTableCrc16ErrorDetected = isDumpsTableCrc16ErrorDetected;
			_isMetersConfigurationComplete = isMetersConfigurationComplete;
			_isMetersRequestingInProgress = isMetersRequestingInProgress;
			_isMetersConfigurationWritingInProgress = isMetersConfigurationWritingInProgress;
			_isMetersConfigurationReadingInProgress = isMetersConfigurationReadingInProgress;
			_isMetersConfigurationReadedAndReadyInNandForVerification = isMetersConfigurationReadedAndReadyInNandForVerification;
			_isMeterLinkErrorDetected = isMeterLinkErrorDetected;
			_isLinkWithMetersTestingComplete = isLinkWithMetersTestingComplete;
			_channelsConfigurationVerificationStatuses = channelsConfigurationVerificationStatuses;
			_psnProtocolType = psnProtocolType;
		}

		public DateTime? DeviceTime {
			get { return _deviceTime; }
		}

		public bool IsLinkWithFRAMErrorDetected {
			get { return _isLinkWithFramErrorDetected; }
		}

		public bool IsLinkWithNANDErrorDetected {
			get { return _isLinkWithNandErrorDetected; }
		}

		public bool IsMetersTableCSErrorDetected {
			get { return _isMetersTableCsErrorDetected; }
		}

		public bool IsDumpsTableCRC16ErrorDetected {
			get { return _isDumpsTableCrc16ErrorDetected; }
		}

		public bool IsMetersConfigurationComplete {
			get { return _isMetersConfigurationComplete; }
		}

		public bool IsMetersRequestingInProgress {
			get { return _isMetersRequestingInProgress; }
		}

		public bool IsMetersConfigurationWritingInProgress {
			get { return _isMetersConfigurationWritingInProgress; }
		}

		public bool IsMetersConfigurationReadingInProgress {
			get { return _isMetersConfigurationReadingInProgress; }
		}

		public bool IsMetersConfigurationReadedAndReadyInNANDForVerification {
			get { return _isMetersConfigurationReadedAndReadyInNandForVerification; }
		}

		public bool IsMeterLinkErrorDetected {
			get { return _isMeterLinkErrorDetected; }
		}

		public bool IsLinkWithMetersTestingComplete {
			get { return _isLinkWithMetersTestingComplete; }
		}

		public IEnumerable<IMetersChannelsConfigurationVerificationStatus> ChannelsConfigurationVerificationStatuses {
			get { return _channelsConfigurationVerificationStatuses; }
		}

		public ushort PsnProtocolType {
			get { return _psnProtocolType; }
			set { _psnProtocolType = value; }
		}
	}
}