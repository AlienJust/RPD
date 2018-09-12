namespace RPD.DAL {
	/// <summary>
	/// Идентификатор
	/// </summary>
	public interface IUid {
		/// <summary>
		/// Строковое представление идентификатора
		/// </summary>
		string UnicString { get; }
	}
}