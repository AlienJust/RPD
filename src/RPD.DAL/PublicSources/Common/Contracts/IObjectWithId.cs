namespace RPD.DAL {
	/// <summary>
	/// Объект с возможностью хранения
	/// </summary>
	public interface IObjectWithId
	{
		/// <summary>
		/// Идентификатор объекта
		/// </summary>
		IUid Id { get; }
	}
}