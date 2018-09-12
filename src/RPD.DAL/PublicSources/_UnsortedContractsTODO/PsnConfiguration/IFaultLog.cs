using System;
using System.Collections.ObjectModel;

namespace RPD.DAL
{
	/// <summary>
	/// Класс, описывающий аварию
	/// </summary>
	public interface IFaultLog
	{
		/// <summary>
		/// Некое имя аварии, для дальнейшего дополнительного идентифицирования пользователем
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// Список сигналов аварии (составляется по конфигурации)
		/// </summary>
		ObservableCollection<ISignal> Signals { get; }

		/// <summary>
		/// Список измерителей РПД
		/// </summary>
		ObservableCollection<IRpdMeter> RpdMeters { get; }

		/// <summary>
		/// Лог ПСН, приписанный к аварии
		/// </summary>
		IPsnLog PsnFaultLog { get; }
		
		/// <summary>
		/// Время срабатывания аварии (Марат сказал, что требуется точность до 1сек, так что DateTime справится с задачей)
		/// </summary>
		DateTime AccuredAt { get; }

		/// <summary>
		/// Секция локомотива, которой принадлежит данная авария
		/// </summary>
		ISection OwnerSection { get; }

		/// <summary>
		/// Флаг того, что авария сохранена в репозиторий
		/// </summary>
		bool SavedToRepository { get; }

		/// <summary>
		/// Конфигурация устройства, которому принадлежит данная авария
		/// </summary>
		IDeviceConfiguration DeviceConfig { get; }

		/// <summary>
		/// Причина аварии
		/// </summary>
		string LogReason { get; }
	}
}
