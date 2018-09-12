namespace RPD.DAL {
	/// <summary>
	/// Часть виртуального параметра ПСН
	/// </summary>
	public interface IPsnVirtualParameterPart
	{
		/// <summary>
		/// Идентификатор реального параметра, на который опирается часть
		/// </summary>
		IUid RealParameterId { get; }

		/// <summary>
		/// Название части, под которым она известна внутри выражения
		/// </summary>
		string ExpressionName { get; }
	}
}