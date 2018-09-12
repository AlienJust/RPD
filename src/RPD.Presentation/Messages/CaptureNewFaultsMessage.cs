namespace RPD.Presentation.Messages {
	/// <summary>
	/// Глобальное сообщение - начать отслеживание появления новых аварий в репозитории.
	/// </summary>
	class CaptureNewFaultsMessage {
		public CaptureNewFaultsMessage(bool start) {
			Start = start;
		}

		public bool Start { get; }
	}
}
