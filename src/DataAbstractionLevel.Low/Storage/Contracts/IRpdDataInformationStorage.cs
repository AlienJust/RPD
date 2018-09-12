using System;
using System.Collections.Generic;
using AlienJust.Support.Identy.Contracts;

namespace DataAbstractionLevel.Low.Storage.Contracts {
	/// <summary>
	/// Хранилище данных информации о РПД логе
	/// </summary>
	public interface IRpdDataInformationStorage
	{
		/// <summary>
		/// Список объектов хранилища (объекты: информация о логе РПД)
		/// </summary>
		IEnumerable<IRpdDataInformation> RpdDataInformations { get; }

		/// <summary>
		/// Добавляет в хранилище информацию о данных ПСН
		/// </summary>
		/// <param name="rpdLogInformationId">Идентификатор объекта</param>
		/// <param name="beginTime">Время начала лога</param>
		/// <param name="endTime">Время окончания лога</param>
		/// <param name="saveTime">Время сохранения лога</param>
		/// <param name="isLastDeviceLog">Является ли лог последним на устройстве</param>
		/// <param name="deviceInformationId">Идентификатор информации об устройстве</param>
		void Add(IIdentifier rpdLogInformationId, DateTime? beginTime, DateTime? endTime, DateTime? saveTime, bool isLastDeviceLog, IIdentifier deviceInformationId);


		/// <summary>
		/// Удаляет из хранилища информацию о данных РПД
		/// </summary>
		/// <param name="rpdLogInformationId">Идентификатор лога для удаления</param>
		void Remove(IIdentifier rpdLogInformationId);
	}
}