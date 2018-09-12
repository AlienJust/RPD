using System;

namespace RPD.DAL {
	/// <summary>
	/// ����������� ����� ������� (������������� ����� ������������ ���������� �������)
	/// </summary>
	public interface ICyclePsnCmdPartInfo : IPsnCommandPartInfo
	{
		/// <summary>
		/// ������ ��������� �������
		/// </summary>
		TimeSpan CyclePeriod { get; }
	}
}