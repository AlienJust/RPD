using DataAbstractionLevel.Low.PsnConfig.Contracts;

namespace RPD.DAL.PsnConfiguration.Contracts.Internal {
	/// <summary>
	/// ������������ ��� ������
	/// </summary>
	internal interface IPsnChannelConfiguration {
		/// <summary>
		/// ����� ������� ����� ��� (������ ��� �����), ������� ����������� �����
		/// </summary>
		IPsnProtocolCommandPartConfiguration CommandPartInfo { get; }

		/// <summary>
		/// ������� ����� ���, ������� ����������� �����
		/// </summary>
		IPsnProtocolParameterConfigurationVariable CommandParameterConfigurationInfo { get; }
	}
}