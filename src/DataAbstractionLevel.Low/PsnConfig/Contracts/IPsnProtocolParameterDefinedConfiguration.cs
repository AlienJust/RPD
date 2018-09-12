namespace DataAbstractionLevel.Low.PsnConfig.Contracts {
	/// <summary>
	/// Описывает параметр ПСН с заранее известным значением (адрес прибора, код команды и т.п.)
	/// </summary>
	public interface IPsnProtocolParameterDefinedConfiguration : IPsnProtocolParameterConfiguration
	{
		/// <summary>
		/// Определенное заранее значение параметра
		/// </summary>
		double DefinedValue { get; }
	}
}