namespace RPD.DAL.PsnConfiguration.Contracts.Internal {
	/// <summary>
	/// Тип части команды ПСН
	/// </summary>
	public enum PsnCommandPartType
	{
		/// <summary>
		/// Запрос ведущего устройства
		/// </summary>
		Request,

		/// <summary>
		/// Ответ ведомого устройства
		/// </summary>
		Reply
	}
}