using RPD.DalRelease.Configuration;

namespace RPD.DAL {
	/// <summary>
	/// ���������� � ���� ���
	/// </summary>
	public interface IRpdLogInfo
	{
		/// <summary>
		/// �����
		/// </summary>
		int Number { get; }

		/// <summary>
		/// ������
		/// </summary>
		int Status { get; }

		/// <summary>
		/// ���
		/// </summary>
		int Year { get; }

		/// <summary>
		/// �����
		/// </summary>
		int Month { get; }

		/// <summary>
		/// ����
		/// </summary>
		int Day { get; }

		/// <summary>
		/// ���
		/// </summary>
		int Hour { get; }

		/// <summary>
		/// ������
		/// </summary>
		int Minute { get; }

		/// <summary>
		/// �������
		/// </summary>
		int Second { get; }

		/// <summary>
		/// ����� �������� � ���������
		/// </summary>
		int DescriptionPageAddress { get; }

		/// <summary>
		/// ����� ��������� ���������� �������� 
		/// </summary>
		int LastWrittenPageAddress { get; }

		/// <summary>
		/// ������ ���� ���������
		/// </summary>
		int FaultWasReaded { get; }

		/// <summary>
		/// ����� ������ �������
		/// </summary>
		int BadPageCounter { get; }
	}
}