namespace DataAbstractionLevel.Low.PsnConfig.Contracts {
	/// <summary>
	/// ������ ���
	/// </summary>
	public interface IPsnProtocolDeviceSignalConfiguration
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
		IPsnProtocolDeviceSignalAddress Address { get; }
	}
}