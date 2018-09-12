using DataAbstractionLevel.Low.PsnConfig.Contracts;
using RPD.DAL.Common.SimpleRelease;

namespace RPD.DAL.PsnConfiguration.RelayOnLowLevel {
	class DefinedPsnParameterInfoBasedOnLowCfg : IDefinedPsnParameterInfo {
		private readonly IPsnProtocolParameterDefinedConfiguration _lowCfg;

		public DefinedPsnParameterInfoBasedOnLowCfg(IPsnProtocolParameterDefinedConfiguration lowCfg) {
			_lowCfg = lowCfg;
		}

		public IUid Id => new UidStringToLower(_lowCfg.Id.IdentyString);

		public string Name => _lowCfg.Name;

		public bool IsBitSignal => _lowCfg.IsBitSignal;

		public double GetValue(byte[] cmdPartContext, int startByte) {
			return _lowCfg.GetValue(cmdPartContext, startByte);
		}

		public double DefinedValue => _lowCfg.DefinedValue;
	}
}