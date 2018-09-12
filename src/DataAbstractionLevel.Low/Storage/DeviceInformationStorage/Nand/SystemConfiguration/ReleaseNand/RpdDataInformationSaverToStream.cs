using System.IO;
using AlienJust.Support.IO;
using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts;

namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.ReleaseNand {
	class RpdDataInformationSaverToStream : IRpdDataInformationSaver
	{
		private readonly Stream _stream;

		public RpdDataInformationSaverToStream(Stream stream)
		{
			_stream = stream;
		}

		public void Save(IRpdDataInformation rpdLogInformation)
		{
			var bw = new AdvancedBinaryWriter(_stream, false);
			bw.Write((byte)rpdLogInformation.Number);
			bw.Write((byte)rpdLogInformation.Status);
			bw.Write((byte)rpdLogInformation.Day);
			bw.Write((byte)rpdLogInformation.Month);
			bw.Write((byte)rpdLogInformation.Year);
			bw.Write((byte)rpdLogInformation.Hour);
			bw.Write((byte)rpdLogInformation.Minute);
			bw.Write((byte)rpdLogInformation.Second);
			bw.Write((int)rpdLogInformation.DescriptionPageAddress);
			bw.Write((int)rpdLogInformation.LastWrittenPageAddress);
			bw.Write((byte)rpdLogInformation.FaultWasReaded);
			bw.Write((short)rpdLogInformation.BadPageCounter);
		}
	}
}