using System.Collections.Generic;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.StreamableData.Contracts;

namespace DataAbstractionLevel.Low.Storage.StreamableStorages.Contracts {
	/// <summary>
	/// Хранилище логов
	/// </summary>
	public interface IStreamableDataStorage {
		/// <summary>
		/// Перечисление всех ключей устройства
		/// </summary>
		IEnumerable<IStreamableDataWithId> Datas { get; }

		/// <summary>
		/// Добавляет ПСН лог в хранилище
		/// </summary>
		/// <param name="id">Идентификатор данных</param>
		/// <param name="data">Данные</param>
		/// <returns>Вновь добавленные данные</returns>
		void Add(IIdentifier id, IStreamedData data);

		/// <summary>
		/// Убирает ПСН лог из хранилища
		/// </summary>
		/// <param name="id">Идентификатор лога</param>
		void Remove(IIdentifier id);
	}
}