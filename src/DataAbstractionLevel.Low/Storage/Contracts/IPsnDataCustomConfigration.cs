using AlienJust.Support.Identy.Contracts;

namespace DataAbstractionLevel.Low.Storage.Contracts {
	/// <summary>
	/// Своя (пользовательская) информация о логе ПСН
	/// </summary>
	public interface IPsnDataCustomConfigration : IObjectWithIdentifier
	{
		/// <summary>
		/// Идентификатор конфигурации ПСН
		/// </summary>
		IIdentifier PsnConfigurationId { get; }

		/// <summary>
		/// Название лога ПСН
		/// </summary>
		string CustomLogName { get; }
	}
}