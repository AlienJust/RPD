namespace RPD.DAL {
	/// <summary>
	/// Адрес сигнала ПСН
	/// </summary>
	public interface IPsnSignalAddress
	{
		/// <summary>
		/// Параметр ПСН
		/// </summary>
		IPsnParameterConfiguration ParameterConfiguration { get; }

		/// <summary>
		/// Посылка, в которой располагается параметр
		/// </summary>
		IPsnCommandPartInfo CommandPart { get; }
	}
}