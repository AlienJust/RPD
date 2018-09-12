using System.Collections.Generic;
using AlienJust.Support.Identy.Contracts;

namespace DataAbstractionLevel.Low.Storage.Contracts {
	/// <summary>
	/// Хранилище "своих" настроек для логов ПСН
	/// </summary>
	public interface IRpdDataCustomConfigurationsStorage
	{
		/// <summary>
		/// Перечисление всех настроек хранилища
		/// </summary>
		IEnumerable<IRpdDataCustomConfigration> Configurations { get; }

		/// <summary>
		/// Добавляет конфигурацию лога в хранилище
		/// </summary>
		/// <param name="rpdDataCustomConfigurationId">Идентификатор конфигурации</param>
		/// <param name="rpdConfigruationId">Идентификатор конфигурации ПСН линии</param>
		/// <param name="customLogName">Свое название лога ПСН</param>
		void Add(IIdentifier rpdDataCustomConfigurationId, IIdentifier rpdConfigruationId, string customLogName);

		/// <summary>
		/// Обновляет конфигурацию внутри хранилища
		/// </summary>
		/// <param name="rpdDataCustomConfigurationId">Идентификатор конфигурации для обновления</param>
		/// <param name="setRpdConfigruationId">Новый идентификатор конфигурации магистрали ПСН</param>
		/// <param name="setCustomLogName">Новое "свое" название лога ПСН</param>
		void Update(IIdentifier rpdDataCustomConfigurationId, IIdentifier setRpdConfigruationId, string setCustomLogName);

		/// <summary>
		/// Удаляет конфигурацию лога ПСН из хранилища
		/// </summary>
		/// <param name="rpdDataCustomConfigurationId"></param>
		void Remove(IIdentifier rpdDataCustomConfigurationId);
	}
}