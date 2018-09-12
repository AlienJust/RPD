using AlienJust.Support.Identy.Contracts;

namespace DataAbstractionLevel.Low.PsnConfig.Contracts {
	/// <summary>
	/// Информация о команде ПСН
	/// </summary>
	public interface IPsnProtocolCommandConfiguration
	{
		/// <summary>
		/// Название команды ПСН
		/// </summary>
		string Name { get; }

        /// <summary>
        /// Идентификатор команды
        /// </summary>
		IIdentifier Id { get; }
	}
}