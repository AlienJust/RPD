using AlienJust.Support.Identy.Contracts;

namespace DataAbstractionLevel.Low.PsnConfig.Contracts {
	/// <summary>
	/// ��������� ����� ������� ���
	/// </summary>
	public interface IPsnProtocolCommandPartConfiguration : IObjectWithIdentifier {
		/// <summary>
		/// �������� ����� �������
		/// </summary>
		string PartName { get; }

		/// <summary>
		/// ��� ����� ������� (������ ��� �����)
		/// </summary>
		PsnProtocolCommandPartType PartType { get; }

		/// <summary>
		/// ������ ��������� ��������, ������������� � ������ ����� �������
		/// </summary>
		IPsnProtocolParameterDefinedConfiguration[] DefParams { get; } // TODO: convert to array

		/// <summary>
		/// ������ ����������, ������������� � ������ ����� �������
		/// </summary>
		IPsnProtocolParameterConfigurationVariable[] VarParams { get; } // TODO: convert to array

		/// <summary>
		/// ����� �����
		/// </summary>
		byte Length { get; }

		/// <summary>
		/// �������� ������������ ������ �������
		/// </summary>
		int Offset { get; }

		/// <summary>
		/// ����� ����������
		/// </summary>
		IPsnProtocolParameterDefinedConfiguration Address { get; }

		/// <summary>
		/// ��� �������
		/// </summary>
		IPsnProtocolParameterDefinedConfiguration CommandCode { get; }

		/// <summary>
		/// ������� ���� CRC
		/// </summary>
		IPsnProtocolParameterConfigurationVariable CrcLow { get; }

		/// <summary>
		/// ������� ���� CRC
		/// </summary>
		IPsnProtocolParameterConfigurationVariable CrcHigh { get; }

		/// <summary>
		/// ������������� �������, � ������� ��������� ������ �����
		/// </summary>
		IIdentifier CommandId { get; }
	}
}
