using System;
using System.Collections.Generic;

namespace RPD.DalRelease.Configuration.CommandConfiguration
{
	/// <summary>
	/// Конфигурация из командного файла
	/// </summary>
	interface ICommandConfiguration
	{
		DateTime? DeviceTime { get; }

		bool IsLinkWithFRAMErrorDetected { get; }
		bool IsLinkWithNANDErrorDetected { get; }
		bool IsMetersTableCSErrorDetected { get; }
		bool IsDumpsTableCRC16ErrorDetected { get; }

		bool IsMetersConfigurationComplete { get; }
		bool IsMetersRequestingInProgress { get; }
		bool IsMetersConfigurationWritingInProgress { get; }
		bool IsMetersConfigurationReadingInProgress { get; }
		bool IsMetersConfigurationReadedAndReadyInNANDForVerification { get; }
		bool IsMeterLinkErrorDetected { get; }
		bool IsLinkWithMetersTestingComplete { get; }

		IEnumerable<IMetersChannelsConfigurationVerificationStatus> ChannelsConfigurationVerificationStatuses { get; }

		ushort PsnProtocolType { get; set; }
	}
}
