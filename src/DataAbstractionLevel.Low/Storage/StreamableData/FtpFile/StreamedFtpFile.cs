using System.IO;
using DataAbstractionLevel.Low.InternalKitchen.Streamable;
using DataAbstractionLevel.Low.Storage.StreamableData.Contracts;

namespace DataAbstractionLevel.Low.Storage.StreamableData
{
	/// <summary>
	/// Потоковый файл на FTP сервере
	/// </summary>
	public class StreamedFtpFile : IStreamedData, IStreamedDataWritable {
		private readonly string _ftpHost;
		private readonly int _ftpPort;
		private readonly string _username;
		private readonly string _password;
		private readonly string _filename;


		/// <summary>
		/// Создает новый экземпляр класса
		/// </summary>
		/// <param name="ftpHost">Узел FTP сервера</param>
		/// <param name="ftpPort">TCP Порт FTP сервера</param>
		/// <param name="username">Имя пользователя FTP сервера</param>
		/// <param name="password">Пароль пользователя</param>
		/// <param name="filename">Имя файла на сервере</param>
		public StreamedFtpFile(string ftpHost, int ftpPort, string username, string password, string filename) {
			_ftpHost = ftpHost;
			_ftpPort = ftpPort;
			_username = username;
			_password = password;
			_filename = filename;
		}

		/// <summary>
		/// Получает поток для чтения данных
		/// </summary>
		/// <returns>Поток для чтения данных</returns>
		public Stream GetStreamForReading() {
			return new FtpFileStream(_ftpHost, _ftpPort, _username, _password, _filename, true);
		}

		/// <summary>
		/// Получает поток для записи данных
		/// </summary>
		/// <returns>Поток для записи данных</returns>
		public Stream GetStreamForWriting() {
			return new FtpFileStream(_ftpHost, _ftpPort, _username, _password, _filename, false);
		}
	}
}
