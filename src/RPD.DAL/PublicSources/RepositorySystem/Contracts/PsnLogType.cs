namespace RPD.DAL {
	/// <summary>
	/// ��� ���� ���
	/// </summary>
	public enum PsnLogType {
		/// <summary>
		/// ��� ������������������ �������
		/// </summary>
		FixedLength,

		/// <summary>
		/// ���, ��������� �� ������� ���
		/// </summary>
		PowerDepended,

		/// <summary>
		/// �������� � �������
		/// </summary>
		LinkedToFault
	}
}