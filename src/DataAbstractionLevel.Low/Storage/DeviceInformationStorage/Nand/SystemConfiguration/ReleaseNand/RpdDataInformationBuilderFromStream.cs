using System.IO;
using AlienJust.Support.IO;
using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Common;
using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts;

namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.ReleaseNand {
	class RpdDataInformationBuilderFromStream : IRpdDataInformationBuilder {
		private readonly Stream _stream;

		public RpdDataInformationBuilderFromStream(Stream stream)
		{
			_stream = stream;
		}

		public IRpdDataInformation Build()
		{
			var rpdLogInfo = new RpdDataInformationSimple();

			var br = new AdvancedBinaryReader(_stream, false);
			rpdLogInfo.Number = br.ReadByte();
			rpdLogInfo.Status = br.ReadByte();
			rpdLogInfo.Day = br.ReadByte();
			rpdLogInfo.Month = br.ReadByte();
			rpdLogInfo.Year = br.ReadByte();
			rpdLogInfo.Hour = br.ReadByte();
			rpdLogInfo.Minute = br.ReadByte();
			rpdLogInfo.Second = br.ReadByte();

			rpdLogInfo.DescriptionPageAddress = br.ReadInt32();
			rpdLogInfo.LastWrittenPageAddress = br.ReadInt32();

			rpdLogInfo.FaultWasReaded = br.ReadByte();
			rpdLogInfo.BadPageCounter = br.ReadInt16();
			return rpdLogInfo;
		}
	}
}