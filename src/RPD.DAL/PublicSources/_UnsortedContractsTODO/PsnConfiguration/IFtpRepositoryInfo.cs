namespace RPD.DAL {
	/// <summary>
	/// ���������� � �����������, ������������� �� FTP �������
	/// </summary>
	public interface IFtpRepositoryInfo {
		/// <summary>
		/// ��� ��� ����� ������� (��������, my.example.ftp ��� 127.0.0.1)
		/// </summary>
		string FtpHost { get; }

		/// <summary>
		/// ����� ����� (������ 21)
		/// </summary>
		int FtpPort { get; }

		/// <summary>
		/// ��� ������������ �� FTP �������
		/// </summary>
		string FtpUsername { get; }

		/// <summary>
		/// ������ ��� ����������� �� FTP �������
		/// </summary>
		string FtpPassword { get; }

		/// <summary>
		/// ��������� ����� ����������
		/// </summary>
		int DeviceNumber { get; }
	}
}