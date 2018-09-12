using System;

namespace RPD.DAL {
	/// <summary>
	/// Циклический кусок команды (повторяющийся через определенный промежуток времени)
	/// </summary>
	public interface ICyclePsnCmdPartInfo : IPsnCommandPartInfo
	{
		/// <summary>
		/// Период появления команды
		/// </summary>
		TimeSpan CyclePeriod { get; }
	}
}