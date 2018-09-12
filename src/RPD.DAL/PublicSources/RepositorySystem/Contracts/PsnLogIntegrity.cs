namespace RPD.DAL {
	/// <summary>
	/// Целостность ПСН лога
	/// </summary>
	public enum PsnLogIntegrity {
		/// <summary>
		/// Всё в порядке
		/// </summary>
		Ok,

		/// <summary>
		/// Ошибка потоковой обработки страниц (либо номера страниц установленны не последовательны, либо имеется скачёк по времени назад)
		/// </summary>
		PagesFlowError,

		/// <summary>
		/// Состояние не известно
		/// </summary>
		Unknown
	}
}