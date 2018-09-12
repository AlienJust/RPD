namespace DataAbstractionLevel.Low.PsnConfig.Contracts {
	/// <summary>
	/// ��� ����� ������� ���
	/// </summary>
	public enum PsnProtocolCommandPartType {
		/// <summary>
		/// ������ �������� ����������
		/// </summary>
		Request,

		/// <summary>
		/// ����� �������� ����������
		/// </summary>
		Reply,

		/// <summary>
		/// ����� �������� ����������, ���� ������� ������������� �������� ������� ������� ����� �������
		/// </summary>
		ReplyWithRequiredRequest
	}
}