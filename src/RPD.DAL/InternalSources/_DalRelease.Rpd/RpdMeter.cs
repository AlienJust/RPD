using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using RPD.DalRelease.RPD;

namespace RPD.DAL
{
	/// <summary>
	/// Класс, описывающий измеритель (ИРВИ, УРАН и т.п.)
	/// </summary>
	internal class RpdMeter : IRpdMeter //, ICloneable
	{
		private const bool DumpSettingsToFile = true; //отладочный флаг
		private RpdMeterType _meterType;

		#region properies

		public string Name { get; set; }

		public int Address { get; set; }

		public ObservableCollection<IRpdChannel> Channels { get; set; }

		public IFaultLog OwnerFault { get; set; }

		public List<FaultArchivePage> ArchivePages { get; set; }

		/// <summary>
		/// Строки, содержащие настройки измерителя
		/// </summary>
		public List<VariableLengthPageLine> SettingsLines { get; set; }

		/// <summary>
		/// Массив байт для формирования TRegSetupRPD
		/// </summary>
		public byte[] SettingsRaw { get; set; }

		/// <summary>
		/// Структура настроек измерителя
		/// </summary>
		public RegSetupRpd Settings { get; set; }

		/// <summary>
		/// Признак получения информации о таймингах
		/// </summary>
		public bool SettingsReaded { get; set; }

		/// <summary>
		/// Время в мкс между последней выборкой и приходом команды "дамп памяти"
		/// </summary>
		public int TrendsTimeOffset { get; set; }

		/// <summary>
		/// Количество выборок до команды "дамп памяти"
		/// </summary>
		public int TrendDataCountBeforeDump { get; set; }

		/// <summary>
		/// Количество выборок после команды "дамп памяти"
		/// </summary>
		public int TrendDataCountAfterDump { get; set; }


		public RpdMeterType Type
		{
			get { return _meterType; }
			set
			{
				_meterType = value;
				RegenerateChannelsForType(value);
			}
		}

		#endregion properies

		//****************************************************************************
		public RpdMeter(FaultLog owner, int address, string name)
		{
			OwnerFault = owner;
			Address = address;
			Name = name;
			//--------------------------------------------------+
			//this.Channels = new ObservableCollection<IChannel>();
			Type = RpdMeterType.Undefined; //каналы регенерируются при изменении свойства, например
			//--------------------------------------------------
			ArchivePages = new List<FaultArchivePage>();
			//--------------------------------------------------+
			TrendsTimeOffset = 0;
			TrendDataCountBeforeDump = 0;
			TrendDataCountAfterDump = 0;
			//---------------------------------------------------
			SettingsReaded = false;
			//---------------------------------------------------
			SettingsLines = new List<VariableLengthPageLine>();
			SettingsRaw = new byte[RegSetupRpd.SizeInBytes];
		}

		public RpdMeter CloneAndLinkWithFault(FaultLog f)
		{
			//Log.Global.Info("Called");
			RpdMeter met = null;
			try
			{
				met = new RpdMeter(f, Address, Name) {Channels = new ObservableCollection<IRpdChannel>(), Type = Type};
				// дублирование регенерации каналов
				met.Channels.Clear();
				foreach (RpdChannel ch in Channels)
				{
					met.Channels.Add(ch.CloneAndLinkWithMeter(met));
				}
			}
			catch 
			{
				// TODO: remove empty catch
			}
			return met;
		}

		public void CopyThisSettingsToAnotherMeter(RpdMeter anotherMeter)
		{
			anotherMeter.SettingsReaded = SettingsReaded;
			anotherMeter.SettingsRaw = SettingsRaw;
			anotherMeter.SettingsLines = SettingsLines;
			anotherMeter.Settings = Settings;
		}

		private void RegenerateChannelsForType(RpdMeterType mType)
		{
			Channels = new ObservableCollection<IRpdChannel>();
			int maxChannels = 0;
			if (mType == RpdMeterType.Uran)
			{
				maxChannels = 16;
			}
			if (mType == RpdMeterType.Irvi)
			{
				maxChannels = 2;
			}
			//---------------------------
			for (int i = 0; i < maxChannels; i++)
				Channels.Add(new RpdChannel(this, i + 1, "Канал №" + (i + 1).ToString(CultureInfo.InvariantCulture), true, false, TrendType.Analogue)); // Все каналы РПД записывают аналоговю информацию

			//теперь установим принудительно разрешенные и запрещенные каналы:
			if (mType == RpdMeterType.Uran) {
				Channels[7].IsEnabled = false;
				((RpdChannel) Channels[7]).IsService = true;
				Channels[7].Name += " - Служебный";

				Channels[15].IsEnabled = false;
				((RpdChannel) Channels[15]).IsService = true;
				Channels[15].Name += " - Служебный";
			}
		}

		

