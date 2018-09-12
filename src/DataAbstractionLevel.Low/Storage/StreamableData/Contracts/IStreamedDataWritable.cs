using System.IO;

namespace DataAbstractionLevel.Low.Storage.StreamableData.Contracts {
	/// <summary>
	/// Потоковые данные для записи
	/// </summary>
	public interface IStreamedDataWritable
	{
		/// <summary>
		/// Создает поток для записи данных в него
		/// </summary>
		/// <returns></returns>
		Stream GetStreamForWriting();
	}
}