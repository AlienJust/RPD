using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.IO;
using AlienJust.Support.IO;
using RPD.DAL;
using RPD.DAL.DataExtraction.SimpleRelease;
using RPD.DalRelease.Shared;
using RPD.Supports;

namespace RPD.DalRelease.RPD
{
	/// <summary>
	/// Класс, описывающий аварию
	/// </summary>
	internal class FaultLog : IFaultLog
	{
		private byte _metersCount; // Чилсо измерителей, вычиатнное из заголовочной страницы
		private byte _faultNumberFromHeader;// Номер аварии из заголовка

		//private IPsnLog _internalPsnDataLog; // null при инициализации

		public string Name { get; set; }


		private readonly IDeviceConfiguration _deviceConfig;

		/// <summary>
		/// Список сигналов
		/// </summary>
		public ObservableCollection<ISignal> Signals { get; set; }

		/// <summary>
		/// Список измерителей РПД
		/// </summary>
		public ObservableCollection<IRpdMeter> RpdMeters { get; set; }

		public IPsnLog PsnFaultLog {
			get { //return _internalPsnDataLog; 
				return null;
			}
		}


		/// <summary>
		/// Произошла в
		/// </summary>
		public DateTime AccuredAt { get; set; }

		/// <summary>
		/// Секция, которой принадлежит авария
		/// </summary>
		public ISection OwnerSection { get; set; }

		/// <summary>
		/// Номер аварии
		/// </summary>
		public int Number { get; set; }

		/// <summary>
		/// Хэш файла в виде строки
		/// </summary>
		public string FileHash { get; set; }

		/// <summary>
		/// Время сохранения аварии
		/// </summary>
		public DateTime SavedAt { get; set; }

		/// <summary>
		/// Причина аварии
		/// </summary>
		public FaultReason Reason { get; private set; }

		

		/// <summary>
		/// Dev. config.
		/// </summary>
		public IDeviceConfiguration DeviceConfig {
			get { return _deviceConfig; }
		}

		/// <summary>
		/// Путь к бинарнику аварии, сначала на Flash
		/// </summary>
		public FileInfo BinFilePath { get; set; }

		/// <summary>
		/// Запомним индекс заголовочной страницы для дальнейшей быстрой навигации
		/// </summary>
		public long HeaderPageIndex { get; set; }

		/// <summary>
		/// Флаг того, что авария сохранена в репозиторий
		/// </summary>
		public bool SavedToRepository { get; set; }

		

		

		/// <summary>
		/// Длина загловка записи текущих данных по измерителю
		/// </summary>
		private const int MeterHeaderCurrentDataRecordLength = 4; // In bytese

		/// <summary>
		/// Длина записи текущих данных по каналу
		/// </summary>
		private const int ChannelCurrentDataRecordLength = 3; // In bytes

		#region .ctors

		/// <summary>
		/// Section-side конструктор
		/// </summary>
		/// <param name="owner">Секция-владелец</param>
		/// <param name="name">название аварии</param>
		public FaultLog(ISection owner, string name)
		{
			Name = name;
			Signals = new ObservableCollection<ISignal>();
			RpdMeters = new ObservableCollection<IRpdMeter>();
			OwnerSection = owner;
			AccuredAt = DateTime.Now;
			SavedAt = DateTime.Now;

			FileHash = string.Empty;
			
			Reason = new FaultReason(null, null);
		}

		/// <summary>
		/// Device-side конструктор
		/// </summary>
		/// <param name="configuration">Конфигурация устройства</param>
		/// <param name="name">Название аварии</param>
		/// <param name="number">Номер аварии</param>
		public FaultLog(IDeviceConfiguration configuration, string name, int number)
		{
			Name = name;
			Number = number;

			Signals = new ObservableCollection<ISignal>();
			RpdMeters = new ObservableCollection<IRpdMeter>();
			OwnerSection = null;
			AccuredAt = DateTime.Now;

			_deviceConfig = configuration;

			HeaderPageIndex = -1;

			Reason = new FaultReason(null, null);
		} 
		#endregion

