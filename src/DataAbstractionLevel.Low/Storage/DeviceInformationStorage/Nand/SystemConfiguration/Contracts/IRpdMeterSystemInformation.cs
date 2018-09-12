using System.Collections.Generic;

namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts
{
	public interface IRpdMeterSystemInformation
	{
		int Address { get; set; }

		/// <summary>
		/// Тип измерителя (0 - Уран, 1 - ИРВИ, 2 - Радар, 0xFF - нечто недостоверное) (1)
		/// </summary>
		int Type { get; set; }

		/// <summary>
		/// Статус (2)
		/// </summary>
		int Status { get; set; }

		/// <summary>
		/// Счетчик ошибок связи (CRC, TO) (3)
		/// </summary>
		int LinkErrorCounter { get; set; }

		/// <summary>
		/// Маска каналов (всего 16 => 2 байта) (4-5)
		/// </summary>
		int ChannelsMask { get; set; }

		/// <summary>
		/// Маска каналов, из измерителя (всего 16 => 2 байта) (6-7)
		/// </summary>
		int ChannelsMaskFromMeter { get; set; }

		/// <summary>
		/// Число каналов (8)
		/// </summary>
		int ChannelsCount { get; set; }

		/// <summary>
		/// Число каналов (9)
		/// </summary>
		int ChannelsDumpedCount { get; set; }

		/// <summary>
		/// Массив из 16 (на каждый канал) кодов условий дампа каналов (от 1 до 47, 0 - нет условия) (10-25)
		/// </summary>
		IList<byte> ChannelsDumpRulesCodes { get; set; }//PsnPageHeaderLength = 16 allways!

		/// <summary>
		/// Номер вычитываемой аварии (до данного номера считаем, что все считанно) (26)
		/// </summary>
		int HigherReadedFaultNumber { get; set; }

		/// <summary>
		/// Число вычитанных аварий (27)
		/// </summary>
		int ReadedFaultsCount { get; set; }

		/// <summary>
		/// Количество одновременно хранимых дампов на измеритель (28)
		/// </summary>
		int NumberOfFaultDumpsForMeter { get; set; }

		/// <summary>
		/// Строка (29-32)
		/// </summary>
		uint PageLine { get; set; }

		/// <summary>
		/// Количество строк на аварию (33-36)
		/// </summary>
		uint PageLinesCountPerFault { get; set; }

		/// <summary>
		/// Контрльная сумма (37)
		/// </summary>
		int Crc { get; set; }
	}
}
