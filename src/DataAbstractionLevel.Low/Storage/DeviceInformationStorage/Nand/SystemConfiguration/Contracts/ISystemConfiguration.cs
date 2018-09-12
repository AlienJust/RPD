using System.Collections.Generic;

namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts {
	public interface ISystemConfiguration {
		int DeviceAddress { get; set; }
		int NetAddress { get; set; }
		
		int LocomotiveNumber { get; set; }
		int SectionNumber { get; set; }

		int FirmwareVersion { get; set; }

		int LastWrittenPageAddress { get; set; }
		int LastReadedBlockAddress { get; set; }

		int BadBlocksCount { get; set; }

		int LastWrittenPageNumber { get; set; }
		int FirstWrittenAfterResetPageNumber { get; set; }
		int PsnLogStartPageNumber { get; set; }
		int ArrayDumpPsnStartPageNumber { get; set; }

		int FatOffsetFromPageZero { get; set; }
		int RpdPagesCountTransmittedToPsnLog { get; set; }
		int ConfigurationByte { get; set; }


		int FaultsCount { get; set; }
		IList<IRpdDataInformation> FaultDumpsTable { get; }

		int MetersCount { get; set; }
		IList<IRpdMeterSystemInformation> MetersTable { get; }
		byte[] PsnRegisterStatusMasks { get; }
		IList<IRpdDumpRule> DumpRules { get; set; }

		int CurrentPsnLogNumber { get; set; }
		IList<IPsnDataFragmentInformation> PsnLogPowerUpFragmentInfos { get; set; }
		IList<IPsnDataFragmentInformation> PsnLogPredefinedFragmentInfos { get; set; }
	}
}