		/// <summary>
		/// Прочитать информацию об аварии из бинарника вида AVR*.bin
		/// Переопределяет название аварии из бинарника
		/// Переопределяет время аварии
		/// Переопределяет FileHash
		/// Переопределяет BinFileInfo
		/// </summary>
		/// <param name="sourceFile">Путь к бинарнику</param>
		/// <returns>Результат операции</returns>
		public bool ReadInfoFromBinaryFile(FileInfo sourceFile)
		{
			//Log.Global.Info("Попытка чтения информации об аварии из файла: " + sourceFile.FullName);
			bool result = true;

			sourceFile.Refresh();
			if (sourceFile.Exists)
			{
				//Log.Global.Info("Файл лога аварии существует");
				using (var reader = new AdvancedBinaryReader(new FileStream(sourceFile.FullName, FileMode.Open, FileAccess.Read), false))
				{
					long pagesCount = reader.BaseStream.Length/2048;
					//Log.Global.Info("Количество страниц в файле = " + pagesCount);
					bool startPageFound = false;
					//int startPageIndex = 0;
					bool firstNonstartPageFound = false;
					for (long i = 0; i < pagesCount; ++i)
					{
						reader.BaseStream.Seek(i*2048, SeekOrigin.Begin);
						byte firstPageByte = reader.ReadByte(); // считаем первый байт
						if (firstPageByte == 0x55) // заголовок найден!
						{
							//Log.Global.Info("Заголовочная страница найдена, её номер = " + i);
							startPageFound = true;
							HeaderPageIndex = i; // запомнить индекс страницы-заголовка аварии (но в реальности страниц может быть много!)
							// следующие 6 байт - дата (dd|MM|yy|HH|mm|ss)
							byte day = reader.ReadByte();
							byte month = reader.ReadByte();
							byte year = reader.ReadByte();

							byte hour = reader.ReadByte();
							byte minute = reader.ReadByte();
							byte second = reader.ReadByte();
							try {
								AccuredAt = new DateTime(2000 + year, month, day, hour, minute, second); // соберем байты в удобный вид
							}
							catch {
								//Log.Global.Info("Не удалось распарсить дату и время аварии из байт: " + year + " " + month + " " + day + " " + hour + " " + minute + " " + second);
								AccuredAt = DateTime.MinValue;
							}
							//Log.Global.Info("Дата аварии = " + AccuredAt.ToString("yyyy.MM.dd HH:mm:ss"));
							_faultNumberFromHeader = reader.ReadByte(); // прочитаем номер аварии (байт №8)
							_metersCount = reader.ReadByte(); // прочитаем число измерителей (байт №9)
						}
						else if (firstPageByte < 0x55) // Первую незаголовочную страницу нужно прочитать для получения настроек измерителя:
						{
							if (startPageFound) firstNonstartPageFound = true;
							//Log.Global.Info("Незаголовочная страница, её индекс=" + i + " RpdMeters.Count=" + RpdMeters.Count);
							reader.BaseStream.Seek(i*2048, SeekOrigin.Begin);

							var pageRaw = new byte[2048];
							reader.Read(pageRaw, 0, 2048); //считываем страницу
							var currentPage = new FaultArchivePage(pageRaw, this);

							// Считываем настройки всех измерителей:
							for (int j = 0; j < _metersCount; ++j)
							{
								var neededMeter = (RpdMeter) RpdMeters[j];
								if (!neededMeter.SettingsReaded)
								{
									neededMeter.ReadSettings(currentPage);
								}
							}
						}


						if (startPageFound && firstNonstartPageFound) break;
					}



					if (startPageFound && firstNonstartPageFound)
					{
						//reader.BaseStream.Seek(startPageIndex * 2048, SeekOrigin.Begin);
						// После чтения информации по измерителям РПД необходимо вернуться в начало и прочитать текущие данные:
						var rpdCurrentData = new List<RpdChannelCurrentData>();
						int rpdChannelCurrentDataOffset = 9;
						// В этом цикле считываем текущие данные каналов РПД, заодно на выходе получая позицию текущих данных ПСН
						for (int i = 0; i < _metersCount; ++i)
						{
							rpdChannelCurrentDataOffset += MeterHeaderCurrentDataRecordLength;

							foreach (RpdChannel ch in RpdMeters[i].Channels)
							{
								//Log.Global.Info("Channel=" + ch.Name + ".IsEnabled = " + ch.IsEnabled);
								if (ch.IsEnabled)
								{
									rpdChannelCurrentDataOffset += ChannelCurrentDataRecordLength;
									//Log.Global.Info("rpdChannelCurrentDataOffset = " + rpdChannelCurrentDataOffset);
									// Если настройки были прочитаны:
									// Нельзя выносить на верхний уровень, т.к. тогда не правильно отработает смещение rpdChannelCurrentDataOffset
									var neededMeter = (RpdMeter) RpdMeters[i];
									if (neededMeter.Settings != null && neededMeter.SettingsReaded)
									{
										//Log.Global.Info(neededMeter.Name + " настроки были прочитаны");
										reader.BaseStream.Seek(HeaderPageIndex*2048 + rpdChannelCurrentDataOffset, SeekOrigin.Begin);

										var chT = neededMeter.Settings.Calibrations.Channels[ch.Number - 1];
										var b1 = reader.ReadByte(); // По суте первый байт не нужен, но нужно сдвинуть текущее положение в файле на 1 байт
										var b2 = reader.ReadByte();
										var b3 = reader.ReadByte();
										//Log.Global.Info("b1 = " + b1);
										//Log.Global.Info("b2 = " + b2);
										//Log.Global.Info("b3 = " + b3);
										double channelCurrentValue = b2 + b3*256.0;
										//Log.Global.Info("ChCurrValue = " + channelCurrentValue);
										double modifiedCurValue = (channelCurrentValue - chT.Zero)*chT.Kkor*chT.Kper*1.0;
										//Log.Global.Info("ResultValue = " + modifiedCurValue);
										rpdCurrentData.Add(new RpdChannelCurrentData(ch, modifiedCurValue));
									}
								}
							}
						}
						int psnCurrentDataOffset = rpdChannelCurrentDataOffset; // На самом деле № позиции текущих данных ПСН определяется так: сумма числа разрешённых каналов рпд по каждому измерителю:
						//Log.Global.Info("Смещение текущих данных магистрали ПСН: psnCurrentDataOffset=" + psnCurrentDataOffset);

						var psnReasonRaw = new byte[FaultReason.PsnGaugesCount*FaultReason.PsnReplyMaxLength];
						reader.BaseStream.Seek(HeaderPageIndex*2048 + psnCurrentDataOffset, SeekOrigin.Begin);
						reader.Read(psnReasonRaw, 0, psnReasonRaw.Length);
						// TODO: whre to get PsnChannelSimple
						//Reason = new FaultReason(rpdCurrentData, PsnPagesParser.GetPsnCurrentData(psnReasonRaw, _deviceConfig));
						reader.Close();
					}
					else
					{
						reader.Close();
						throw new Exception("Ошибка, не удалось найти начальную страницу или первую страницу с данными");
					}
					//reader.Close();


					FileHash = DTHasher.GetMD5Hash(sourceFile.FullName, string.Empty);
					BinFilePath = sourceFile;
					//this.Name = sourceFile.Name; // Имя исходного файла всегда корявое,
					UpdateName();
					//Log.Global.Info("Информация об аварии {" + Name + "} успешно прочитана");
					//Log.Global.Info(BinFilePath.FullName);
					//Log.Global.Info(Reason.ToString);
				}
			}
			else
			{
				result = false;
				//Log.Global.Info("Файл лога аварии не существует! result = " + result);
			}

			return result;
		}

