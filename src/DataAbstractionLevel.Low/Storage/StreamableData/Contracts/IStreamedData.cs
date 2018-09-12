using System.IO;

namespace DataAbstractionLevel.Low.Storage.StreamableData.Contracts {
	/// <summary>
	/// Потоковые данные для чтения
	/// </summary>
	public interface IStreamedData {
		/// <summary>
		/// Создает поток для получения данных
		/// </summary>
		/// <returns>Поток для чтения данных</returns>
		Stream GetStreamForReading();
	}
}