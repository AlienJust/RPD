namespace DataAbstractionLevel.Low.PsnConfig.Contracts {
	/// <summary>
	/// ������������ ���������� ����� ��� (�������� ����� ���)
	/// </summary>
	public interface IPsnProtocolDeviceConfiguration
	{
		/// <summary>
		/// �������� ����������
		/// </summary>
		string Name { get; }

		/// <summary>
		/// ����� ��������
		/// </summary>
		int Address { get; }
		
		///// <summary>
		///// ������������ �������� ����������
		///// </summary>
		//IEnumerable<IPsnProtocolDeviceSignalConfiguration> Signals { get; }
	}
}