namespace RPD.DAL {
	/// <summary>
	/// Сигнал ПСН
	/// </summary>
	public interface IPsnSignalConfiguration
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
		IPsnSignalAddress Address { get; }
	}
}