using System.Collections.Generic;
using DataAbstractionLevel.Low.PsnConfig.Contracts;

namespace DataAbstractionLevel.Low.PsnConfig {
	internal class PsnParamVariableSByte : PsnParamBase, IPsnProtocolParameterConfigurationVariable
	{
		private readonly int _positionByte;
		private readonly double _multiplier;

		public PsnParamVariableSByte(string id, string name, int positionByte, double multiplier) : 
			base(id, name, false) {
			_positionByte = positionByte;
			_multiplier = multiplier;
		}

		public override double GetValue(byte[] cmdPartContext, int startByte)
		{
			return PsnParamValueExtractor.GetSByteValueFromBytes(cmdPartContext, startByte, _positionByte) * _multiplier;
		}
	}
}