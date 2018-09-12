using System;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.Shared;

namespace DataAbstractionLevel.Low.Storage.Contracts {
	/// <summary>
	/// Информация о ПСН логе
	/// </summary>
	public interface IPsnDataInformation : IObjectWithIdentifier
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
		/// Тип лога
		/// </summary>
		PsnDataFragmentType DataFragmentType { get; }

		/// <summary>
		/// Флаг того, что лог является последним логом на устройстве
		/// </summary>
		bool IsLastDeviceLog { get; }

		/// <summary>
		/// Идентификатор информации об устройстве
		/// </summary>
		IIdentifier DeviceInformationId { get; }
	}
}