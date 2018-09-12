using DataAbstractionLevel.Low.PsnConfig.Contracts;

namespace DataAbstractionLevel.Low.PsnConfig {
	internal class PsnParamVariableBit : PsnParamBase, IPsnProtocolParameterConfigurationVariable
	{
		private readonly int _posotionBit;
		private readonly int _positionByte;

		public PsnParamVariableBit(string id, string name, int positionByte, int posotionBit) : 
			base(id, name, true) {
			_positionByte = positionByte;
			_posotionBit = posotionBit;
		}

		public override double GetValue(byte[] cmdPartContext, int startByte)
		{
			return PsnParamValueExtractor.GetBitValueFromBytes(cmdPartContext, startByte, _positionByte, _posotionBit) ? 1.0 : 0.0;
		}

		public override string ToString() {
			return Name + "@" + _positionByte + "." + _posotionBit;
		}
	}
}
