using System;
using DataAbstractionLevel.Low.Storage.Shared;
using RPD.DAL.Common.Contracts;

namespace RPD.DAL.RepostirorySystem.Relay {
	class PsnDataFragmentTypeBuilderFromHighLevel : IBuilder<PsnDataFragmentType> {
		private readonly PsnLogType _val;
		public PsnDataFragmentTypeBuilderFromHighLevel(PsnLogType val) {
			_val = val;
		}

		public PsnDataFragmentType Build() {
			switch (_val)
			{
				case PsnLogType.FixedLength:
					return PsnDataFragmentType.FixedLength;
				case PsnLogType.LinkedToFault:
					return PsnDataFragmentType.LinkedToFault;
				case PsnLogType.PowerDepended:
					return PsnDataFragmentType.PowerDepended;
			}
			throw new Exception("Cannot convert " + typeof(PsnLogType).FullName + " to " + typeof(PsnDataFragmentType).FullName);
		}
	}
}