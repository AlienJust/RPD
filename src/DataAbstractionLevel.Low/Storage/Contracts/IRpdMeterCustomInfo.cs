using System.Collections.Generic;

namespace DataAbstractionLevel.Low.Storage.Contracts {
	public interface IRpdMeterCustomInfo {
		string Name { get; set; }
		int Address { get; set; }
		RpdProtocolMeterType MeterType { get; set; }
		IList<IRpdChannelCustomInformation> ChannelInfos { get; }
	}
}