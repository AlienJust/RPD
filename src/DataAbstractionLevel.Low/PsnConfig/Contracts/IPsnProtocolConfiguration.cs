using System.Collections.Generic;
using AlienJust.Support.Identy.Contracts;

namespace DataAbstractionLevel.Low.PsnConfig.Contracts {
	/// <summary>
	/// ������������ ��� ���������
	/// </summary>
	public interface IPsnProtocolConfiguration : IObjectWithIdentifier {
		/// <summary>
		/// ���������� � ��� ���������
		/// </summary>
		IPsnProtocolConfigurationInformtaion Information { get; }

		/// <summary>
		/// ������������ ��������� ����� ���
		/// </summary>
		IEnumerable<IPsnProtocolDeviceConfiguration> PsnDevices { get; }

		/// <summary>
        /// ������������ ������������� ���������� ������ ������ ���
		/// </summary>
		IEnumerable<IPsnProtocolCommandPartConfigurationCycle> CycleCommandParts { get; }

        /// <summary>
        /// ������������ ������ ������ ���
        /// </summary>
		IEnumerable<IPsnProtocolCommandPartConfiguration> CommandParts { get; }

        /// <summary>
        /// ������������ ������ ���
        /// </summary>
        IEnumerable<IPsnProtocolCommandConfiguration> Commands { get; }

		IEnumerable<IPsnMergedDevice> MergedDevices { get; }
	}
}