using AlienJust.Support.Identy.Contracts;

namespace DataAbstractionLevel.Low.PsnConfig.Contracts {
	/// <summary>
	/// Описание конфигурации ПСН
	/// </summary>
	public interface IPsnProtocolConfigurationInformtaion : IObjectWithIdentifier
	{
		/// <summary>
		/// Название конфигурации
		/// </summary>
		string Name { get; }
		
		/// <summary>
		/// Более детальное описание конфигурации, нежели просто название
		/// </summary>
		string Description { get; }

		/// <summary>
		/// Версия конфигурации
		/// </summary>
		string Version { get; }

		/// <summary>
		/// Идентификатор конфигурации в блоке РПД
		/// </summary>
		string RpdId { get; }
	}
}