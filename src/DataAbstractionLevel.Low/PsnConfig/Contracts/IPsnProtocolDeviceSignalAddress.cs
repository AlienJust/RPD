namespace DataAbstractionLevel.Low.PsnConfig.Contracts {
	/// <summary>
	/// ����� ������� ���
	/// </summary>
	public interface IPsnProtocolDeviceSignalAddress
	{
		/// <summary>
		/// �������� ���
		/// </summary>
		IPsnProtocolParameterConfiguration ParameterConfiguration { get; }

		/// <summary>
		/// ����� ������� ��������� ���, � ������� ������������� ��������
		/// </summary>
		IPsnProtocolCommandPartConfiguration CommandPart { get; }
	}
}