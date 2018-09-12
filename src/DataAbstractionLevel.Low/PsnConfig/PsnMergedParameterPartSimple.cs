using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.PsnConfig.Contracts;

namespace DataAbstractionLevel.Low.PsnConfig {
	class PsnMergedParameterPartSimple : IPsnMergedParameterPart {
		private readonly IIdentifier _realParameterId;
		private readonly string _expressionName;
		public PsnMergedParameterPartSimple(IIdentifier realParameterId, string expressionName) {
			_realParameterId = realParameterId;
			_expressionName = expressionName;
		}

		public IIdentifier RealParameterId {
			get { return _realParameterId; }
		}

		public string ExpressionName {
			get { return _expressionName; }
		}
	}
}