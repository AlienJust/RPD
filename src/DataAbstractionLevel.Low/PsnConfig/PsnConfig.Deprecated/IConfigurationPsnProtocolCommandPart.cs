using RPD.DalRelease.PSN.Config;

namespace RPD.DAL.LowLevel.Storage.PsnConfig {
	/// <summary>
	/// Конфигурация части команды ПСН
	/// </summary>
	public interface IConfigurationPsnProtocolCommandPart : IObjectWithIdentifier
	{
		/// <summary>
		/// Название части команды
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Тип части команды (запрос/ответ)
		/// </summary>
		PsnProtocolCommandPartType Type { get; }

		/// <summary>
		/// Идентификатор конфигурации команды протокола ПСН, к которой относится данная часть команды
		/// </summary>
		IIdentifier IdConfigurationProtocolPsnCommand { get; }
	}
}