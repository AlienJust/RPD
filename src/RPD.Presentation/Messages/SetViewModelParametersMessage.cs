namespace RPD.Presentation.Messages {
	class SetViewModelParametersMessage<TParameter> {
		public SetViewModelParametersMessage(TParameter parameter) {
			Parameter = parameter;
		}

		public TParameter Parameter { get; }
	}
}
