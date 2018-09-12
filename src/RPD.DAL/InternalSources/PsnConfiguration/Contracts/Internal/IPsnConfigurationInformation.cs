namespace RPD.DAL.PsnConfiguration.Contracts.Internal {
	/// <summary>
	/// Конфигурация протокола магистрали ПСН
	/// </summary>
	interface IPsnConfigurationInformation : IObjectWithId
	{
		/// <summary>
		/// Название конфигурации
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Более детальное описание конфигурации, нежели просто название
		/// </summary>
		string Description { get; }

		/// <summary>
		/// Версия конфигурации
		/// </summary>
		string Version { get; }

		/// <summary>
		/// Идентификатор конфигурации в блоке РПД
		/// </summary>
		string RpdId { get; }
	}
}