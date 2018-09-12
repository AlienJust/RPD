using System;

namespace RPD.DAL {
	/// <summary>
	/// ���������� � ��������� ���� ���
	/// </summary>
	public interface IPsnLogFragmentInfo
	{
		/// <summary>
		/// �������� ������
		/// </summary>
		int StartOffset { get; }

		/// <summary>
		/// ������ ��������� ����
		/// </summary>
		DateTime? StartTime { get; }
	}
}