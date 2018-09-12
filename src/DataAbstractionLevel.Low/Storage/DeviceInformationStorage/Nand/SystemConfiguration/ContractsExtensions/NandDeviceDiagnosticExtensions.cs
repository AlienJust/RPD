using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts;

namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.ContractsExtensions {
	public static class NandDeviceDiagnosticExtensions
	{
		public static bool IsEqualTo(this INandDeviceDiagnostic diag1, INandDeviceDiagnostic diag2)
		{
			if (diag1.ArrayDumpPsnStartPageNumber != diag2.ArrayDumpPsnStartPageNumber) return false;
			if (diag1.BadBlocksCount != diag2.BadBlocksCount) return false;
			if (diag1.BadBlocks.Count != diag2.BadBlocks.Count) return false;
			for (int i = 0; i < diag1.BadBlocks.Count; ++i) {
				if (diag1.BadBlocks[i] != diag2.BadBlocks[i]) return false;
			}
			if (diag1.CurrentPsnLogNumber != diag2.CurrentPsnLogNumber) return false;
			if (diag1.ErrorDumpTableCRC != diag2.ErrorDumpTableCRC) return false;
			if (diag1.ErrorRpdMetersTableCRC != diag2.ErrorRpdMetersTableCRC) return false;
			if (diag1.FatOffsetFromPageZero != diag2.FatOffsetFromPageZero) return false;
			if (diag1.FaultDumpsTable.Count != diag2.FaultDumpsTable.Count) return false;
			for (int i = 0; i < diag1.FaultDumpsTable.Count; ++i) {
				if (!diag1.FaultDumpsTable[i].IsEqualTo(diag2.FaultDumpsTable[i])) return false;
			}
			if (diag1.FaultLogsCount != diag2.FaultLogsCount) return false;
			if (diag1.FirmwareVersion != diag2.FirmwareVersion) return false;
			if (diag1.FirstWrittenAfterResetPageNumber != diag2.FirstWrittenAfterResetPageNumber) return false;
			if (diag1.LastReadedBlockAddress != diag2.LastReadedBlockAddress) return false;
			if (diag1.LastWrittenPageAddress != diag2.LastWrittenPageAddress) return false;
			if (diag1.LastWrittenPageNumber != diag2.LastWrittenPageNumber) return false;
			if (diag1.LostFRAM != diag2.LostFRAM) return false;
			if (diag1.LostNAND != diag2.LostNAND) return false;
			//if (diag1.PsnLogPowerUpFragmentInfos.Count != diag2.PsnLogPowerUpFragmentInfos.Count) return false;
			//for (int i = 0; i < diag1.PsnLogPowerUpFragmentInfos.Count; ++i) {
			//if (!diag1.PsnLogPowerUpFragmentInfos[i].IsEqualTo(diag2.PsnLogPowerUpFragmentInfos[i])) return false;
			//}
			//if (diag1.PsnLogPredefinedFragmentInfos.Count != diag2.PsnLogPredefinedFragmentInfos.Count) return false;
			//for (int i = 0; i < diag1.PsnLogPredefinedFragmentInfos.Count; ++i) {
			//if (!diag1.PsnLogPredefinedFragmentInfos[i].IsEqualTo(diag2.PsnLogPredefinedFragmentInfos[i])) return false;
			//}

			if (diag1.PsnLogStartPageNumber != diag2.PsnLogStartPageNumber) return false;
			if (diag1.RpdPagesCountTransmittedToPsnLog != diag2.RpdPagesCountTransmittedToPsnLog) return false;

			return true;
		}
	}
}