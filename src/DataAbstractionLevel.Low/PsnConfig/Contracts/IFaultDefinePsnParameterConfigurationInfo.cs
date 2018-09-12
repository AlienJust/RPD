namespace DataAbstractionLevel.Low.PsnConfig.Contracts {
	public interface IFaultDefinePsnParameterConfigurationInfo : IPsnProtocolParameterConfigurationVariable
	{
		/// <summary>
		/// ����� ���������� ����� ������������
		/// </summary>
		int RpdConfPosByte { get; }

		/// <summary>
		/// ����� ���������� ���� ������������
		/// </summary>
		int RpdConfPosBit { get; }
	}
}