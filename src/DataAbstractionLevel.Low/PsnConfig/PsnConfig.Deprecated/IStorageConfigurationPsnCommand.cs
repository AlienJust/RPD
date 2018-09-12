using System.Collections.Generic;

namespace RPD.DAL.LowLevel.Storage.PsnConfig {
	/// <summary>
	/// Хранилище конфигураций команд протоколов ПСН
	/// </summary>
	public interface IStorageConfigurationPsnProtocolCommands
	{
		/// <summary>
		/// Перечисление конфигураций
		/// </summary>
		IEnumerable<IConfigurationPsnProtocolCommand> ConfigurationPsnProtocolCommands { get; }
	}
}