namespace RPD.DAL {
	/// <summary>
	/// ������ ���
	/// </summary>
	public interface IPsnSignalConfiguration
	{
		/// <summary>
		/// �������� �������
		/// </summary>
		string Name { get; }

		/// <summary>
		/// ����, �����������, ��� ������ �������
		/// </summary>
		bool IsBooleanValued { get; }


		/// <summary>
		/// ����� �������, ����������� ��� ������ ������ �� ������� ������ ����
		/// </summary>
		IPsnSignalAddress Address { get; }
	}
}