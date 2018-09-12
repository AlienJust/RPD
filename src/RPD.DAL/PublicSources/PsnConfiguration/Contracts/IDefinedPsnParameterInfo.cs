namespace RPD.DAL {
	/// <summary>
	/// Описывает параметр ПСН с заранее известным значением (адрес прибора, код команды и т.п.)
	/// </summary>
	public interface IDefinedPsnParameterInfo : IPsnParameterConfiguration
	{
		/// <summary>
		/// Определенное заранее значение параметра
		/// </summary>
		double DefinedValue { get; }
	}
}