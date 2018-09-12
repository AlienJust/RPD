namespace DataAbstractionLevel.Low.PsnConfig.Contracts {
	/// <summary>
	/// ���������� �� ���������� ���
	/// </summary>
	public interface IPsnProtocolMeterConfiguration {
		/// <summary>
		/// �������� ����������
		/// </summary>
		string Name { get; }

		/// <summary>
		/// ����� ����������
		/// </summary>
		int Address { get; }
	}
}