		/// <summary>
		/// Прочитать тренд из бинарника
		/// </summary>
		/// <param name="channel">Канал, для которого грузится тренд</param>
		public ReadChannelTrendResult ReadChannelTrendFromBinaryFile( /*string filePath,*/ RpdChannel channel)
		{
			var result = new ReadChannelTrendResult {Result = ((RpdMeter) channel.OwnerMeter).CloneAndLinkWithFault(null)};
			// Запишем в результат измеритель (родитель для channel)
			result.Result.Channels.Clear(); //каналы тоже склонировались!!1
			result.Result.Channels.Add(channel.CloneAndLinkWithMeter(result.Result)); //добавим результирующему измерителю канал
			try {
				result.Result.Channels[0].Trend.Clear(); //очистим тренд канала, вдруг там что-то есть!
				//using (AdvancedBinaryReader reader = new AdvancedBinaryReader(new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read), false))
				using (var reader = new AdvancedBinaryReader(new FileStream(BinFilePath.FullName, FileMode.Open, FileAccess.Read), false))
				{
					// запомним количество страниц в файле
					long pagesCount = reader.BaseStream.Length/2048;

					// постраничное перемещение i
					int timePosition = 0;

					for (long i = 0; i < pagesCount; ++i) {
						try {
							var pageRaw = new byte[2048]; //ряд байтов, представляющих собой страницу
							reader.Read(pageRaw, 0, 2048); //считываем страницу
							// тут не надо проверять псн! // && pageRaw[0] != 0x80) //если это не заголовок аварии и не данные по ПСН (по идее заголовок - это >= 0x55) fixed
							if (pageRaw[0] < 0x55) {
								int meterAddrFromPage = pageRaw[0]; //номер/адрес измерителя хранится в первом байте страницы
								if (meterAddrFromPage == result.Result.Channels[0].OwnerMeter.Address) //если страница принадлежит нужному измерителю, то:
								{
									var currentPage = new FaultArchivePage(pageRaw, this); //сформируем страницу как объект
									var neededMeter = (RpdMeter) result.Result.Channels[0].OwnerMeter;

									//если настройки не считаны - считываем:
									if (!neededMeter.SettingsReaded) {
										neededMeter.ReadSettings(currentPage); // Настройки измерителя хранятся в первой послезаголовочной странице ( TODO: а если это измеритель № 10 и его настройки не влезли в первую послезаголовочную страницу?)
									}

									// В любом случае удаляем информационные строки с настройками по измерителю
									RemoveMeterSettingLines(currentPage); 
									// Зачем пытаться удалять то, чего возможно и нет? (не лучше ли будет перенести под условие if (!neededMeter.SettingsReaded) 
									// Не лучше, т.к. нужно в люом случае проверять строки на нулевой номер (вдруг в системе куча измерителей? и т.п.)
									// Можно не удалять, а работать только со строками, номер которых больше нуля (нулевые - это строки настроек измерителей)

									
									// Теперь необходимо прочесать строки и найти строки для нашего канала
									foreach (VariableLengthPageLine currentLine in currentPage.Lines) {
										if (currentLine.ChannelNumber + 1 == result.Result.Channels[0].Number) //ввбираем строки для нужного канала
										{
											foreach (int val in currentLine.Values) {
												ChannelCalibration chT = neededMeter.Settings.Calibrations.Channels[result.Result.Channels[0].Number - 1];
												//формула вычисления значения с учетом настроек измерителя: (см. ТЗ)
												var point = new DataPointSimple((val - chT.Zero)*chT.Kkor*chT.Kper*1.0, AccuredAt.AddTicks((long) (timePosition*(10000000.0/neededMeter.Settings.Time) - neededMeter.TrendsTimeOffset*10)), true, 0, null); // TODO
												result.Result.Channels[0].Trend.Add(point); // *10 is microseconds in ticks//timePosition * 25 - neededMeter.TrendsTimeOffset));
												timePosition++;
											}
											//else пропускаем, такого канала в системе нету :(
										}
									}
								}
							}
						}
						catch {
							// TODO: remove empty catch
						}
					}
					//sw.Close();//DEBUG
					reader.Close(); //ридер нужно закрыть
				}
			}
			catch // (Exception ex)
			{
				result = null;
			}

			return result;
		}

