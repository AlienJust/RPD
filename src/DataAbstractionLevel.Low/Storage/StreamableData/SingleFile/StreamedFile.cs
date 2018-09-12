using System.IO;
using DataAbstractionLevel.Low.Storage.StreamableData.Contracts;

namespace DataAbstractionLevel.Low.Storage.StreamableData.SingleFile
{
	/// <summary>
	/// Потоковый файл
	/// </summary>
	public class StreamedFile : IStreamedData, IStreamedDataWritable {
		private readonly string _filename;

		/// <summary>
		/// Создает новый экземпляр класса
		/// </summary>
		/// <param name="filename">Имя файла</param>
		public StreamedFile(string filename) {
			_filename = filename;
		}

		/// <summary>
		/// Получает поток для чтения данных
		/// </summary>
		/// <returns>Поток для чтения данных</returns>
		public Stream GetStreamForReading() {
			return File.Open(_filename, FileMode.Open, FileAccess.Read, FileShare.Read);
		}

		/// <summary>
		/// Получает поток для записи данных
		/// </summary>
		/// <returns>Поток для записи данных</returns>
		public Stream GetStreamForWriting() {
			return File.Open(_filename, FileMode.Create);
		}
	}
}
