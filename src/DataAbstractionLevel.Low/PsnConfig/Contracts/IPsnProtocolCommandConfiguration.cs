using AlienJust.Support.Identy.Contracts;

namespace DataAbstractionLevel.Low.PsnConfig.Contracts {
	/// <summary>
	/// ���������� � ������� ���
	/// </summary>
	public interface IPsnProtocolCommandConfiguration
	{
		/// <summary>
		/// �������� ������� ���
		/// </summary>
		string Name { get; }

        /// <summary>
        /// ������������� �������
        /// </summary>
		IIdentifier Id { get; }
	}
}