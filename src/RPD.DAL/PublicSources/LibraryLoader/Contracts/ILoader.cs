using System;
using System.Collections.Generic;
using RPD.EventArgs;

namespace RPD.DAL {
	/// <summary>
	/// Загрузчик
	/// </summary>
	public interface ILoader : IDisposable {
		/// <summary>
		/// Создает описание кофигурации
		/// </summary>
		/// <returns>Ссылка на новое описание конфигурации</returns>
		IDeviceConfiguration CreateDeviceConfiguration();

		/// <summary>
		/// Получает список доступных программе PSN конфигураций
		/// </summary>
		IEnumerable<IPsnConfiguration> AvailablePsnConfigruations { get; }

		/// <summary>
		/// Получает ссылку на репозиторий в локальной директории
		/// </summary>
		/// <param name="directoryPath">Путь к директории</param>
		/// <returns>Ссылка на репозиторий</returns>
		IRepository GetLocalDirectoryRepository(string directoryPath);
		
		/// <summary>
		/// Получает ссылку на архивированный репозиторий
		/// </summary>
		/// <param name="zipFileName">Путь к файлу-архиву репозитория</param>
		/// <returns>Ссылка на репозиторий</returns>
		IRepository GetZippedRepository(string zipFileName);

		/// <summary>
		/// Получает ссылку на репозиторий устройства РПД
		/// </summary>
		/// <param name="nandPath">Путь к устройству РПД</param>
		/// <returns>Ссылка на репозиторий</returns>
		/// <param name="psnConfiguration">Конфигурация ПСН применяемая ко всем данным NAND</param>
		/// <returns>Ссылка на репозиторий</returns>
		IRepository GetNandFlashRepository(string nandPath, IPsnConfiguration psnConfiguration);

		/// <summary>
		/// Получает ссылку на репозиторий устройства РПД, основанный на файлах с FTP сервера
		/// </summary>
		/// <param name="ftpFilesDirecotyPath">Путь к папке с файлами-дампами взятыми ранее с FTP сервера</param>
		/// <returns>Ссылка на репозиторий</returns>
		/// <param name="locomotiveName">Название локомотива</param>
		/// <param name="sectionName">Название секции</param>
		/// <param name="psnConfiguration">Конфигурация ПСН применяемая ко всем данным</param>
		/// <returns>Ссылка на репозиторий</returns>
		IRepository GetFtpFilesRepository(string ftpFilesDirecotyPath, string locomotiveName, string sectionName, IPsnConfiguration psnConfiguration);

		/// <summary>
		/// Получает ссылку на репозиторий FTP сервера
		/// </summary>
		/// <param name="ftpHost">Хост FTP сервера</param>
		/// <param name="ftpPort">Порт FTP сервера </param>
		/// <param name="ftpUsername">Пользователь FTP сервера</param>
		/// <param name="ftpPassword">Пароль пользователя FTP сервера</param>
		/// <param name="deviceNumber">Номер устройства, скинувшего свои данные на сервер</param>
		/// <param name="locomotiveName">Название локомотива</param>
		/// <param name="sectionName">Название секции локомотива</param>
		/// <param name="psnConfiguration">Конфигурация ПСН применяемая ко всем данным</param>
		/// <returns>Ссылка на репозиторий</returns>
		IRepository GetFtpClientRepository(string ftpHost, int ftpPort, string ftpUsername, string ftpPassword, int deviceNumber, string locomotiveName, string sectionName, IPsnConfiguration psnConfiguration);

		/// <summary>
		/// Получает перечисление сведений о репозториях FTP сервера
		/// </summary>
		/// <param name="ftpHost">Хост FTP сервера</param>
		/// <param name="ftpPort">Порт FTP сервера</param>
		/// <param name="ftpUsername">Пользователь FTP сервера</param>
		/// <param name="ftpPassword">Пароль пользователя FTP севрера</param>
		/// <param name="callbackAction">Действие обратного вызова в которое передается перечисление устройств сервера, если асинхронная операция завершилась успешно</param>
		void GetFtpRepositoryInfosAsync(string ftpHost, int ftpPort, string ftpUsername, string ftpPassword, Action<OnCompleteEventArgs, IEnumerable<IFtpRepositoryInfo>> callbackAction);

		/// <summary>
		/// Получает репозиторий с FTP сервера
		/// </summary>
		/// <param name="ftpRepositoryInfo"></param>
		/// <param name="locomotiveName"></param>
		/// <param name="sectionName"></param>
		/// <param name="psnConfiguration"></param>
		/// <returns></returns>
		IRepository GetFtpRepository(IFtpRepositoryInfo ftpRepositoryInfo, string locomotiveName, string sectionName, IPsnConfiguration psnConfiguration);
	}
}