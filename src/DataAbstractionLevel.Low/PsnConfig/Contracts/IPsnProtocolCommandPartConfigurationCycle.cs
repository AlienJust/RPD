using System;

namespace DataAbstractionLevel.Low.PsnConfig.Contracts {
	/// <summary>
	/// ����������� ����� ������� (������������� ����� ������������ ���������� �������)
	/// </summary>
	public interface IPsnProtocolCommandPartConfigurationCycle : IPsnProtocolCommandPartConfiguration
	{
		/// <summary>
		/// ������ ��������� �������
		/// </summary>
		TimeSpan CyclePeriod { get; }
	}
}