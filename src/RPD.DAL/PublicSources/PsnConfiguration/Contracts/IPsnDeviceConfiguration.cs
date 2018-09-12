namespace RPD.DAL {
	/// <summary>
	/// ������������ ���������� ����� ��� (�������� ����� ���)
	/// </summary>
	public interface IPsnDeviceConfiguration
	{
		/// <summary>
		/// �������� ����������
		/// </summary>
		string Name { get; }

		/// <summary>
		/// ����� �������� ���������� ���
		/// </summary>
		int Address { get; }
	}
}