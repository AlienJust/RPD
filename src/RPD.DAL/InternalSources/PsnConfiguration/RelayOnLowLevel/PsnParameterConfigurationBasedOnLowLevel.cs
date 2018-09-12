using DataAbstractionLevel.Low.PsnConfig.Contracts;
using RPD.DAL.Common.SimpleRelease;

namespace RPD.DAL.PsnConfiguration.RelayOnLowLevel {
	class PsnParameterConfigurationBasedOnLowLevel : IPsnParameterConfiguration {
		private readonly IPsnProtocolParameterConfiguration _cfg;

		public PsnParameterConfigurationBasedOnLowLevel(IPsnProtocolParameterConfiguration cfg)
		{
			_cfg = cfg;
		}

		public IUid Id => new UidStringToLower(_cfg.Id.IdentyString);

		public string Name => _cfg.Name;

		public bool IsBitSignal => _cfg.IsBitSignal;

		public double GetValue(byte[] cmdPartContext, int startByte) {
			return _cfg.GetValue(cmdPartContext, startByte);
		}
	}
}