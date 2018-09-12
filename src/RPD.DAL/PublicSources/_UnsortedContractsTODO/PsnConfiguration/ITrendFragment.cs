using System;
using System.Collections.ObjectModel;

namespace RPD.DAL
{
	/// <summary>
	/// Апроксиматор данных
	/// </summary>
	public interface ITrendFragment
	{
		/// <summary>
		/// 
		/// </summary>
		ReadOnlyCollection<IDataPoint> Points { get; }

		/// <summary>
		/// 
		/// </summary>
		DateTime From { get; }

		/// <summary>
		/// 
		/// </summary>
		DateTime Upto { get; }
	}
}
