using AlienJust.Support.Identy.Contracts;

namespace DataAbstractionLevel.Low.Storage.Contracts {
	/// <summary>
	/// ���� (����������������) ���������� � ���� ���
	/// </summary>
	public interface IRpdDataCustomConfigration : IObjectWithIdentifier
	{
		/// <summary>
		/// ������������� ������������ ���
		/// </summary>
		IIdentifier RpdConfigurationId { get; }

		/// <summary>
		/// �������� ���� ���
		/// </summary>
		string CustomLogName { get; }
	}
}