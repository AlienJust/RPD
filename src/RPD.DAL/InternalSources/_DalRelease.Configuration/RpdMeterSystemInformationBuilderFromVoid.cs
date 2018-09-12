using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Common;
using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts;

namespace RPD.DalRelease.Configuration {
	internal class RpdMeterSystemInformationBuilderFromVoid : IRpdMeterSystemInformationBuilder
	{
		private const int MaxChannels = 16;

		public IRpdMeterSystemInformation Build()
		{
			var rpdMeterInfo = new RpdMeterSystemInformationSimple {
				Address = 0xFF,
				Type = 0xFF,
				Status = 0xFF,
				LinkErrorCounter = 0xFF,
				ChannelsMask = 0xFFFF,
				ChannelsMaskFromMeter = 0xFFFF,
				ChannelsCount = 0xFF,
				ChannelsDumpedCount = 0xFF,
				ChannelsDumpRulesCodes = new byte[MaxChannels], 
				HigherReadedFaultNumber = 0xFF, 
				ReadedFaultsCount = 0xFF, 
				NumberOfFaultDumpsForMeter = 0xFF, 
				PageLine = 0xFFFFFFFF, 
				PageLinesCountPerFault = 0xFFFFFFFF, 
				Crc = 0
			};

			for (int i = 0; i < MaxChannels; i++)
				rpdMeterInfo.ChannelsDumpRulesCodes[i] = 0xFF;
			return rpdMeterInfo;
		}
	}
}