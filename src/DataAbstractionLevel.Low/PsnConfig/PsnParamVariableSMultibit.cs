using System.Collections.Generic;
using DataAbstractionLevel.Low.PsnConfig.Contracts;

namespace DataAbstractionLevel.Low.PsnConfig {
	internal sealed class PsnParamVariableSMultibit : PsnParamBase, IPsnProtocolParameterConfigurationVariable
	{
		private readonly int _positionByte;
		private readonly int _positionBit;
		private readonly int _length;
		private readonly double _multiplier;

		public PsnParamVariableSMultibit(string id, string name, int positionByte, int positionBit, int length, double multiplier) : 
			base(id, name, false) {
			_positionByte = positionByte;
			_positionBit = positionBit;
			_length = length;
			_multiplier = multiplier;
		}

		public override double GetValue(byte[] cmdPartContext, int startByte)
		{
			return PsnParamValueExtractor.GetMultibitSignedValueFromBytes(cmdPartContext, startByte, _positionByte, _positionBit, _length) * _multiplier;
		}
	}
}