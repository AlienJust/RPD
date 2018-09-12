namespace RPD.Presentation.Messages {
	public enum NavigateDirection {
		Forward,
		Backward
	};

	/// <summary>
	/// Сообщение о смене представления (View в MVVM).
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class NavigateMessage<T> {
		public NavigateMessage(NavigateDirection direction, T from, T to) {
			NavigateDirection = direction;
			From = from;
			To = to;
		}

		public NavigateMessage(NavigateDirection direction, T from) {
			NavigateDirection = direction;
			From = from;
		}

		public NavigateDirection NavigateDirection { get; }

		/// <summary>
		/// Исходное представление, которое послало сообщение.
		/// </summary>
		public T From { get; }

		/// <summary>
		/// Представление, на которое нужно перейти.
		/// </summary>
		public T To { get; }
	}
}