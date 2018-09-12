using AlienJust.Support.Identy.Contracts;

namespace DataAbstractionLevel.Low.Storage.Contracts {
	/// <summary>
	/// Своя (пользовательская) информация о логе РПД
	/// </summary>
	public interface IRpdDataCustomConfigration : IObjectWithIdentifier
	{
		/// <summary>
		/// Идентификатор конфигурации РПД
		/// </summary>
		IIdentifier RpdConfigurationId { get; }

		/// <summary>
		/// Название лога ПСН
		/// </summary>
		string CustomLogName { get; }
	}
}