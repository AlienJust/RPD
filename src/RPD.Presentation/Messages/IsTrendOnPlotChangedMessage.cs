using RPD.Presentation.Contracts.ViewModels;

namespace RPD.Presentation.Messages {
	/// <summary>
	/// Сообщение. Посылается когда нужно поместить или удалить тренд с графика.
	/// </summary>
	class IsTrendOnPlotChangedMessage {
		public IsTrendOnPlotChangedMessage(ITrendViewModel trend) {
			Trend = trend;
		}

		/// <summary>
		/// Тренд.
		/// </summary>
		public ITrendViewModel Trend { get; }
	}
}
