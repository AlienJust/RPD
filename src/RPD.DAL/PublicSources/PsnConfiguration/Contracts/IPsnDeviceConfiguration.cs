namespace RPD.DAL {
	/// <summary>
	/// Конфигурация устройства линии ПСН (абонента линии ПСН)
	/// </summary>
	public interface IPsnDeviceConfiguration
	{
		/// <summary>
		/// Название устройства
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Адрес абонента магистрали ПСН
		/// </summary>
		int Address { get; }
	}
}