		/// <summary>
		/// Считать настройки измерителя из первой страницы (первая строка + заполнение спец. струкутр из оставшихся нулевых строк)
		/// </summary>
		/// <param name="firstPage">Первая послезаголовочная страница</param>
		public void ReadSettings(FaultArchivePage firstPage)
		{
			//Log.Global.Info("Called");
			try
			{
				if (firstPage == null) throw new Exception("Первая страница аварии не передана в метод!");
				string dumpLog = "";
				if (DumpSettingsToFile)
				{
					dumpLog += DateTime.Now.ToString("yyyy.MM.dd - HH:mm.ss") + Environment.NewLine;
					dumpLog += "Считываем первую послезаголовочную страницу для измерителя с адресом № " + Address + Environment.NewLine;
					dumpLog += "Выбираем все нулевые строки (СТРОКА0):" + Environment.NewLine;
				}


				TrendsTimeOffset = firstPage.Lines[0].LineRaw[12] + 256*firstPage.Lines[0].LineRaw[13];
				TrendDataCountBeforeDump = firstPage.Lines[0].LineRaw[14] + 256*firstPage.Lines[0].LineRaw[15] + 256*256*firstPage.Lines[0].LineRaw[16];
				TrendDataCountAfterDump = firstPage.Lines[0].LineRaw[17] + 256*firstPage.Lines[0].LineRaw[18] + 256*256*firstPage.Lines[0].LineRaw[19];

				//const int lineBytesCount = 21;
				SettingsLines = new List<VariableLengthPageLine>();
				#region debug
				for (int i = 1; i < firstPage.Lines.Count; ++i)
				{
					if (firstPage.Lines[i].LineNumber == 0 && firstPage.Lines[i].ChannelNumber != 0) // Номер строки = ноль и номер канала не ноль
					{
						if (DumpSettingsToFile)
						{
							dumpLog += "Порядковый номер строки=" + i.ToString("d4") + "   содержимое:";
							for (int j = 0; j < firstPage.Lines[i].LineRaw.Length; ++j)
							{
								dumpLog += " " + firstPage.Lines[i].LineRaw[j].ToString("x2").ToUpper();
							}
							dumpLog += Environment.NewLine;
						}

						SettingsLines.Add(firstPage.Lines[i]);
					}
				} 
				#endregion
				//Log.Global.Info("SettingsLines.Count=" + SettingsLines.Count);
				//SettingsLines.Sort(SortLinesByChannelNumberASC);
				SettingsLines = SettingsLines.OrderBy(s => s.ChannelNumber).ToList();
				//Log.Global.Info("After sorting SettingsLines.Count=" + SettingsLines.Count);
				for (int i = 0; i < SettingsLines.Count; ++i) {
					for (int j = VariableLengthPageLine.LineHeaderBytesCount; j < SettingsLines[i].LineRaw.Length; ++j)
					{
						var index = i * SettingsLines[i].LineRaw.Length + j - VariableLengthPageLine.LineHeaderBytesCount;
						if (index >= RegSetupRpd.SizeInBytes) break; // Читаем строки, пока не вышли за размер структуры
						SettingsRaw[index] = SettingsLines[i].LineRaw[j];
					}
				}
				Settings = new RegSetupRpd();
				Settings.FillSetupStructFromRaw(SettingsRaw);
				SettingsReaded = true;
				//Log.Global.Info("SettingsRaw.Length=" + SettingsRaw.Length);
				if (DumpSettingsToFile)
				{
					//ниже расположен отладочный код дампа настроек в текстовый файл
					dumpLog += Environment.NewLine + "После упорядочивания строк и отсечения их заголовков получаем 608 байт: " + Environment.NewLine;
					const int bytesInLine = 16;
					for (int i = 0; i < RegSetupRpd.SizeInBytes / bytesInLine; ++i)
					{
						for (int j = 0; j < 16; j++)
						{
							dumpLog += SettingsRaw[j + i * bytesInLine].ToString("x2").ToUpper() + " ";
						}
						dumpLog += Environment.NewLine;
					}
					dumpLog += Environment.NewLine + "Из этих " + RegSetupRpd.SizeInBytes + " байт извлекаем следующую структуру настроек измерителя:" + Environment.NewLine + Environment.NewLine + Settings.ToString(false, true);


					var sw = new StreamWriter("RpdMeter.ReadSettings." + Address + ".txt", false); //DEBUG
					sw.WriteLine(dumpLog);
					sw.Close();
				}
				//-------------------------------------------------------------------
			}
			catch 
			{
				//Log.Global.Info(ex);
				SettingsReaded = false;
			}
		}

		//private int SortLinesByChannelNumberASC(VariableLengthPageLine x, VariableLengthPageLine y) //вспомогательный метод для удобной сортировки списка SettingsLines в порядке возрастания номера канала
		//{
			//if (x.ChannelNumber > y.ChannelNumber) return 1;
			//if (x.ChannelNumber < y.ChannelNumber) return -1;
			//return 0;
		//}
	}
}