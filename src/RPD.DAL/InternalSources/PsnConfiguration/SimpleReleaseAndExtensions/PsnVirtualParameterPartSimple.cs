namespace RPD.DAL.PsnConfiguration.SimpleReleaseAndExtensions {
	class PsnVirtualParameterPartSimple : IPsnVirtualParameterPart {
		private readonly IUid _realParameterId;
		private readonly string _expressionName;
		public PsnVirtualParameterPartSimple(IUid realParameterId, string expressionName) {
			_realParameterId = realParameterId;
			_expressionName = expressionName;
		}

		public IUid RealParameterId {
			get { return _realParameterId; }
		}

		public string ExpressionName {
			get { return _expressionName; }
		}
	}
}