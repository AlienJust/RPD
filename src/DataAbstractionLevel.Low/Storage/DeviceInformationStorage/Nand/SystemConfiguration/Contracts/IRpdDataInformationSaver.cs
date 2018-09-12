namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts {
	interface IRpdDataInformationSaver {
		void Save(IRpdDataInformation rpdLogInformation);
	}
}