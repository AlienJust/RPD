namespace RPD.DalRelease.Configuration.CommandConfiguration {
	internal interface IMetersChannelsConfigurationVerificationStatus {
		int ZeroBasedMeterIndex { get; }
		ChannelConfigVerificationStatus ChannelConfigurationVerificationStatus { get; }
	}
}