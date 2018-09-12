using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.StreamableData.Contracts;

namespace DataAbstractionLevel.Low.Storage.Contracts {
	/// <summary>
	/// Данные магистрали ПСНа
	/// </summary>
	public interface IRpdDataStreamed : IStreamedData, IObjectWithIdentifier
	{
		//// <summary>
		//// Загрузить данные сигнала (синхронно, для асинхронной загрузки воспользуйтесь методом репозитория)
		//// </summary>
		//// <param name="configuration">Конфигурация РПД</param>
		//// <param name="rpdSignalAddress">Адрес сигнала РПД</param>
		//// <param name="beginTime">Время отсчета (начала) лога</param>
		//IEnumerable<IDataPoint> LoadTrend(IRpdConfiguration configuration, IRpdSignalAddress rpdSignalAddress, DateTime beginTime);

		/// <summary>
		/// Сообщает о выгрузке каких-либо данных
		/// </summary>
		void UnloadSomeTrend();

		/// <summary>
		/// Постраничная информация, если имеется
		/// </summary>
		IRpdDataPaged PagesInformation { get; }
	}
}