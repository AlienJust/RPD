using System;
using System.Collections.Generic;
using System.Linq;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.Contracts;
using DataAbstractionLevel.Low.Storage.Shared;

namespace DataAbstractionLevel.Low.Storage.PsnDataCustomConfigStorage
{
	/// <summary>
	/// Кэширующее в ОЗУ хранилище пользовательских данных о логе ПСН
	/// </summary>
	public class PsnDataCustomConfigurationsStorageRelayMemoryCache : IPsnDataCustomConfigurationsStorage {
		private readonly IPsnDataCustomConfigurationsStorage _subStorage;
		private readonly List<IPsnDataCustomConfigrationWritable> _customConfigs; // writable memory cache

		/// <summary>
		/// Создает новое хранилище В ОЗУ
		/// </summary>
		/// <param name="relayOnStorage">Хранилище источник</param>
		public PsnDataCustomConfigurationsStorageRelayMemoryCache(IPsnDataCustomConfigurationsStorage relayOnStorage) {
			if (relayOnStorage == null) throw new NullReferenceException("relayOnStorage");
			
			_subStorage = relayOnStorage;
			_customConfigs = relayOnStorage.Configurations.Select(pcc => (IPsnDataCustomConfigrationWritable)new PsnDataCustomConfigurationSimple(pcc.Id, pcc.PsnConfigurationId, pcc.CustomLogName)).ToList();
		}

		/// <summary>
		/// Перечисление данных хранилища
		/// </summary>
		public IEnumerable<IPsnDataCustomConfigration> Configurations {
			get { return _customConfigs; }
		}

		
		/// <summary>
		/// Добавляет данные в хранилище
		/// </summary>
		/// <param name="psnDataCustomConfigurationId">Идентификатор записи данных</param>
		/// <param name="psnConfigruationId">Идентификатор пользовательской конфигурации ПСН</param>
		/// <param name="customLogName">Пользовательское название лога</param>
		public void Add(IIdentifier psnDataCustomConfigurationId, IIdentifier psnConfigruationId, string customLogName)
		{
			_subStorage.Add(psnDataCustomConfigurationId, psnConfigruationId, customLogName);
			_customConfigs.Add(new PsnDataCustomConfigurationSimple(psnDataCustomConfigurationId, psnConfigruationId, customLogName));
		}

		/// <summary>
		/// Обновляет данные пользователя о ПСН логе
		/// </summary>
		/// <param name="psnDataCustomConfigurationId">Идентификатор записи, которую необходимо обновить</param>
		/// <param name="setPsnConfigruationId">Новое значение идентификатора пользовательской конфигурации ПСН</param>
		/// <param name="setCustomLogName">Новое пользовательское название лога</param>
		public void Update(IIdentifier psnDataCustomConfigurationId, IIdentifier setPsnConfigruationId, string setCustomLogName)
		{
			var customConfig = _customConfigs.First(cc => cc.Id.IdentyString == psnDataCustomConfigurationId.IdentyString);
			customConfig.SetPsnConfigurationId(setPsnConfigruationId);
			customConfig.SetCustomLogName(setCustomLogName);

			_subStorage.Update(psnDataCustomConfigurationId, setPsnConfigruationId, setCustomLogName);
		}

		/// <summary>
		/// Удаляет данные из хранилища
		/// </summary>
		/// <param name="psnDataCustomConfigurationId">Идентификатор записи данных, которые необходимо удалить</param>
		public void Remove(IIdentifier psnDataCustomConfigurationId)
		{
			var customConfigs = _customConfigs.Where(cc => cc.Id == psnDataCustomConfigurationId).ToList();
			foreach (var pdccw in customConfigs) {
				_customConfigs.Remove(pdccw);
			}

			_subStorage.Remove(psnDataCustomConfigurationId);
		}
	}
}
