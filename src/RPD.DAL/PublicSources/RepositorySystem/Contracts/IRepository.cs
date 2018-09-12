using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RPD.EventArgs;

namespace RPD.DAL
{
	/// <summary>
	/// Хранилище данных (как правило, асинхронное)
	/// </summary>
	public interface IRepository
	{
		/// <summary>
		/// Открывает хранилище
		/// onComplete получает код результата завершения операции
		/// progressChanged получает процент завершения операции
		/// </summary>
		/// <param name="onComplete">Действие обратного вызова по завершению операции</param>
		/// <param name="onProgressChange">Действие обратного вызова при изменении прогресса открытия хранилища</param>
		void Open(Action<OnCompleteEventArgs> onComplete, Action<OnProgressChangeEventArgs> onProgressChange);

		/// <summary>
		/// Список локомотивов
		/// </summary>
		ObservableCollection<ILocomotive> Locomotives { get; }


		/// <summary>
		/// Удаляет секцию из репозитория
		/// </summary>
		/// <param name="section">Что удалять</param>
		/// <param name="onComplete">Действие обратного вызова по завершению удаления</param>
		void Remove(ISection section, Action<OnCompleteEventArgs> onComplete);

		/// <summary>
		/// Удаляет логи ПСН из репозитория
		/// </summary>
		/// <param name="psnLogs">Что удалять</param>
		/// <param name="onComplete">Действие обратного вызова по завершению удаления</param>
		void Remove(IEnumerable<IPsnLog> psnLogs, Action<OnCompleteEventArgs> onComplete);


		/// <summary>
		/// Сохраняет данные в хранилище
		/// </summary>
		/// <param name="sourceRepo">Хранилище источник, к которому принадлежат логи</param>
		/// <param name="psnLogs">ПСН логи</param>
		/// <param name="rpdLogs">РПД логи</param>
		/// <param name="onComplete">Действие обратного вызова по завершению сохранения</param>
		/// <param name="onProgressChange">Действие обратного вызова при изменении прогресса сохранения</param>
		void SaveDataAsync(IRepository sourceRepo, IEnumerable<IPsnLog> psnLogs, IEnumerable<IFaultLog> rpdLogs, Action<OnCompleteEventArgs> onComplete, Action<OnProgressChangeEventArgs> onProgressChange);


		/// <summary>
		/// Метод проверяет наличие такого лога РПД в хранилище
		/// </summary>
		/// <param name="rpdLog">Искомый лог РПД</param>
		/// <returns>Истина, если лог есть в хранилище</returns>
		bool IsExist(IFaultLog rpdLog);


		/// <summary>
		/// Метод проверяет наличие такого лога ПСН в хранилище
		/// </summary>
		/// <param name="psnLog">Искомый лог ПСН</param>
		/// <returns>Истина, если лог есть в хранилище</returns>
		bool IsExist(IPsnLog psnLog);
	}
}
