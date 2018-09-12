namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts {
	interface IRpdMeterSystemInformationSaver
	{
		void Save(IRpdMeterSystemInformation rpdMeterSystemInformation);
	}
}