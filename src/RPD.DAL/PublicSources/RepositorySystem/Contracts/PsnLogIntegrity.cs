namespace RPD.DAL {
	/// <summary>
	/// ����������� ��� ����
	/// </summary>
	public enum PsnLogIntegrity {
		/// <summary>
		/// �� � �������
		/// </summary>
		Ok,

		/// <summary>
		/// ������ ��������� ��������� ������� (���� ������ ������� ������������ �� ���������������, ���� ������� ������ �� ������� �����)
		/// </summary>
		PagesFlowError,

		/// <summary>
		/// ��������� �� ��������
		/// </summary>
		Unknown
	}
}