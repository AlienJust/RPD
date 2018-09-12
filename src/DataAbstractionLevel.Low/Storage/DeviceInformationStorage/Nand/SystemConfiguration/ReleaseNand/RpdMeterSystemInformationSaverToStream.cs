using System.IO;
using System.Linq;
using AlienJust.Support.IO;
using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts;

namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.ReleaseNand {
	class RpdMeterSystemInformationSaverToStream : IRpdMeterSystemInformationSaver
	{
		const int MaxChannels = 16;
		private readonly Stream _stream;

		public RpdMeterSystemInformationSaverToStream(Stream stream)
		{
			_stream = stream;
		}

		public void Save(IRpdMeterSystemInformation rpdMeterSystemInformation)
		{
			var bw = new AdvancedBinaryWriter(_stream, false);
			bw.Write((byte)rpdMeterSystemInformation.Address);
			bw.Write((byte)rpdMeterSystemInformation.Type);
			bw.Write((byte)rpdMeterSystemInformation.Status);
			bw.Write((byte)rpdMeterSystemInformation.LinkErrorCounter);

			bw.Write((ushort)rpdMeterSystemInformation.ChannelsMask);
			bw.Write((ushort)rpdMeterSystemInformation.ChannelsMaskFromMeter); // TODO: как быть? поидее не нужно переписывать
			bw.Write((byte)rpdMeterSystemInformation.ChannelsCount);
			bw.Write((byte)rpdMeterSystemInformation.ChannelsDumpedCount);
			bw.Write(rpdMeterSystemInformation.ChannelsDumpRulesCodes.ToArray(), 0, MaxChannels);

			bw.Write((byte)rpdMeterSystemInformation.HigherReadedFaultNumber);
			bw.Write((byte)rpdMeterSystemInformation.ReadedFaultsCount);
			bw.Write((byte)rpdMeterSystemInformation.NumberOfFaultDumpsForMeter);
			bw.Write((uint)rpdMeterSystemInformation.PageLine);
			bw.Write((uint)rpdMeterSystemInformation.PageLinesCountPerFault);
			bw.Write((byte)rpdMeterSystemInformation.CalculateCrc()); // перевычисляем КС по имеющимся данным
		}
	}
}