using AlienJust.Support.Identy.Contracts;

namespace DataAbstractionLevel.Low.PsnConfig.Contracts {
	/// <summary>
	/// �������� ������������ ��� ���������
	/// </summary>
	public interface IPsnParameterConfigurationColor : IObjectWithIdentifier
	{
		/// <summary>
		/// ���� �� ���������
		/// </summary>
		int DefaultColor { get; set; }
	}
}