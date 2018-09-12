using System;
using System.IO;
using AlienJust.Support.IO;
using RPD.DAL;

namespace RPD.DalRelease.Configuration.System {
	/// <summary>
	/// Репрезентс одну запись из таблицы измерителей (binary size = 38 bytes)
	/// </summary>
	internal class RpdMeterTableRecord {
		#region props.

		/// <summary>
		/// Адрес с первого (0)
		/// </summary>
		public byte Address { get; set; }

		/// <summary>
		/// Тип измерителя (0 - Уран, 1 - ИРВИ, 2 - Радар, 0xFF - нечто недостоверное) (1)
		/// </summary>
		public byte Type { get; set; }

		/// <summary>
		/// Статус (2)
		/// </summary>
		public byte Status { get; set; }

		/// <summary>
		/// Счетчик ошибок связи (CRC, TO) (3)
		/// </summary>
		public byte LinkErrorCounter { get; set; }

		/// <summary>
		/// Маска каналов (всего 16 => 2 байта) (4-5)
		/// </summary>
		public ushort ChannelsMask { get; set; }

		/// <summary>
		/// Маска каналов, из измерителя (всего 16 => 2 байта) (6-7)
		/// </summary>
		public ushort ChannelsMaskFromMeter { get; set; }

		/// <summary>
		/// Число каналов (8)
		/// </summary>
		public byte ChannelsCount { get; set; }

		/// <summary>
		/// Число каналов (9)
		/// </summary>
		public byte ChannelsDumpedCount { get; set; }

		/// <summary>
		/// Массив из 16 (на каждый канал) кодов условий дампа каналов (от 1 до 47, 0 - нет условия) (10-25)
		/// </summary>
		public byte[] ChannelsDumpRulesCodes { get; set; } //PsnPageHeaderLength = 16 allways!
		public const int MaxChannels = 16;

		/// <summary>
		/// Номер вычитываемой аварии (до данного номера считаем, что все считанно) (26)
		/// </summary>
		public byte HigherReadedFaultNumber { get; set; }

		/// <summary>
		/// Число вычитанных аварий (27)
		/// </summary>
		public byte ReadedFaultsCount { get; set; }

		/// <summary>
		/// Количество одновременно хранимых дампов на измеритель (28)
		/// </summary>
		public byte NumberOfFaultDumpsForMeter { get; set; }

		/// <summary>
		/// Строка (29-32)
		/// </summary>
		public uint PageLine { get; set; }

		/// <summary>
		/// Количество строк на аварию (33-36)
		/// </summary>
		public uint PageLinesCountPerFault { get; set; }

		/// <summary>
		/// Контрльная сумма (37)
		/// </summary>
		public byte Crc { get; set; }

		#endregion

		/// <summary>
		/// Конструктор по умолчанию записывает все байты 0xFF-ками
		/// </summary>
		public RpdMeterTableRecord() {
			Address = 0xFF;
			Type = 0xFF;
			Status = 0xFF;
			LinkErrorCounter = 0xFF;

			ChannelsMask = 0xFFFF;
			ChannelsMaskFromMeter = 0xFFFF;
			ChannelsCount = 0xFF;
			ChannelsDumpedCount = 0xFF;

			ChannelsDumpRulesCodes = new byte[MaxChannels];
			for (int i = 0; i < MaxChannels; i++)
				ChannelsDumpRulesCodes[i] = 0xFF;

			HigherReadedFaultNumber = 0xFF;
			ReadedFaultsCount = 0xFF;
			NumberOfFaultDumpsForMeter = 0xFF;
			PageLine = 0xFFFFFFFF;
			PageLinesCountPerFault = 0xFFFFFFFF;
			Crc = 0;
		}

		public RpdMeterTableRecord(IRpdMeter meter) {
			Address = (byte) meter.Address;
			switch (meter.Type) {
				case RpdMeterType.Uran:
					Type = 0;
					break;
				case RpdMeterType.Irvi:
					Type = 1;
					break;
				case RpdMeterType.Radar:
					Type = 2;
					break;
			}
			Status = 0;
			LinkErrorCounter = 0;
			ChannelsCount = 0;
			ChannelsMask = 0;
			ChannelsMaskFromMeter = 0;

			ChannelsDumpRulesCodes = new byte[MaxChannels];

			for (int i = 0; i < meter.Channels.Count; i++) //в этом цикле считаем маску разрешенных каналов и номера правил из таблицы правил регистрации и контроля
			{
				if (meter.Channels[i].IsEnabled) {
					ChannelsCount++; //как быть со служебными каналами?
					ChannelsMask += (ushort) (0x01 << i); // Math.Pow(2, i); //если канал разрешен - выжигаем единицу в маске :)
				}
				//выставляем номер условия канала, если оно используются для разрешенного канала:
				if (meter.Channels[i].IsEnabled) {
					if (meter.Channels[i].DumpCondition.IsUsed /* && */)
						ChannelsDumpRulesCodes[i] = (byte) (meter.Channels[i].DumpCondition.ConnectionPointIndex);
					else
						ChannelsDumpRulesCodes[i] = 0; //а если не используется - пишем 0
				}
				else {
					ChannelsDumpRulesCodes[i] = 0;
				}
			}
			ChannelsDumpedCount = ChannelsCount;

			HigherReadedFaultNumber = 0;
			ReadedFaultsCount = 0;
			NumberOfFaultDumpsForMeter = 0;
			PageLine = 0;
			PageLinesCountPerFault = 0;
			Crc = 0;
		}

