using System.Collections.Generic;

namespace DataAbstractionLevel.Low.Storage.Contracts {
	/// <summary>
	/// Постраничная информация о данных ПСН
	/// </summary>
	public interface IPsnDataPaged {
		/// <summary>
		/// Возвращает индекс страниц
		/// </summary>
		/// <returns>Индекс страниц</returns>
		IEnumerable<IPsnPageIndexRecord> GetPagesIndex();
	}

	/// <summary>
	/// Постраничная информация о данных ПСН
	/// </summary>
	public interface IRpdDataPaged
	{
		/// <summary>
		/// Возвращает индекс страниц
		/// </summary>
		/// <returns>Индекс страниц</returns>
		IEnumerable<IRpdPageIndexRecord> GetPagesIndex();
	}
}