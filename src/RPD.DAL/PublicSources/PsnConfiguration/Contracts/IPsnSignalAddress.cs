namespace RPD.DAL {
	/// <summary>
	/// ����� ������� ���
	/// </summary>
	public interface IPsnSignalAddress
	{
		/// <summary>
		/// �������� ���
		/// </summary>
		IPsnParameterConfiguration ParameterConfiguration { get; }

		/// <summary>
		/// �������, � ������� ������������� ��������
		/// </summary>
		IPsnCommandPartInfo CommandPart { get; }
	}
}