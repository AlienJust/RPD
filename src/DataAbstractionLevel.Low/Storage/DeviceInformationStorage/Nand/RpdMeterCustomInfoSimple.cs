using System.Collections.Generic;
using DataAbstractionLevel.Low.Storage.Contracts;

namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand {
	public class RpdMeterCustomInfoSimple : IRpdMeterCustomInfo
	{
		public string Name { get; set; }
		public int Address { get; set; }
		public RpdProtocolMeterType MeterType { get; set; }
		public IList<IRpdChannelCustomInformation> ChannelInfos { get; set; }
	}
}