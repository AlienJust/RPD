using System.Collections.Generic;
using DataAbstractionLevel.Low.PsnConfig.Contracts;

namespace DataAbstractionLevel.Low.PsnConfig {
	internal sealed class PsnParamDefinedByte : PsnParamBase, IPsnProtocolParameterDefinedConfiguration
	{
		private readonly int _positionByte;

		public PsnParamDefinedByte(string id, string name, int positionByte, double definedValue) : 
			base(id, name, false) {
			_positionByte = positionByte;
			DefinedValue = definedValue;
		}

		public double DefinedValue { get; }

		public override double GetValue(byte[] cmdPartContext, int startByte)
		{
			return PsnParamValueExtractor.GetByteValueFromBytes(cmdPartContext, startByte, _positionByte);
		}
	}
}