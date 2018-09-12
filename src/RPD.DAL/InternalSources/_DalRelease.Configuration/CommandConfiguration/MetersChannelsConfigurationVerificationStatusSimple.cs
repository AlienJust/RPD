namespace RPD.DalRelease.Configuration.CommandConfiguration {
	class MetersChannelsConfigurationVerificationStatusSimple : IMetersChannelsConfigurationVerificationStatus {
		private readonly int _zeroBasedMeterIndex;
		private readonly ChannelConfigVerificationStatus _channelConfigurationVerificationStatus;

		public MetersChannelsConfigurationVerificationStatusSimple(int zeroBasedMeterIndex, ChannelConfigVerificationStatus channelConfigurationVerificationStatus) {
			_zeroBasedMeterIndex = zeroBasedMeterIndex;
			_channelConfigurationVerificationStatus = channelConfigurationVerificationStatus;
		}

		public int ZeroBasedMeterIndex {
			get { return _zeroBasedMeterIndex; }
		}

		public ChannelConfigVerificationStatus ChannelConfigurationVerificationStatus {
			get { return _channelConfigurationVerificationStatus; }
		}
	}
}