		/// <summary>
		/// Удаляет нулевые строки из списка строк страницы, потому что в них хранятся настройки измерителей
		/// </summary>
		/// <param name="currentPage">Страница, из которой нужно удалить данные</param>
		private void RemoveMeterSettingLines(FaultArchivePage currentPage)
		{
			for (int j = 0; j < currentPage.Lines.Count; j++)
			{
				if (currentPage.Lines[j].LineNumber == 0) //то есть такие строки есть только в первой странице!
				{
					currentPage.Lines.RemoveAt(j);
					j--;
				}
			}
		}

		//----------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Получить измеритель по его адресу
		/// </summary>
		/// <param name="meterAddress">Aдрес измерителя</param>
		/// <returns>Найденный измеритель или null, если измеритель с таким номером не найден</returns>
		public RpdMeter GetMeterByAddress(int meterAddress)
		{
			RpdMeter result = null;
			foreach (IRpdMeter im in RpdMeters)
			{
				var m = (RpdMeter) im;
				if (m.Address == meterAddress)
				{
					result = m;
				}
			}
			return result;
		}

		//-----------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Сохранить аварию в директорию. При этом изменяется свойство this.BinFileInfo.
		/// </summary>
		/// <param name="destinationFileName">Путь к файлу назначения</param>
		public void SaveToFile(string destinationFileName)
		{
			//Log.Global.Info("Управление получено, попытка сохранить лог аварии по пути: " + destinationFileName);
			try
			{
				BinFilePath.Refresh();
				if (BinFilePath.Exists)
				{
					//Log.Global.Info("Исходный файл: " + BinFilePath.FullName + " существует");
					//FileInfo curBinFile = new FileInfo(this.BinFileInfo);
					//DateTime now = DateTime.Now;
					//string modifiedFileName = now.Ticks.ToString("00000000000000000000") + "." + BinFilePath.Name + "." + RPD.Support.DTHasher.GetMD5Hash(BinFilePath.FullName, string.Empty);
					//string newFaultBinPath = Path.Combine(destinationDir.FullName, modifiedFileName);
					// Нужно проверить, может быть тренд по аварии уже есть в репозитории (а если есть - то ничего делать и не надобно :))
					//var newBinFile = new FileInfo(newFaultBinPath);
					//var saveToFilePath = new FileInfo(destinationFileName);

					if (!File.Exists(destinationFileName))
					{
						//Log.Global.Info("Новый файл: " + destinationFileName + " не существует, копируем...");
						File.Copy(BinFilePath.FullName, destinationFileName);
					}

					BinFilePath = new FileInfo(destinationFileName); // Путь к бинарнику изменился
					SavedAt = DateTime.Now;
					UpdateName();
					//Log.Global.Info("Лог аварии {" + Name + "} успешно сохранен в " + destinationFileName);
				}
			}
			catch 
			{
				// TODO: remove empty catch
			}
		}

