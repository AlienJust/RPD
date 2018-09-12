namespace RPD.DAL.PsnConfiguration.Contracts.Internal {

	/// <summary>
	/// Информация о канале линии ПСН
	/// </summary>
	internal interface IPsnChannelInfo {
		/// <summary>
		/// Тип данных канала
		/// </summary>
		PsnChannelTrendDataType Type { get; }

		/// <summary>
		/// Название канала
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Канал разрешен/используется
		/// </summary>
		bool IsEnabled { get; }

		/// <summary>
		/// Канал является входящим
		/// </summary>
		bool IsInput { get; }

		/// <summary>
		/// Канал может быть генератором аварий РПД
		/// </summary>
		bool CanBeFaultSign { get; }

		/// <summary>
		/// Канал предконфигурирован быть генератором аварий РПД
		/// </summary>
		bool IsFaultSign { get; }

		/// <summary>
		/// Конфигурация канала
		/// </summary>
		IPsnChannelConfiguration Configuration { get; }

		/// <summary>
		/// Адрес абонента ПСН, к которому относится канал
		/// </summary>
		int MeterAddress { get; }
	}
}