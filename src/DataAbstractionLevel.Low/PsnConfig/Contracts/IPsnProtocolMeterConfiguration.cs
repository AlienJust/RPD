namespace DataAbstractionLevel.Low.PsnConfig.Contracts {
	/// <summary>
	/// Информация об измерителе ПСН
	/// </summary>
	public interface IPsnProtocolMeterConfiguration {
		/// <summary>
		/// Название измерителя
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Адрес измерителя
		/// </summary>
		int Address { get; }
	}
}