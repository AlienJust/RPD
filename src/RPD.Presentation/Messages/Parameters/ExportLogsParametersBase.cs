namespace RPD.Presentation.Messages.Parameters {
	/// <summary>
	/// Параметр сообщения для диалога прогресса копирования. Содержит список данных, которые нужно экспортировать.
	/// </summary>
	class ExportLogsParametersBase : LogsParameters {

		public string FileName { get; set; }
	}
}
