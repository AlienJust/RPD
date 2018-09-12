using System;
using System.Collections.Generic;

namespace RPD.DAL {
	/// <summary>
	/// Конфигурация проткола ПСН
	/// </summary>
	public interface IPsnConfiguration {
		/// <summary>
		/// Идентификатор конфигурации протокола ПСН
		/// </summary>
		Guid Id { get; }

		/// <summary>
		/// Идентификатор для настройки блока РПД
		/// </summary>
		int RpdId { get; }

		/// <summary>
		/// Название протокола ПСН
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Версия протокола ПСН
		/// </summary>
		string Version { get; }

		/// <summary>
		/// Описание протокола ПСН
		/// </summary>
		string Description { get; }

		/// <summary>
		/// Конфигурации измерителей ПСН
		/// </summary>
		IEnumerable<IPsnMeterConfig> PsnMeterConfigs { get; }
	}
}