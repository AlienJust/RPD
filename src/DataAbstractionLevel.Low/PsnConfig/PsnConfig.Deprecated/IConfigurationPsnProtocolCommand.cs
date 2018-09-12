namespace RPD.DAL.LowLevel.Storage.PsnConfig {
	/// <summary>
	/// Конфигурация команды ПСН
	/// </summary>
	public interface IConfigurationPsnProtocolCommand : IObjectWithIdentifier
	{
		/// <summary>
		/// Название команды
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Идентификатор конфигурации протокола ПСН, к которому относится команда
		/// </summary>
		IIdentifier IdConfigurationPsnProtocol { get; }
	}
}