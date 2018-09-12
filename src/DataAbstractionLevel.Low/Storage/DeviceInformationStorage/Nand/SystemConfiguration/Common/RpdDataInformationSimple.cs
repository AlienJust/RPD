using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts;

namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Common {
	class RpdDataInformationSimple : IRpdDataInformation
	{
		public int Number { get; set; }
		public int Status { get; set; }
		public int Year { get; set; }
		public int Month { get; set; }
		public int Day { get; set; }
		public int Hour { get; set; }
		public int Minute { get; set; }
		public int Second { get; set; }
		public int DescriptionPageAddress { get; set; }
		public int LastWrittenPageAddress { get; set; }
		public int FaultWasReaded { get; set; }
		public int BadPageCounter { get; set; }
	}
}