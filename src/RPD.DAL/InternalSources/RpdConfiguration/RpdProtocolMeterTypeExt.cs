using System;
using DataAbstractionLevel.Low.Storage.Contracts;
using RPD.DAL.Common.Contracts;

namespace RPD.DAL.RpdConfiguration {
	class RpdMeterTypeBuilderFromLowLevel : IBuilder<RpdMeterType> {
		private readonly RpdProtocolMeterType _val;
		public RpdMeterTypeBuilderFromLowLevel(RpdProtocolMeterType val) {
			_val = val;
		}

		public RpdMeterType Build() {
			switch (_val)
			{
				case RpdProtocolMeterType.Irvi:
					return RpdMeterType.Irvi;
				case RpdProtocolMeterType.Radar:
					return RpdMeterType.Radar;
				case RpdProtocolMeterType.Undefined:
					return RpdMeterType.Undefined;
				case RpdProtocolMeterType.Uran:
					return RpdMeterType.Uran;
			}
			throw new Exception("Cannot convert " + typeof(RpdProtocolMeterType).FullName + " to " + typeof(RpdMeterType).FullName + " (Low to High)");
		}
	}
}