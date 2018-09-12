using System.Collections.Generic;

namespace RPD.DAL {
	/// <summary>
	/// ����������� �������� ���
	/// </summary>
	public interface IPsnVirtualParameter : IObjectWithId
	{
		/// <summary>
		/// �������� ���������
		/// </summary>
		string Name { get; }

		/// <summary>
		/// ��������� ���������
		/// </summary>
		string Expression { get; }

		/// <summary>
		/// ����� ��������� (������ �� �������� ���������)
		/// </summary>
		IEnumerable<IPsnVirtualParameterPart> Parts { get; }
	}
}