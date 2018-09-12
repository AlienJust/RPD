namespace DataAbstractionLevel.Low.Storage.Contracts {
	internal interface IDeviceInformationWritable : IDeviceInformation {
		void SetName(string name);
		void SetDescription(string description);
	}
}