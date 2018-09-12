namespace DataAbstractionLevel.Low.PsnConfig.Contracts {
	/// <summary>
	/// Адрес сигнала ПСН
	/// </summary>
	public interface IPsnProtocolDeviceSignalAddress
	{
		/// <summary>
		/// Параметр ПСН
		/// </summary>
		IPsnProtocolParameterConfiguration ParameterConfiguration { get; }

		/// <summary>
		/// Часть команды протокола ПСН, в которой располагается параметр
		/// </summary>
		IPsnProtocolCommandPartConfiguration CommandPart { get; }
	}
}