		/// <summary>
		/// Обновляет имя аварии, основываясь на BinFileName, SavedAt
		/// </summary>
		private void UpdateName()
		{
			Name = "Авария " + BinFilePath.Name.Substring(BinFilePath.Name.IndexOf("AVR"), BinFilePath.Name.IndexOf(".BIN") - BinFilePath.Name.IndexOf("AVR")); // В итоге имеем имя аварии "Авария AVRXX"
			if (!BinFilePath.Name.StartsWith("AVR")) // Если имя файла не начинается с AVAR, значит в имени есть префикс даты в виде тиков:
			{
				long ticks = long.Parse(BinFilePath.Name.Substring(0, BinFilePath.Name.IndexOf(".AVR")));
				SavedAt = new DateTime(ticks);
				Name += " (считана " + SavedAt.ToShortDateString() + " " + SavedAt.ToShortTimeString() + ")";
			}
		}

		/// <summary>
		/// Получает лог аварии по хэшу файла
		/// </summary>
		/// <param name="faults">Список логов аварий, в котором будет производиться поиск</param>
		/// <param name="fileHash">Строка хэша файла</param>
		/// <returns>Найденый лог аварии, либо null</returns>
		public static FaultLog GetFaultByFileHash(IEnumerable<FaultLog> faults, string fileHash)
		{
			FaultLog result = null;

			try
			{
				foreach (FaultLog fault in faults)
				{
					if (fault.FileHash == fileHash)
					{
						result = fault;
						break;
					}
				}
			}
			catch 
			{
				// TODO: remove empty catch
			}

			return result;
		}

		/// <summary>
		/// Причина аварии в виде текста
		/// </summary>
		public string LogReason
		{
			get { return Reason.ToString(); }
		}
	}
}
