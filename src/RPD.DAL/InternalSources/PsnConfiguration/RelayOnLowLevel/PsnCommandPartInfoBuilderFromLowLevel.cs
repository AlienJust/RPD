using System.Linq;
using DataAbstractionLevel.Low.PsnConfig.Contracts;
using RPD.DAL.Common.Contracts;
using RPD.DAL.Common.SimpleRelease;
using RPD.DAL.PsnConfiguration.SimpleReleaseAndExtensions;

namespace RPD.DAL.PsnConfiguration.RelayOnLowLevel {
	class PsnCommandPartInfoBuilderFromLowLevel : IBuilder<IPsnCommandPartInfo> {
		private readonly IPsnProtocolCommandPartConfiguration _commandPart;

		public PsnCommandPartInfoBuilderFromLowLevel(IPsnProtocolCommandPartConfiguration commandPart) {
			_commandPart = commandPart;
		}

		public IPsnCommandPartInfo Build() {
			return new PsnCommandPartInfoSimple(
				new UidStringToLower(_commandPart.Id.IdentyString),
				_commandPart.PartName,
				new PsnCommandPartTypeBuilderFromLowLevel(_commandPart.PartType).Build(),
				_commandPart.DefParams.Select(d => (IDefinedPsnParameterInfo)new DefinedPsnParameterInfoBasedOnLowCfg(d)).ToList(),
				_commandPart.VarParams.Select(v => (IVariablePsnParameterInfo)new VariablePsnParameterInfoBasedOnLowCfg(v)).ToList(),
				_commandPart.Length,
				_commandPart.Offset,
				new DefinedPsnParameterInfoBasedOnLowCfg(_commandPart.Address),
				new DefinedPsnParameterInfoBasedOnLowCfg(_commandPart.CommandCode),
				new VariablePsnParameterInfoBasedOnLowCfg(_commandPart.CrcLow),
				new VariablePsnParameterInfoBasedOnLowCfg(_commandPart.CrcHigh),
				new UidStringToLower(_commandPart.Id.IdentyString));
		}
	}
}