namespace DataAbstractionLevel.Low.Storage.Shared {
	/// <summary>
	/// тип лога ПСН
	/// </summary>
	public enum PsnDataFragmentType {
		/// <summary>
		/// Лог предустановленного размера
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