using System.Collections.Generic;

namespace RPD.DAL {
	/// <summary>
	/// ����������� ����������
	/// </summary>
	public interface IDeviceDiagnostic
	{
		/// <summary>
		/// ������ ��������
		/// </summary>
		int FirmwareVersion { get; }

		/// <summary>
		/// ��� ����� � FRAM
		/// </summary>
		bool LostFRAM { get; }

		/// <summary>
		/// ��� ����� � NAND
		/// </summary>
		bool LostNAND { get; }

		/// <summary>
		/// ������ ����������� ����� � ������� �����������
		/// </summary>
		bool ErrorRpdMetersTableCRC { get; }

		/// <summary>
		/// ������ CRC16 ������� ������ ������
		/// </summary>
		bool ErrorDumpTableCRC { get; }

		/// <summary>
		/// ����� ������ ������ �� ����������
		/// </summary>
		int FaultLogsCount { get; }

		/// <summary>
		/// ������ ������� ������ ������
		/// </summary>
		List<long> BadBlocks { get; }

		/// <summary>
		/// ���������� ������ ������ ������
		/// </summary>
		int BadBlocksCount { get; }

		/// <summary>
		/// ����� ��������� ���������� ��������
		/// </summary>
		int LastWrittenPageAddress { get; }

		/// <summary>
		/// ����� ���������� ���������� ���� 
		/// </summary>
		int LastReadedBlockAddress { get; }

		/// <summary>
		/// ����� ��������� ���������� ��������
		/// </summary>
		int LastWrittenPageNumber { get; }

		/// <summary>
		/// ����� ������ ���������� �������� ����� ������ �������
		/// </summary>
		int FirstWrittenAfterResetPageNumber { get; }

		/// <summary>
		/// ����� ��������, � ������� ���������� ��� ���
		/// </summary>
		int PsnLogStartPageNumber { get; }

		/// <summary>
		/// ����� �������� ������ ������� ������ ���
		/// </summary>
		int ArrayDumpPsnStartPageNumber { get; }

		/// <summary>
		/// �������� FAT ������������ ������ ��������
		/// </summary>
		int FatOffsetFromPageZero { get; }

		/// <summary>
		/// ����� �������, ������������ � ������� ���� ���
		/// </summary>
		int RpdPagesCountTransmittedToPsnLog { get; }

		/// <summary>
		/// ������� ����� ���
		/// </summary>
		IList<IRpdLogInfo> FaultDumpsTable { get; }

		/// <summary>
		/// ������� ����� ���� ���
		/// </summary>
		int CurrentPsnLogNumber { get; set; }

		/// <summary>
		/// ������ ���������� ����� ��� �� ���������
		/// </summary>
		IList<IPsnLogFragmentInfo> PsnLogPowerUpFragmentInfos { get; }

		/// <summary>
		/// ������ ���������� ����� ��� ������������� �������
		/// </summary>
		IList<IPsnLogFragmentInfo> PsnLogPredefinedFragmentInfos { get; }
	}
}