namespace DataAbstractionLevel.Low.Storage.Contracts {
	/// <summary>
	/// ��� ����������
	/// </summary>
	public enum RpdProtocolMeterType {
		/// <summary>
		/// ��� ���������� �� ���������
		/// </summary>
		Undefined = 0xFF,

		/// <summary>
		/// ����
		/// </summary>
		Uran = 0,

		/// <summary>
		/// ����
		/// </summary>
		Irvi = 1,

		/// <summary>
		/// �����
		/// </summary>
		Radar = 2
	}
}