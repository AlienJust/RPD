using System;
using System.Collections.Generic;
using RPD.EventArgs;

namespace RPD.DAL {
	/// <summary>
	/// ���������
	/// </summary>
	public interface ILoader : IDisposable {
		/// <summary>
		/// ������� �������� �����������
		/// </summary>
		/// <returns>������ �� ����� �������� ������������</returns>
		IDeviceConfiguration CreateDeviceConfiguration();

		/// <summary>
		/// �������� ������ ��������� ��������� PSN ������������
		/// </summary>
		IEnumerable<IPsnConfiguration> AvailablePsnConfigruations { get; }

		/// <summary>
		/// �������� ������ �� ����������� � ��������� ����������
		/// </summary>
		/// <param name="directoryPath">���� � ����������</param>
		/// <returns>������ �� �����������</returns>
		IRepository GetLocalDirectoryRepository(string directoryPath);
		
		/// <summary>
		/// �������� ������ �� �������������� �����������
		/// </summary>
		/// <param name="zipFileName">���� � �����-������ �����������</param>
		/// <returns>������ �� �����������</returns>
		IRepository GetZippedRepository(string zipFileName);

		/// <summary>
		/// �������� ������ �� ����������� ���������� ���
		/// </summary>
		/// <param name="nandPath">���� � ���������� ���</param>
		/// <returns>������ �� �����������</returns>
		/// <param name="psnConfiguration">������������ ��� ����������� �� ���� ������ NAND</param>
		/// <returns>������ �� �����������</returns>
		IRepository GetNandFlashRepository(string nandPath, IPsnConfiguration psnConfiguration);

		/// <summary>
		/// �������� ������ �� ����������� ���������� ���, ���������� �� ������ � FTP �������
		/// </summary>
		/// <param name="ftpFilesDirecotyPath">���� � ����� � �������-������� ������� ����� � FTP �������</param>
		/// <returns>������ �� �����������</returns>
		/// <param name="locomotiveName">�������� ����������</param>
		/// <param name="sectionName">�������� ������</param>
		/// <param name="psnConfiguration">������������ ��� ����������� �� ���� ������</param>
		/// <returns>������ �� �����������</returns>
		IRepository GetFtpFilesRepository(string ftpFilesDirecotyPath, string locomotiveName, string sectionName, IPsnConfiguration psnConfiguration);

		/// <summary>
		/// �������� ������ �� ����������� FTP �������
		/// </summary>
		/// <param name="ftpHost">���� FTP �������</param>
		/// <param name="ftpPort">���� FTP ������� </param>
		/// <param name="ftpUsername">������������ FTP �������</param>
		/// <param name="ftpPassword">������ ������������ FTP �������</param>
		/// <param name="deviceNumber">����� ����������, ���������� ���� ������ �� ������</param>
		/// <param name="locomotiveName">�������� ����������</param>
		/// <param name="sectionName">�������� ������ ����������</param>
		/// <param name="psnConfiguration">������������ ��� ����������� �� ���� ������</param>
		/// <returns>������ �� �����������</returns>
		IRepository GetFtpClientRepository(string ftpHost, int ftpPort, string ftpUsername, string ftpPassword, int deviceNumber, string locomotiveName, string sectionName, IPsnConfiguration psnConfiguration);

		/// <summary>
		/// �������� ������������ �������� � ����������� FTP �������
		/// </summary>
		/// <param name="ftpHost">���� FTP �������</param>
		/// <param name="ftpPort">���� FTP �������</param>
		/// <param name="ftpUsername">������������ FTP �������</param>
		/// <param name="ftpPassword">������ ������������ FTP �������</param>
		/// <param name="callbackAction">�������� ��������� ������ � ������� ���������� ������������ ��������� �������, ���� ����������� �������� ����������� �������</param>
		void GetFtpRepositoryInfosAsync(string ftpHost, int ftpPort, string ftpUsername, string ftpPassword, Action<OnCompleteEventArgs, IEnumerable<IFtpRepositoryInfo>> callbackAction);

		/// <summary>
		/// �������� ����������� � FTP �������
		/// </summary>
		/// <param name="ftpRepositoryInfo"></param>
		/// <param name="locomotiveName"></param>
		/// <param name="sectionName"></param>
		/// <param name="psnConfiguration"></param>
		/// <returns></returns>
		IRepository GetFtpRepository(IFtpRepositoryInfo ftpRepositoryInfo, string locomotiveName, string sectionName, IPsnConfiguration psnConfiguration);
	}
}