using System.Collections.Generic;

namespace DataAbstractionLevel.Low.Storage.Contracts {
	/// <summary>
	/// ������������ ���������� � ������ ���
	/// </summary>
	public interface IPsnDataPaged {
		/// <summary>
		/// ���������� ������ �������
		/// </summary>
		/// <returns>������ �������</returns>
		IEnumerable<IPsnPageIndexRecord> GetPagesIndex();
	}

	/// <summary>
	/// ������������ ���������� � ������ ���
	/// </summary>
	public interface IRpdDataPaged
	{
		/// <summary>
		/// ���������� ������ �������
		/// </summary>
		/// <returns>������ �������</returns>
		IEnumerable<IRpdPageIndexRecord> GetPagesIndex();
	}
}