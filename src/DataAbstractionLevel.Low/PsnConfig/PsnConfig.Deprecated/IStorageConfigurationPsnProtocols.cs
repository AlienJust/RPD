using System.Collections.Generic;

namespace RPD.DAL.LowLevel.Storage.PsnConfig {
	/// <summary>
	/// Хранилище конфигурация протоколов ПСН
	/// </summary>
	public interface IStorageConfigurationPsnProtocols {
		/// <summary>
		/// Перечисление конфигураций
		/// </summary>
		IEnumerable<IConfigurationPsnProtocol> ConfigurationPsnProtocols { get; }
	}
}