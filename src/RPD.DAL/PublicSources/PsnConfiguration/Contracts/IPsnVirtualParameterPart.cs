namespace RPD.DAL {
	/// <summary>
	/// ����� ������������ ��������� ���
	/// </summary>
	public interface IPsnVirtualParameterPart
	{
		/// <summary>
		/// ������������� ��������� ���������, �� ������� ��������� �����
		/// </summary>
		IUid RealParameterId { get; }

		/// <summary>
		/// �������� �����, ��� ������� ��� �������� ������ ���������
		/// </summary>
		string ExpressionName { get; }
	}
}