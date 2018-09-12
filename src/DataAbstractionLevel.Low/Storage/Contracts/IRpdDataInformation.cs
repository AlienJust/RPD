using System;
using AlienJust.Support.Identy.Contracts;

namespace DataAbstractionLevel.Low.Storage.Contracts {
	/// <summary>
	/// Информация о логе РПД
	/// </summary>
	public interface IRpdDataInformation : IObjectWithIdentifier
	{
		/// <summary>
		/// Время начала лога
		/// </summary>
		DateTime? BeginTime { get; }

		/// <summary>
		/// Время завершения лога
		/// </summary>
		DateTime? EndTime { get; }

		/// <summary>
		/// Время сохранения лога в репозиторий
		/// </summary>
		DateTime? SaveTime { get; }

		/// <summary>
		/// Флаг того, что лог является последним логом на устройстве
		/// </summary>
		bool IsLastDeviceLog { get; }

		/// <summary>
		/// Идентификатор информации об устройстве
		/// </summary>
		IIdentifier DeviceInformationId { get; }

		//IUid LinkedPsnLogId { get; }
	}
}