using System;

namespace DataAbstractionLevel.Low.Storage.Contracts {

	/// <summary>
	/// ������ ������� ������� ���� ���
	/// </summary>
	public interface IPsnPageIndexRecord {
		/// <summary>
		/// ���������� � ��������
		/// </summary>
		PsnPageInfo PageInfo { get; }

		/// <summary>
		/// ���������� �������� � ������ ������
		/// </summary>
		long AbsolutePositionInStream { get; }

		/// <summary>
		/// ���������� ����� ��������
		/// </summary>
		long AbsolutePageNumber { get; }

		/// <summary>
		/// ���� � �����
		/// </summary>
		DateTime? PageTime { get; }

		/// <summary>
		/// ����� ��������
		/// </summary>
		int PageNumber { get; }
	}
}