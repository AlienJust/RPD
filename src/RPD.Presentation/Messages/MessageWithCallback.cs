using System;

namespace RPD.Presentation.Messages {
	/// <summary>
	/// Сообщение с обратным вызовом.
	/// </summary>
	class MessageWithCallback {
		private readonly Action _callback;

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="callback">Колбэк, который будет вызван принимающей сторой (вызывается вручную)</param>
		public MessageWithCallback(Action callback) {
			_callback = callback;
		}

		/// <summary>
		/// Вызвать колбэк. Вызывается принимающей стороной.
		/// </summary>
		public void ProcessCallback()
		{
			_callback?.Invoke();
		}
	}
}
