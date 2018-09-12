using System.IO;
using AlienJust.Support.IO;
using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Common;
using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts;

namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.ReleaseNand {
	class RpdMeterSystemInformationBuilderFromStream : IRpdMeterSystemInformationBuilder
	{
		const int MaxChannels = 16;
		private readonly Stream _stream;
		
		public RpdMeterSystemInformationBuilderFromStream(Stream stream)
		{
			_stream = stream;
		}

		public IRpdMeterSystemInformation Build()
		{
			var rpdMeterInfo = new RpdMeterSystemInformationSimple();

			var br = new AdvancedBinaryReader(_stream, false);
			rpdMeterInfo.Address = br.ReadByte();
			rpdMeterInfo.Type = br.ReadByte();
			rpdMeterInfo.Status = br.ReadByte();
			rpdMeterInfo.LinkErrorCounter = br.ReadByte();

			rpdMeterInfo.ChannelsMask = br.ReadUInt16();
			rpdMeterInfo.ChannelsMaskFromMeter = br.ReadUInt16();
			rpdMeterInfo.ChannelsCount = br.ReadByte();
			rpdMeterInfo.ChannelsDumpedCount = br.ReadByte();

			var channelsDumpRulesCodes = new byte[MaxChannels]; // 0 - unused, 1..47 - valid
			br.Read(channelsDumpRulesCodes, 0, MaxChannels);
			rpdMeterInfo.ChannelsDumpRulesCodes = channelsDumpRulesCodes;

			rpdMeterInfo.HigherReadedFaultNumber = br.ReadByte();
			rpdMeterInfo.ReadedFaultsCount = br.ReadByte();
			rpdMeterInfo.NumberOfFaultDumpsForMeter = br.ReadByte();
			rpdMeterInfo.PageLine = br.ReadUInt32();
			rpdMeterInfo.PageLinesCountPerFault = br.ReadUInt32();
			rpdMeterInfo.Crc = br.ReadByte();
			return rpdMeterInfo;
		}
	}
}