namespace RPD.DalRelease.Configuration.CommandConfiguration {
	enum ChannelConfigVerificationStatus
	{
		NoMeterFoundInTable,
		VerificationSuccess,
		ErrorDuringConfigurationWriting,
		NoLinkWithMeter,
		VerificationInProgress,
		Unknown
	}
}