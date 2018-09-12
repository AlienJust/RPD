using System.Collections.Generic;
using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts;

namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Common
{
	public class SystemConfigurationSimple : ISystemConfiguration
	{
		public int DeviceAddress { get; set; }
		public int NetAddress { get; set; }

		public int LocomotiveNumber { get; set; }
		public int SectionNumber { get; set; }

		public int FirmwareVersion { get; set; }
		public int LastWrittenPageAddress { get; set; }
		public int LastReadedBlockAddress { get; set; }
		public int BadBlocksCount { get; set; }
		public int LastWrittenPageNumber { get; set; }
		public int FirstWrittenAfterResetPageNumber { get; set; }
		public int PsnLogStartPageNumber { get; set; }
		public int ArrayDumpPsnStartPageNumber { get; set; }
		public int FatOffsetFromPageZero { get; set; }
		public int RpdPagesCountTransmittedToPsnLog { get; set; }
		public int ConfigurationByte { get; set; }
		
		public int FaultsCount { get; set; }
		public IList<IRpdDataInformation> FaultDumpsTable { get; set; }
		
		public int MetersCount { get; set; }
		public IList<IRpdMeterSystemInformation> MetersTable { get; set; }
		public byte[] PsnRegisterStatusMasks { get; set; }
		public IList<IRpdDumpRule> DumpRules { get; set; }

		public int CurrentPsnLogNumber { get; set; }
		public IList<IPsnDataFragmentInformation> PsnLogPowerUpFragmentInfos { get; set; }
		public IList<IPsnDataFragmentInformation> PsnLogPredefinedFragmentInfos { get; set; }
	}
}
