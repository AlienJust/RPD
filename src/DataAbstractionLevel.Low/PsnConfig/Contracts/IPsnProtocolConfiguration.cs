using System.Collections.Generic;
using AlienJust.Support.Identy.Contracts;

namespace DataAbstractionLevel.Low.PsnConfig.Contracts {
	/// <summary>
	/// Конфигурация ПСН протокола
	/// </summary>
	public interface IPsnProtocolConfiguration : IObjectWithIdentifier {
		/// <summary>
		/// Информация о ПСН протоколе
		/// </summary>
		IPsnProtocolConfigurationInformtaion Information { get; }

		/// <summary>
		/// Перечисление абонентов линии ПСН
		/// </summary>
		IEnumerable<IPsnProtocolDeviceConfiguration> PsnDevices { get; }

		/// <summary>
        /// Перечисление повторяющихся циклически частей команд ПСН
		/// </summary>
		IEnumerable<IPsnProtocolCommandPartConfigurationCycle> CycleCommandParts { get; }

        /// <summary>
        /// Перечисление частей команд ПСН
        /// </summary>
		IEnumerable<IPsnProtocolCommandPartConfiguration> CommandParts { get; }

        /// <summary>
        /// Перечисление команд ПСН
        /// </summary>
        IEnumerable<IPsnProtocolCommandConfiguration> Commands { get; }

		IEnumerable<IPsnMergedDevice> MergedDevices { get; }
	}
}