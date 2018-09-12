namespace DataAbstractionLevel.Low.PsnConfig.Contracts {
	/// <summary>
	/// Сигнал ПСН
	/// </summary>
	public interface IPsnProtocolDeviceSignalConfiguration
	{
		/// <summary>
		/// Название сигнала
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Флаг, указывающий, что сигнал битовый
		/// </summary>
		bool IsBooleanValued { get; }


		/// <summary>
		/// Адрес сигнала, необходимый для поиска данных по сигналу внутри лога
		/// </summary>
		IPsnProtocolDeviceSignalAddress Address { get; }
	}
}