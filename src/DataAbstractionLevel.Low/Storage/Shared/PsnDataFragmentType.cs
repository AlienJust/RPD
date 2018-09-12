namespace DataAbstractionLevel.Low.Storage.Shared {
	/// <summary>
	/// ��� ���� ���
	/// </summary>
	public enum PsnDataFragmentType {
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