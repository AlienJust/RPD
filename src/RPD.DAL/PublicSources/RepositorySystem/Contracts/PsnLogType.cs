namespace RPD.DAL {
	/// <summary>
	/// тип лога ПСН
	/// </summary>
	public enum PsnLogType {
		/// <summary>
		/// Лог предуставноленного размера
		/// </summary>
		FixedLength,

		/// <summary>
		/// Лог, зависящий от питания РПД
		/// </summary>
		PowerDepended,

		/// <summary>
		/// Связаный с аварией
		/// </summary>
		LinkedToFault
	}
}