using System.Collections.Generic;
using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts;

namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Common {
	public class RpdMeterSystemInformationSimple : IRpdMeterSystemInformation
	{
		public int Address { get; set; }
		public int Type { get; set; }
		public int Status { get; set; }
		public int LinkErrorCounter { get; set; }
		public int ChannelsMask { get; set; }
		public int ChannelsMaskFromMeter { get; set; }
		public int ChannelsCount { get; set; }
		public int ChannelsDumpedCount { get; set; }
		public IList<byte> ChannelsDumpRulesCodes { get; set; }
		public int HigherReadedFaultNumber { get; set; }
		public int ReadedFaultsCount { get; set; }
		public int NumberOfFaultDumpsForMeter { get; set; }
		public uint PageLine { get; set; }
		public uint PageLinesCountPerFault { get; set; }
		public int Crc { get; set; }
	}
}