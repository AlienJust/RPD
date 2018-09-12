namespace DataAbstractionLevel.Low.PsnConfig.Contracts {
	/// <summary>
	/// Тип части команды ПСН
	/// </summary>
	public enum PsnProtocolCommandPartType {
		/// <summary>
		/// Запрос ведущего устройства
		/// </summary>
		Request,

		/// <summary>
		/// Ответ ведомого устройства
		/// </summary>
		Reply,

		/// <summary>
		/// Ответ ведомого устройства, плюс указане необходимости проверки наличия запроса перед ответом
		/// </summary>
		ReplyWithRequiredRequest
	}
}