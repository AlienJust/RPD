using DataAbstractionLevel.Low.Storage.Contracts;

namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand {
	public class RpdChannelCustomInformationSimple : IRpdChannelCustomInformation
	{
		public string Name { get; set; }
		public int Number { get; set; }
		public bool IsEnabled { get; set; }
		public bool IsService { get; set; }
	}
}