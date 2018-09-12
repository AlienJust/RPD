namespace RPD.DAL.PsnConfiguration.Contracts.Internal {
	/// <summary>
	/// ������������ ��������� ���������� ���
	/// </summary>
	interface IPsnConfigurationInformation : IObjectWithId
	{
		/// <summary>
		/// �������� ������������
		/// </summary>
		string Name { get; }

		/// <summary>
		/// ����� ��������� �������� ������������, ������ ������ ��������
		/// </summary>
		string Description { get; }

		/// <summary>
		/// ������ ������������
		/// </summary>
		string Version { get; }

		/// <summary>
		/// ������������� ������������ � ����� ���
		/// </summary>
		string RpdId { get; }
	}
}