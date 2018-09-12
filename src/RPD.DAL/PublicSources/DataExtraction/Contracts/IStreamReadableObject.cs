using System.IO;

namespace RPD.DAL {
	/// <summary>
	/// Потоковые данные для чтения
	/// </summary>
	public interface IStreamReadableObject
	{
		/// <summary>
		/// Создает поток для получения данных
		/// </summary>
		/// <returns>Поток для чтения данных</returns>
		Stream GetStreamForReading();
	}
}