using System;
using DataAbstractionLevel.Low.Storage.Shared;
using RPD.DAL.Common.Contracts;

namespace RPD.DAL.RepostirorySystem.Relay {
	class PsnLogTypeBuilderFromLowLevel : IBuilder<PsnLogType> {
		private readonly PsnDataFragmentType _val;
		public PsnLogTypeBuilderFromLowLevel(PsnDataFragmentType val) {
			_val = val;
		}

		public PsnLogType Build() {
			switch (_val)
			{
				case PsnDataFragmentType.FixedLength:
					return PsnLogType.FixedLength;
				case PsnDataFragmentType.LinkedToFault:
					return PsnLogType.LinkedToFault;
				case PsnDataFragmentType.PowerDepended:
					return PsnLogType.PowerDepended;
			}
			throw new Exception("Cannot convert " + typeof(PsnDataFragmentType).FullName + " to " + typeof(PsnLogType).FullName);
		}
	}
}