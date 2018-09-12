using System;
using System.Collections.Generic;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.StreamableData.Contracts;

namespace DataAbstractionLevel.Low.Storage.Contracts {
	/// <summary>
	/// Хранилище логов
	/// </summary>
	public interface IPsnDataStorage {
		/// <summary>
		/// Перечисление всех ПСН логов устройства
		/// </summary>
		IEnumerable<IPsnData> PsnDatas { get; }

		/// <summary>
		/// Добавляет ПСН лог в хранилище
		/// </summary>
		/// <param name="id">Идентификатор данных</param>
		/// <param name="data">Данные</param>
		/// <param name="progressChangedAction">Действие при изменении прогресса</param>
		/// <returns>Вновь добавленные данные</returns>
		IStreamedData Add(IIdentifier id, IStreamedData data, Action<double> progressChangedAction);

		/// <summary>
		/// Убирает ПСН лог из хранилища
		/// </summary>
		/// <param name="id">Идентификатор лога</param>
		void Remove(IIdentifier id);
	}
}