using System.Collections.Generic;

namespace RPD.DAL.LowLevel.Storage.PsnConfig {
	/// <summary>
	/// Хранилище частей конфигураций команд протоколов ПСН
	/// </summary>
	public interface IStorageConfigurationPsnProtocolCommandParts
	{
		/// <summary>
		/// Перечисление конфигураций
		/// </summary>
		IEnumerable<IConfigurationPsnProtocolCommandPart> ConfigurationPsnProtocolCommandParts { get; }
	}
}