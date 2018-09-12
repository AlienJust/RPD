using System.Collections.Generic;
using RPD.DAL;
using RPD.DalRelease.Configuration.System.Contracts;

namespace RPD.DalRelease.Configuration {
	class DeviceDiagnosticSimple : IDeviceDiagnostic {
		public int FirmwareVersion { get; set; }
		public bool LostFRAM { get; set; }
		public bool LostNAND { get; set; }
		public bool ErrorRpdMetersTableCRC { get; set; }
		public bool ErrorDumpTableCRC { get; set; }
		public int FaultLogsCount { get; set; }
		public List<long> BadBlocks { get; set; }

		public int BadBlocksCount { get; set; }
		public int LastWrittenPageAddress { get; set; }
		public int LastReadedBlockAddress { get; set; }
		public int LastWrittenPageNumber { get; set; }
		public int FirstWrittenAfterResetPageNumber { get; set; }
		public int PsnLogStartPageNumber { get; set; }
		public int ArrayDumpPsnStartPageNumber { get; set; }
		public int FatOffsetFromPageZero { get; set; }
		public int RpdPagesCountTransmittedToPsnLog { get; set; }
		public IList<IRpdLogInfo> FaultDumpsTable { get; set; }
		public int CurrentPsnLogNumber { get; set; }
		public IList<IPsnLogFragmentInfo> PsnLogPowerUpFragmentInfos { get; set; }
		public IList<IPsnLogFragmentInfo> PsnLogPredefinedFragmentInfos { get; set; }
	}
}