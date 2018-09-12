using System.Collections.Generic;
using AlienJust.Support.Identy.Contracts;

namespace DataAbstractionLevel.Low.Storage.Contracts {
	/// <summary>
	/// Хранилище "своих" настроек для логов ПСН
	/// </summary>
	public interface IPsnDataCustomConfigurationsStorage {
		/// <summary>
		/// Перечисление всех настроек хранилища
		/// </summary>
		IEnumerable<IPsnDataCustomConfigration> Configurations { get; }

		/// <summary>
		/// Добавляет конфигурацию лога в хранилище
		/// </summary>
		/// <param name="psnDataCustomConfigurationId">Идентификатор конфигурации</param>
		/// <param name="psnConfigruationId">Идентификатор конфигурации ПСН линии</param>
		/// <param name="customLogName">Свое название лога ПСН</param>
		void Add(IIdentifier psnDataCustomConfigurationId, IIdentifier psnConfigruationId, string customLogName);

		/// <summary>
		/// Обновляет конфигурацию внутри хранилища
		/// </summary>
		/// <param name="psnDataCustomConfigurationId">Идентификатор конфигурации для обновления</param>
		/// <param name="setPsnConfigruationId">Новый идентификатор конфигурации магистрали ПСН</param>
		/// <param name="setCustomLogName">Новое "свое" название лога ПСН</param>
		void Update(IIdentifier psnDataCustomConfigurationId, IIdentifier setPsnConfigruationId, string setCustomLogName);

		/// <summary>
		/// Удаляет конфигурацию лога ПСН из хранилища
		/// </summary>
		/// <param name="psnDataCustomConfigurationId"></param>
		void Remove(IIdentifier psnDataCustomConfigurationId);
	}
}