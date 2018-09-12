namespace DataAbstractionLevel.Low.Storage.Contracts {
	/// <summary>
	/// Информация о странице лога данных РПД
	/// </summary>
	public enum RpdPageInfo
	{
		/// <summary>
		/// Нормальная страница
		/// </summary>
		NormalPage,

		/// <summary>
		/// Плохая страница
		/// </summary>
		BadPage,

		/// <summary>
		/// Заголовочная страница РПД
		/// </summary>
		HeadPage
	}
}