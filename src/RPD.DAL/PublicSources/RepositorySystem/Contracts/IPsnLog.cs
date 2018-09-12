using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RPD.DAL {
	/// <summary>
	/// Лог магистрали ПСН
	/// </summary>
	public interface IPsnLog : IObjectWithId, IStreamReadableObject
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
		PsnLogType LogType { get; }

		/// <summary>
		/// Признак, что лог является последним на устройстве
		/// </summary>
		bool IsLastDeviceLog { get; }

		/// <summary>
		/// Измерители
		/// </summary>
		ObservableCollection<IPsnMeter> Meters { get; }


		/// <summary>
		/// Обновляет конфигурацию ПСН
		/// </summary>
		/// <param name="psnConfig">ПСН конфигурация</param>
		/// <param name="customName">Пользовательское название для лога</param>
		void Update(IPsnConfiguration psnConfig, string customName);

		/// <summary>
		/// Конфигурация ПСН для данного лога
		/// </summary>
		IPsnConfiguration PsnConfiguration { get; }

		/// <summary>
		/// Получить статистику о логе асинхронно
		/// </summary>
		/// <param name="callback">Метод обратного вызова, с параметрами исключения во время сбора статистики и результатом в текстовом виде</param>
		void GetStatisticAsync(Action<Exception, IEnumerable<string>> callback);

		/// <summary>
		/// Что то не так с логом?
		/// </summary>
		PsnLogIntegrity LogIntegrity { get; }

		/// <summary>
		/// Событие, вызываемое при смене состояния флага LogIntegrity
		/// </summary>
		event EventHandler IsSomethingWrongWithLogChanged;


		/// <summary>
		/// Название лога
		/// </summary>
		string Name { get; }
	}
}