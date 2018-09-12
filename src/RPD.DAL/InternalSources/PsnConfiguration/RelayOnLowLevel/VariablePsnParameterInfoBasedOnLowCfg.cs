using System.Collections.Generic;
using DataAbstractionLevel.Low.PsnConfig.Contracts;
using RPD.DAL.Common.SimpleRelease;

namespace RPD.DAL.PsnConfiguration.RelayOnLowLevel {
	class VariablePsnParameterInfoBasedOnLowCfg : IVariablePsnParameterInfo {
		private readonly IPsnProtocolParameterConfigurationVariable _lowCfg;

		public VariablePsnParameterInfoBasedOnLowCfg(IPsnProtocolParameterConfigurationVariable lowCfg)
		{
			_lowCfg = lowCfg;
		}

		public IUid Id => new UidStringToLower(_lowCfg.Id.IdentyString);

		public string Name => _lowCfg.Name;

		public bool IsBitSignal => _lowCfg.IsBitSignal;

		public double GetValue(byte[] cmdPartContext, int startByte) {
			return _lowCfg.GetValue(cmdPartContext, startByte);
		}
	}
}