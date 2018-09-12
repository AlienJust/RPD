using System;

namespace DataAbstractionLevel.Low.PsnConfig.Contracts {
	/// <summary>
	/// Циклический кусок команды (повторяющийся через определенный промежуток времени)
	/// </summary>
	public interface IPsnProtocolCommandPartConfigurationCycle : IPsnProtocolCommandPartConfiguration
	{
		/// <summary>
		/// Период появления команды
		/// </summary>
		TimeSpan CyclePeriod { get; }
	}
}