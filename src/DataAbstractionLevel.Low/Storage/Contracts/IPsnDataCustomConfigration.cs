using AlienJust.Support.Identy.Contracts;

namespace DataAbstractionLevel.Low.Storage.Contracts {
	/// <summary>
	/// ���� (����������������) ���������� � ���� ���
	/// </summary>
	public interface IPsnDataCustomConfigration : IObjectWithIdentifier
	{
		/// <summary>
		/// ������������� ������������ ���
		/// </summary>
		IIdentifier PsnConfigurationId { get; }

		/// <summary>
		/// �������� ���� ���
		/// </summary>
		string CustomLogName { get; }
	}
}