using System;
using DataAbstractionLevel.Low.Storage.Contracts;
using RPD.DAL.Common.Contracts;

namespace RPD.DAL.RpdConfiguration {
	class RpdProtocolMeterTypeBuilderFromHighLevel : IBuilder<RpdProtocolMeterType> {
		private readonly RpdMeterType _val;
		public RpdProtocolMeterTypeBuilderFromHighLevel(RpdMeterType val)
		{
			_val = val;
		}

		public RpdProtocolMeterType Build() {
			switch (_val)
			{
				case RpdMeterType.Irvi:
					return RpdProtocolMeterType.Irvi;
				case RpdMeterType.Radar:
					return RpdProtocolMeterType.Radar;
				case RpdMeterType.Undefined:
					return RpdProtocolMeterType.Undefined;
				case RpdMeterType.Uran:
					return RpdProtocolMeterType.Uran;
			}
			throw new Exception("Cannot convert " + typeof(RpdMeterType) + " to " + typeof(RpdProtocolMeterType) + " (Low to High)");
		}
	}
}