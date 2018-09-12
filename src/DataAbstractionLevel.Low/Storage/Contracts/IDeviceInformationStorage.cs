using System.Collections.Generic;
using AlienJust.Support.Identy.Contracts;

namespace DataAbstractionLevel.Low.Storage.Contracts {

	/// <summary>
	/// Хранилище информаций об устройствах
	/// </summary>
	public interface IDeviceInformationStorage {
		/// <summary>
		/// Перечисление всех хранящихся информаций об устройствах
		/// </summary>
		IEnumerable<IDeviceInformation> DeviceInformations { get; }

		/// <summary>
		/// Добавляет новую информацию об устройстве
		/// </summary>
		/// <param name="id">Идентификатор инфомрации об устройстве</param>
		/// <param name="name">Название устройства (имя локомотива)</param>
		/// <param name="description">Описание устройства (номер секции)</param>
		void Add(IIdentifier id, string name, string description);

		/// <summary>
		/// Удаляет информацию об устройстве
		/// </summary>
		/// <param name="id">Идентификатор информации для удаления</param>
		void Remove(IIdentifier id);

		/// <summary>
		/// Обновляет информацию об устройстве
		/// </summary>
		/// <param name="id">Идентификатор информации для обновления</param>
		/// <param name="setName">Новое имя устройства</param>
		/// <param name="setDescription">Новое описание</param>
		void Update(IIdentifier id, string setName, string setDescription);
	}
}