using System.Collections.Generic;

namespace RPD.DAL {
	/// <summary>
	/// ����������� ���������� ���
	/// </summary>
	public interface IPsnVirtualDevice {
		/// <summary>
		/// �������� ����������
		/// </summary>
		string Name { get; }

		/// <summary>
		/// ��������� ���������� (���� �����������)
		/// </summary>
		IEnumerable<IPsnVirtualParameter> Parameters { get; }
	}
}