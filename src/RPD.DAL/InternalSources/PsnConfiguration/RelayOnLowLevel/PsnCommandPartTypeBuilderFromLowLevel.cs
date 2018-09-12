using System;
using DataAbstractionLevel.Low.PsnConfig.Contracts;
using RPD.DAL.Common.Contracts;
using RPD.DAL.PsnConfiguration.Contracts.Internal;

namespace RPD.DAL.PsnConfiguration.RelayOnLowLevel {
	class PsnCommandPartTypeBuilderFromLowLevel : IBuilder<PsnCommandPartType> {
		private readonly PsnProtocolCommandPartType _val;
		public PsnCommandPartTypeBuilderFromLowLevel(PsnProtocolCommandPartType val) {
			_val = val;
		}

		public PsnCommandPartType Build() {
			switch (_val)
			{
				case PsnProtocolCommandPartType.Reply:
					return PsnCommandPartType.Reply;
				case PsnProtocolCommandPartType.Request:
					return PsnCommandPartType.Request;
			}
			throw new Exception("Cannot convert " + typeof (PsnProtocolCommandPartType).FullName + " to " + typeof (PsnCommandPartType).FullName + " (from low to high)");
		}
	}
}