namespace RPD.DAL.LowLevel.Storage.PsnConfig
{
	/// <summary>
	/// Конфигурация протокола ПСН
	/// </summary>
	public interface IConfigurationPsnProtocol : IObjectWithIdentifier
	{
		/// <summary>
		/// Название протокола
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Версия
		/// </summary>
		string Version { get; }

		/// <summary>
		/// Подробное описание
		/// </summary>
		string Description { get; }

		/// <summary>
		/// Идентификатор для РПД
		/// </summary>
		int RpdId { get; }
	}
}
