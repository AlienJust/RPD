namespace RPD.DAL {
	/// <summary>
	/// Информация о репозитории, расположенном на FTP сервере
	/// </summary>
	public interface IFtpRepositoryInfo {
		/// <summary>
		/// Имя или адрес сервера (например, my.example.ftp или 127.0.0.1)
		/// </summary>
		string FtpHost { get; }

		/// <summary>
		/// Номер порта (обычно 21)
		/// </summary>
		int FtpPort { get; }

		/// <summary>
		/// Имя пользователя на FTP сервере
		/// </summary>
		string FtpUsername { get; }

		/// <summary>
		/// Пароль для авторизации на FTP сервере
		/// </summary>
		string FtpPassword { get; }

		/// <summary>
		/// Заводской номер устройства
		/// </summary>
		int DeviceNumber { get; }
	}
}