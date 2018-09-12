using System.Collections.Generic;
using RPD.DAL;
using RPD.DalRelease.Configuration;
using RPD.DalRelease.Configuration.System.Contracts;

namespace RPD.DalRelease {
	/* TODO: restore funtionality (IDeviceDiagnostic)*/
	/*internal class DeviceDiagnostic : IDeviceDiagnostic {
		private readonly IDeviceConfiguration _configBase;

		public DeviceDiagnostic(IDeviceConfiguration basedConfig) {
			_configBase = basedConfig;
			BadBlocks = new List<long>(); //что-то я хз, где брать адреса плохих блоков

			//page65 ><
			LostFRAM = false;
			LostNAND = false;
			ErrorDumpTableCRC = false;
			ErrorRpdMetersTableCRC = false; //for example :)

			//FaultLogsCount = 10;//хз

			//хз:
			//BadBlocks.Add(123);
			//BadBlocks.Add(456);
			//BadBlocks.Add(789);
		}

		/// <summary>
		/// Версия прошивки
		/// </summary>
		public int FirmwareVersion {
			get { return _configBase.D.FirmwareVersion; }
		}

		/// <summary>
		/// Число дампов аварий на устройстве
		/// </summary>
		public int FaultLogsCount {
			get { return _configBase.SysConfig.FaultsCount; }
		}

		/// <summary>
		/// Список адресов плохих блоков
		/// </summary>
		public List<long> BadBlocks { get; set; }

		public int BadBlocksCount { get; private set; }
		public int LastWrittenPageAddress { get; private set; }
		public int LastReadedBlockAddress { get; private set; }
		public int LastWrittenPageNumber { get; private set; }
		public int FirstWrittenAfterResetPageNumber { get; private set; }
		public int PsnLogStartPageNumber { get; private set; }
		public int ArrayDumpPsnStartPageNumber { get; private set; }
		public int FatOffsetFromPageZero { get; private set; }
		public int RpdPagesCountTransmittedToPsnLog { get; private set; }
		public IList<IRpdLogInfo> FaultDumpsTable { get; private set; }
		public int CurrentPsnLogNumber { get; set; }
		public IList<IPsnLogFragmentInfo> PsnLogPowerUpFragmentInfos { get; private set; }
		public IList<IPsnLogFragmentInfo> PsnLogPredefinedFragmentInfos { get; private set; }

		#region Свойства из 65 страницы (смещение страниц 524287), которая не проецируется в файловую систему

		/// <summary>
		/// Нет связи с FRAM
		/// </summary>
		public bool LostFRAM { get; set; }

		//это хранится в 65 страние

		/// <summary>
		/// Нет связи с NAND
		/// </summary>
		public bool LostNAND { get; set; }

		//это хранится в 65 страние (смещение страниц 524287)

		/// <summary>
		/// Ошибка контрольной суммы в таблице измерителей
		/// </summary>
		public bool ErrorRpdMetersTableCRC { get; set; }

		//это хранится в 65 страние (смещение страниц 524287)

		/// <summary>
		/// Ошибка CRC16 таблицы дампов аварий
		/// </summary>
		public bool ErrorDumpTableCRC { get; set; }

		//это хранится в 65 страние (смещение страниц 524287)

		#endregion
	}*/
}