		public void ReadFromStream(Stream stream) {
			var br = new AdvancedBinaryReader(stream, false);

			Address = br.ReadByte();
			Type = br.ReadByte();
			Status = br.ReadByte();
			LinkErrorCounter = br.ReadByte();

			ChannelsMask = br.ReadUInt16();
			ChannelsMaskFromMeter = br.ReadUInt16();
			ChannelsCount = br.ReadByte();
			ChannelsDumpedCount = br.ReadByte();
			br.Read(ChannelsDumpRulesCodes, 0, 16);

			HigherReadedFaultNumber = br.ReadByte();
			ReadedFaultsCount = br.ReadByte();
			NumberOfFaultDumpsForMeter = br.ReadByte();
			PageLine = br.ReadUInt32();
			PageLinesCountPerFault = br.ReadUInt32();
			Crc = br.ReadByte();
		}

		public void WriteToStream(Stream stream) {
			var bw = new AdvancedBinaryWriter(stream, false);
			bw.Write(Address);
			bw.Write(Type);
			bw.Write(Status);
			bw.Write(LinkErrorCounter);

			bw.Write(ChannelsMask);
			bw.Write(ChannelsMaskFromMeter); //как быть? поидее не нужно переписывать
			bw.Write(ChannelsCount);
			bw.Write(ChannelsDumpedCount);
			bw.Write(ChannelsDumpRulesCodes, 0, 16);

			bw.Write(HigherReadedFaultNumber);
			bw.Write(ReadedFaultsCount);
			bw.Write(NumberOfFaultDumpsForMeter);
			bw.Write(PageLine);
			bw.Write(PageLinesCountPerFault);
			bw.Write(CalculateCrc()); //перевычисляем КС по имеющимся данным
		}

		public byte CalculateCrc() {
			//int crc1 = ((Address + Type + (ChannelsMask & 0x00FF) + ((ChannelsMask & 0xFF00) >> 8) + ChannelsCount) & 0x000000FF);
			int crc1 = ((Address + Type + (ChannelsMask & 0x00FF) + ((ChannelsMask & 0xFF00) >> 8) + ChannelsDumpedCount) & 0x000000FF);
			byte crc2 = (byte) ~crc1;
			//var result = (byte) crc;
			return crc2;
		}

		public RpdMeter GenerateRpdMeter(string name) {
			//Log.Global.Info("GenerateRpdMeter > " + name);
			var meter = new RpdMeter(null, Address, name);
			switch (Type) //при изменении типа, класс RpdMeter автоматически генерирует новый список каналов с определенными DumpConditions (ConnectionPointIndex = 0)
			{
				case 0:
					meter.Type = RpdMeterType.Uran;
					break;
				case 1:
					meter.Type = RpdMeterType.Irvi;
					break;
				case 2:
					meter.Type = RpdMeterType.Radar;
					break;
			}
			for (int i = 0; i < meter.Channels.Count; ++i) {
				meter.Channels[i].IsEnabled = ((ChannelsMask & (0x01 << i)) != 0x00); // По маске каналов определяется разрешён ли канал
				//Log.Global.Info("Channel[" + i + "].IsEnabled=" + meter.Channels[i].IsEnabled);
				//meter.Channels[i].IsEnabled = (( ChannelsMask & (0x01 << i)) == (0x01 << i));
				if ( /*meter.Channels[i].IsEnabled && */ ChannelsDumpRulesCodes[i] != 0) {
					meter.Channels[i].DumpCondition.IsUsed = true;
					meter.Channels[i].DumpCondition.ConnectionPointIndex = ChannelsDumpRulesCodes[i];
				}
			}
			return meter;
		}

		public override string ToString() {
			string result;
			try {
				result =
					"Address=" + Address + Environment.NewLine +
					"Type=" + Type + Environment.NewLine +
					"Status=" + Status + Environment.NewLine +
					"LinkErrorCounter=" + LinkErrorCounter + Environment.NewLine +

					"ChannelsMask=" + ChannelsMask + Environment.NewLine +
					"ChannelsMaskFromMeter=" + ChannelsMaskFromMeter + Environment.NewLine +
					"ChannelsCount=" + ChannelsCount + Environment.NewLine +
					"ChannelsDumpedCount=" + ChannelsDumpedCount + Environment.NewLine;
				for (int i = 0; i < MaxChannels; i++)
					result += "ChannelsDumpRulesCodes[" + i + "]=" + ChannelsDumpRulesCodes[i] + Environment.NewLine;
				result +=
					"HigherReadedFaultNumber=" + HigherReadedFaultNumber + Environment.NewLine +
					"ReadedFaultsCount=" + ReadedFaultsCount + Environment.NewLine +
					"NumberOfFaultDumpsForMeter=" + NumberOfFaultDumpsForMeter + Environment.NewLine +
					"VariableLengthPageLine=" + PageLine + Environment.NewLine +
					"PageLinesCountPerFault=" + PageLinesCountPerFault + Environment.NewLine +
					"Crc=" + Crc;
			}
			catch (Exception ex) {
				result = ex.ToString();
			}
			return result;
		}
	}
}