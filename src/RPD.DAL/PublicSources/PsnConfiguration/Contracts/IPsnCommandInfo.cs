namespace RPD.DAL {
	/// <summary>
	/// Информация о команде ПСН
	/// </summary>
	public interface IPsnCommandInfo
	{
		/// <summary>
		/// Название команды ПСН
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Идентификатор команды
		/// </summary>
		IUid Id { get; }
	}
}