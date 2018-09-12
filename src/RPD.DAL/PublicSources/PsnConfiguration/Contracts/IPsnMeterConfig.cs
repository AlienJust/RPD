using System.Collections.Generic;

namespace RPD.DAL {
	/// <summary>
	/// Конфигурация измерителя ПСН
	/// </summary>
	public interface IPsnMeterConfig {
		/// <summary>
		/// Название измерителя
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Конфигурации каналов
		/// </summary>
		IEnumerable<IPsnChannelConfig> PsnChannelConfigs { get; }
	}
}