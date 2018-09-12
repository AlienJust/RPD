using System.IO;
using AlienJust.Support.IO;
using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts;

namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.ReleaseNand {
	class RpdDumpRuleSaverToStream : IRpdDumpRuleSaver {
		private readonly Stream _stream;

		public RpdDumpRuleSaverToStream(Stream stream)
		{
			_stream = stream;
		}

		public void Save(IRpdDumpRule rule) {
			var bw = new AdvancedBinaryWriter(_stream, false);
			bw.Write((byte)rule.Type);
			bw.Write((short)rule.ControlValue);
			bw.Write((short)rule.MaxValue);
			bw.Write((short)rule.MinValue);
		}
	}
}