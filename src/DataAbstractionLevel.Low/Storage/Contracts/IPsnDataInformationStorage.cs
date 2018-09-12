using System;
using System.Collections.Generic;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.Shared;

namespace DataAbstractionLevel.Low.Storage.Contracts {
	/// <summary>
	/// Хранилище данных информации о ПСН логе
	/// </summary>
	public interface IPsnDataInformationStorage {
		/// <summary>
		/// Список логов ПСН
		/// </summary>
		IEnumerable<IPsnDataInformation> PsnDataInformations { get; }

		/// <summary>
		/// Добавляет в хранилище информацию о данных ПСН
		/// </summary>
		/// <param name="psnLogInformationId"></param>
		/// <param name="beginTime"></param>
		/// <param name="endTime"></param>
		/// <param name="saveTime"></param>
		/// <param name="psnDataType"></param>
		/// <param name="isLastDeviceLog"></param>
		/// <param name="deviceInformationId"></param>
		void Add(IIdentifier psnLogInformationId, DateTime? beginTime, DateTime? endTime, DateTime? saveTime, PsnDataFragmentType psnDataType, bool isLastDeviceLog, IIdentifier deviceInformationId);


		/// <summary>
		/// Удаляет из хранилища информацию о данных ПСН
		/// </summary>
		/// <param name="psnLogInformationId"></param>
		void Remove(IIdentifier psnLogInformationId);
	}
}