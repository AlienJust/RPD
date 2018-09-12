using DataAbstractionLevel.Low.PsnConfig.Contracts;

namespace DataAbstractionLevel.Low.PsnConfig {
	internal class PsnParamVariableByte : PsnParamBase, IPsnProtocolParameterConfigurationVariable
	{
		private readonly int _positionByte;
		private readonly double _multiplier;

		public PsnParamVariableByte(string id, string name, int positionByte, double multiplier) : 
			base(id, name, false) {
			_positionByte = positionByte;
			_multiplier = multiplier;
		}

		public override double GetValue(byte[] cmdPartContext, int startByte)
		{
			return PsnParamValueExtractor.GetByteValueFromBytes(cmdPartContext, startByte, _positionByte) * _multiplier;
		}
	}
}