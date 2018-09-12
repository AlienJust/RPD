using System.IO;
using AlienJust.Support.IO;
using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Common;
using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts;

namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.ReleaseNand {
	class RpdDumpRuleBuilderFromStream : IRpdDumpRuleBuilder {
		private readonly Stream _stream;

		public RpdDumpRuleBuilderFromStream(Stream stream) {
			_stream = stream;
		}

		public IRpdDumpRule Build() {
			var rule = new RpdDumpRuleSimple();
			var br = new AdvancedBinaryReader(_stream, false);
			rule.Type = br.ReadByte();
			rule.ControlValue = br.ReadInt16();
			rule.MaxValue = br.ReadInt16();
			rule.MinValue = br.ReadInt16();
			return rule;
		}
	}
}