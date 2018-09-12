using System;

namespace DataAbstractionLevel.Low.Storage.Contracts {
	/// <summary>
	/// Запись индекса страниц лога ПСН
	/// </summary>
	public interface IRpdPageIndexRecord
	{
		/// <summary>
		/// Информация о странице
		/// </summary>
		RpdPageInfo PageInfo { get; }

		/// <summary>
		/// Абсолютное смещение в потоке данных
		/// </summary>
		long AbsolutePositionInStream { get; }

		/// <summary>
		/// Абсолютный номер страницы
		/// </summary>
		long AbsolutePageNumber { get; }

		/// <summary>
		/// Дата и время
		/// </summary>
		DateTime? PageTime { get; }

		/// <summary>
		/// Номер страницы
		/// </summary>
		int PageNumber { get; }
	}
}