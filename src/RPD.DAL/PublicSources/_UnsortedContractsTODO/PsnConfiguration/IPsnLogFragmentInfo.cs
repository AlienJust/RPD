using System;

namespace RPD.DAL {
	/// <summary>
	/// Информация о фрагменте лога ПСН
	/// </summary>
	public interface IPsnLogFragmentInfo
	{
		/// <summary>
		/// Смещение начала
		/// </summary>
		int StartOffset { get; }

		/// <summary>
		/// Начало фрагмента лога
		/// </summary>
		DateTime? StartTime { get; }
	}
}