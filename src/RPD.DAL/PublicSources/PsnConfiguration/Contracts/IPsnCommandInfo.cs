namespace RPD.DAL {
	/// <summary>
	/// ���������� � ������� ���
	/// </summary>
	public interface IPsnCommandInfo
	{
		/// <summary>
		/// �������� ������� ���
		/// </summary>
		string Name { get; }

		/// <summary>
		/// ������������� �������
		/// </summary>
		IUid Id { get; }
	}
}