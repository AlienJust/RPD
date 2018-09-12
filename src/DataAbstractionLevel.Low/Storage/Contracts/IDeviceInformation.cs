using AlienJust.Support.Identy.Contracts;

namespace DataAbstractionLevel.Low.Storage.Contracts {
	/// <summary>
	/// ���������� �� ����������
	/// </summary>
	public interface IDeviceInformation : IObjectWithIdentifier {
		/// <summary>
		/// �������� ����������
		/// </summary>
		string Name { get; }

		/// <summary>
		/// �������� ����������
		/// </summary>
		string Description { get; }
	}
}