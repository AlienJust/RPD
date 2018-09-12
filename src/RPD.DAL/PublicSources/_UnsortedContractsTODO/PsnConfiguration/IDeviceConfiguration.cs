using System;
using System.Collections.ObjectModel;
using RPD.EventArgs;

namespace RPD.DAL {
	/// <summary>
	/// Интерфейс для работы с конфигурацией устройства
	/// </summary>
	public interface IDeviceConfiguration {
		/// <summary>
		/// Фабрика измерителей
		/// </summary>
		/// <returns>Ссылка на новый измеритель</returns>
		IRpdMeter CreateMeter();

		/// <summary>
		/// Список измерителей, указанных в конфигурации
		/// </summary>
		ObservableCollection<IRpdMeter> RpdMeters { get; set; }

		/// <summary>
		/// Имя локомотива
		/// </summary>
		string LocomotiveName { get; set; }

		/// <summary>
		/// Номер секции
		/// </summary>
		int SectionNumber { get; set; }

		/// <summary>
		/// Комментарий для человечков.
		/// </summary>
		string Comment { get; set; }

		/// <summary>
		/// Адрес прибора.
		/// </summary>
		int Address { get; set; }

		/// <summary>
		/// Сетевой адрес.
		/// </summary>
		int NetAddress { get; set; }

		/// <summary>
		/// Применять код Хэминга к содержимому страниц в NAND.
		/// </summary>
		bool UseHammingForNand { get; set; }

		/// <summary>
		/// Вести лог магистрали ПСН.
		/// </summary>
		bool LogPsn { get; set; }

		/// <summary>
		/// Сохранять межбайтовый интервал в обмене ПСН.
		/// </summary>
		bool SaveByteInterval { get; set; }

		/// <summary>
		/// Сбрасывать параметры в таблице аварий и освобождать память под дампы аварий, если считаны все файлы дампов аварий.
		/// </summary>
		bool ResetFaultsDump { get; set; }

		/// <summary>
		/// Диагностика устройства
		/// </summary>
		IDeviceDiagnostic Diagnostic { get; }

		/// <summary>
		/// Текущее время внутренних часов блока РПД
		/// </summary>
		DateTime? CurrentTime { get; }


		/// <summary>
		/// Номер протокола магистрали ПСН
		/// </summary>
		int PsnProtocolNumber { get; set; }

		/// <summary>
		/// Номер локомотива
		/// </summary>
		int LocomotiveNumber { get; set; }
		#region deprecated

		/// <summary>
		/// Чтение конфигурации
		/// </summary>
		/// <param name="path">Путь к устройству</param>
		/// <param name="onComplete">Callback завершения операции</param>
		void Read(string path, Action<OnCompleteEventArgs> onComplete);

		/// <summary>
		/// Запись конфигурации
		/// </summary>
		/// <param name="path">Путь к устройству</param>
		/// <param name="onComplete">Callback завершения операции</param>
		void Write(string path, Action<OnCompleteEventArgs> onComplete);


		//11.01.2011г.------------------------------------------------------------------------------------------------------+
		/// <summary>
		/// "Очистка архивов" - при вызове этого метода производится сброс счетчика аварий и выдача служебной команды для РПД, файлы архивов на самом деле не трогаются
		/// </summary>
		/// <param name="driveLetter">Буква диска, которой определилось устройство в системе, например F</param>
		void ClearArchives(string driveLetter);


		/// <summary>
		/// Асинхронная очистка архивов
		/// </summary>
		/// <param name="driveLetter">Буква диска устройства</param>
		/// <param name="onComplete">Callback-метод завершения операции</param>
		void ClearArchivesAsync(string driveLetter, Action<OnCompleteEventArgs> onComplete);

		/// <summary>
		/// Асинхронное форматирование РПД
		/// </summary>
		/// <param name="driveLetter">Буква диска устройства</param>
		/// <param name="onComplete">Callback-метод завершения операции</param>
		void FormatRpdAsync(string driveLetter, Action<OnCompleteEventArgs> onComplete);

		/// <summary>
		/// Асинхронная проверка связи с измерителями
		/// </summary>
		/// <param name="driveLetter">Буква диска устройства</param>
		/// <param name="onComplete">Callback-метод завершения операции</param>
		void TestLinkWithMetersAsync(string driveLetter, Action<OnCompleteEventArgs> onComplete);

		/// <summary>
		/// Прошивка ПО устройства
		/// </summary>
		/// <param name="deviceDriveLetter">Буква диска устройства</param>
		/// <param name="firmwireHexFilename">Путь к файлу прошивки</param>
		/// <param name="onComplete">Callback-метод завершения операции</param>
		void WriteFirmware(string firmwireHexFilename, string deviceDriveLetter,
		                   Action<OnCompleteEventArgs> onComplete);

		/// <summary>
		/// Установка времени внутренних часов РПД
		/// </summary>
		/// <param name="time">Время для установки</param>
		/// <param name="deviceDriveLetter">Буква диска устройства</param>
		/// <param name="onComplete">Callback-метод завершения операции</param>
		void SetTime(DateTime time, string deviceDriveLetter, Action<OnCompleteEventArgs> onComplete);

		#endregion
	}
}
