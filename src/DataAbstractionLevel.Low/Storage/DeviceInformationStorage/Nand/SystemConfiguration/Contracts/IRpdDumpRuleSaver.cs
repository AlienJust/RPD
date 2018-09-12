namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts {
	interface IRpdDumpRuleSaver {
		void Save(IRpdDumpRule rule);
	}
}