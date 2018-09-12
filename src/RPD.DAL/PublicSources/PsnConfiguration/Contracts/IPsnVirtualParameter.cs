using System.Collections.Generic;

namespace RPD.DAL {
	/// <summary>
	/// Виртуальный параметр ПСН
	/// </summary>
	public interface IPsnVirtualParameter : IObjectWithId
	{
		/// <summary>
		/// Название параметра
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Выражение параметра
		/// </summary>
		string Expression { get; }

		/// <summary>
		/// Части параметра (ссылки на реальные параметры)
		/// </summary>
		IEnumerable<IPsnVirtualParameterPart> Parts { get; }
	}
}