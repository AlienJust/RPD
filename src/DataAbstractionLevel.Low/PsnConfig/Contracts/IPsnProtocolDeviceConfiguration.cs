namespace DataAbstractionLevel.Low.PsnConfig.Contracts {
	/// <summary>
	/// Конфигурация устройства линии ПСН (абонента линии ПСН)
	/// </summary>
	public interface IPsnProtocolDeviceConfiguration
	{
		/// <summary>
		/// Название устройства
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Адрес абонента
		/// </summary>
		int Address { get; }
		
		///// <summary>
		///// Перечисление сигналов устройства
		///// </summary>
		//IEnumerable<IPsnProtocolDeviceSignalConfiguration> Signals { get; }
	}
}