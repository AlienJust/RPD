using System;
using System.IO;
using AlienJust.Support.IO;
using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Common;
using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts;

namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.ReleaseNand {
	internal class PsnDataFragmentInformationBuilderFromStream : IPsnDataFragmentInformationBuilder {
		private readonly Stream _stream;

		public PsnDataFragmentInformationBuilderFromStream(Stream stream) {
			_stream = stream;
		}

		public IPsnDataFragmentInformation Build()
		{
			var br = new AdvancedBinaryReader(_stream, false);
			var pageOffset = br.ReadUInt32(); // Номер страницы

			// 6 байт даты, если она была известна к началу фрагмента:
			var day = br.ReadByte();
			var month = br.ReadByte();
			var year = 2000 + br.ReadByte();

			var hour = br.ReadByte();
			var minute = br.ReadByte();
			var second = br.ReadByte();

			DateTime? time;
			try {
				time = new DateTime(year, month, day, hour, minute, second);
			}
			catch {
				time = null;
			}

			return new PsnDataFragmentInformationSimple {StartOffset = (int) pageOffset, StartTime = time};
		}
	}
}