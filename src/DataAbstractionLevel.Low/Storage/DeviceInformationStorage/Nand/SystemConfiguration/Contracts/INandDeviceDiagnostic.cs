using System.Collections.Generic;

namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts {
	/// <summary>
	/// Диагностика устройства
	/// </summary>
	public interface INandDeviceDiagnostic {
		/// <summary>
		/// Версия прошивки
		/// </summary>
		int FirmwareVersion { get; }

		/// <summary>
		/// Нет связи с FRAM
		/// </summary>
		bool LostFRAM { get; }

		/// <summary>
		/// Нет связи с NAND
		/// </summary>
		bool LostNAND { get; }

		/// <summary>
		/// Ошибка контрольной суммы в таблице измерителей
		/// </summary>
		bool ErrorRpdMetersTableCRC { get; }

		/// <summary>
		/// Ошибка CRC16 таблицы дампов аварий
		/// </summary>
		bool ErrorDumpTableCRC { get; }

		/// <summary>
		/// Число дампов аварий на устройстве
		/// </summary>
		int FaultLogsCount { get; }

		/// <summary>
		/// Список адресов плохих блоков
		/// </summary>
		List<long> BadBlocks { get; }

		/// <summary>
		/// Количество плохих блоков данных
		/// </summary>
		int BadBlocksCount { get; }

		/// <summary>
		/// Адрес последней записанной страницы
		/// </summary>
		int LastWrittenPageAddress { get; }

		/// <summary>
		/// Адрес последнего считанного блок 
		/// </summary>
		int LastReadedBlockAddress { get; }

		/// <summary>
		/// Номер последней записанной страницы
		/// </summary>
		int LastWrittenPageNumber { get; }

		/// <summary>
		/// Номер первой записанной страницы после подачи питания
		/// </summary>
		int FirstWrittenAfterResetPageNumber { get; }

		/// <summary>
		/// Номер страницы, с которой начинается лог ПСН
		/// </summary>
		int PsnLogStartPageNumber { get; }

		/// <summary>
		/// Номер страницы начала массива дампов ПСН
		/// </summary>
		int ArrayDumpPsnStartPageNumber { get; }

		/// <summary>
		/// Смещение FAT относительно первой страницы
		/// </summary>
		int FatOffsetFromPageZero { get; }

		/// <summary>
		/// Число страниц, перемещенных в область лога ПСН
		/// </summary>
		int RpdPagesCountTransmittedToPsnLog { get; }

		/// <summary>
		/// Таблица логов РПД
		/// </summary>
		IList<IRpdDataInformation> FaultDumpsTable { get; }

		/// <summary>
		/// Текущий номер лога ПСН
		/// </summary>
		int CurrentPsnLogNumber { get; set; }

		/// <summary>
		/// Список фрагментов логов ПСН по включению
		/// </summary>
		IList<IPsnDataFragmentInformation> PsnLogPowerUpFragmentInfos { get; }

		/// <summary>
		/// Список фрагментов логов ПСН определенного размера
		/// </summary>
		IList<IPsnDataFragmentInformation> PsnLogPredefinedFragmentInfos { get; }
	}
}