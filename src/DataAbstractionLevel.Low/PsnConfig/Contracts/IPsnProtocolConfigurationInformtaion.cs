using AlienJust.Support.Identy.Contracts;

namespace DataAbstractionLevel.Low.PsnConfig.Contracts {
	/// <summary>
	/// �������� ������������ ���
	/// </summary>
	public interface IPsnProtocolConfigurationInformtaion : IObjectWithIdentifier
	{
		/// <summary>
		/// �������� ������������
		/// </summary>
		string Name { get; }
		
		/// <summary>
		/// ����� ��������� �������� ������������, ������ ������ ��������
		/// </summary>
		string Description { get; }

		/// <summary>
		/// ������ ������������
		/// </summary>
		string Version { get; }

		/// <summary>
		/// ������������� ������������ � ����� ���
		/// </summary>
		string RpdId { get; }
	}
}