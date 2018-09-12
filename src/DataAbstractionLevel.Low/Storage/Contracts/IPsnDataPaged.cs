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
}