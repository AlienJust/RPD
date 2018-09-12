using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using AlienJust.Support.Concurrent.Contracts;
using DataAbstractionLevel.Low.PsnConfig.Contracts;
using DataAbstractionLevel.Low.Storage.Contracts;
using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand;
using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Common;
using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts;
using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.ReleaseNand;
using RPD.DAL;
using RPD.DAL.PsnConfiguration.Contracts.Internal;
using RPD.DalRelease.Configuration.CommandConfiguration;
using RPD.DalRelease.Configuration.System;
using RPD.DalRelease.Rpd;
using RPD.DalRelease.RPD;
using RPD.EventArgs;

namespace RPD.DalRelease.Configuration {
	internal class DeviceConfiguration : IDeviceConfiguration {
		private readonly IWorker<Action> _backWorker;
		private readonly IThreadNotifier _uiNotifier;

		private readonly DeviceFlasher _flasher;

		private readonly List<ICyclePsnCmdPartInfo> _cycledPsnCmdPartInfos;
		private readonly List<IPsnChannelInfo> _psnChannelInfos;

		private ISystemConfiguration _systemConfig;
		private ICustomConfiguration _customConfig;
		private ICommandConfiguration _commandConfig;

		public ObservableCollection<IRpdMeter> RpdMeters { get; set; } // Предполагается, что работа с коллекцией будет производиться из потока пользователя

		public DeviceConfiguration(IWorker<Action> backWorker, IThreadNotifier uiNotifier) {
			_backWorker = backWorker;
			_uiNotifier = uiNotifier;
			 _flasher = new DeviceFlasher(_backWorker, _uiNotifier);

			_customConfig = new CustomConfigurationBuilderDefault().BuildConfiguration();
			_systemConfig = new SystemConfigurationBuilderDefault().BuildConfiguration();
			_commandConfig = new CommandConfigurationBuilderDefault().BuildConfiguration();

			RpdMeters = new ObservableCollection<IRpdMeter>();

			Comment = string.Empty;

			_cycledPsnCmdPartInfos = new List<ICyclePsnCmdPartInfo>();
			_psnChannelInfos = new List<IPsnChannelInfo>();
			PsnMeters = new ObservableCollection<IPsnMeter>();
		}

		public ObservableCollection<IPsnMeter> PsnMeters { get; }

		public IList<IPsnChannelInfo> PsnChannelInfos => _psnChannelInfos;

		public ObservableCollection<IPsnCommandInfo> PsnCommands { get; private set; }

		public IDeviceDiagnostic Diagnostic { get; set; }

		public DateTime? CurrentTime => _commandConfig.DeviceTime;

		public int PsnProtocolNumber {
			get => _commandConfig.PsnProtocolType;
			set => _commandConfig.PsnProtocolType = (ushort) value;
		}

		public int LocomotiveNumber {
			get => _systemConfig.LocomotiveNumber;
			set => _systemConfig.LocomotiveNumber = value;
		}

		public string LocomotiveName {
			get => _customConfig.LocomotiveName;
			set => _customConfig.LocomotiveName = value;
		}

		public int SectionNumber {
			get => _systemConfig.SectionNumber;
			set => _systemConfig.SectionNumber = value;
		}

		public string Comment { get; set; }

		#region Props. that're projected to SysConfig

		/// <summary>
		/// Адрес прибора.
		/// </summary>
		public int Address {
			get => _systemConfig.DeviceAddress;
			set => _systemConfig.DeviceAddress = (ushort) value;
		}

		/// <summary>
		/// Сетевой адрес.
		/// </summary>
		public int NetAddress {
			get => _systemConfig.NetAddress;
			set => _systemConfig.NetAddress = value;
		}

		/// <summary>
		/// Применять код Хэминга к содержимому страниц в NAND.
		/// </summary>
		public bool UseHammingForNand {
			get => ((_systemConfig.ConfigurationByte & 0x01) == 0x01);
			set {
				if (value)
					_systemConfig.ConfigurationByte = (byte) (_systemConfig.ConfigurationByte | 0x01);
					//if true, складываем биты, все с ложью (останутся неизменными), кроме первого
				else
					_systemConfig.ConfigurationByte = (byte) (_systemConfig.ConfigurationByte & 0xFE);
				//overwise if false, то есть умножаем все биты на истину, кроме первого (FE = FF - 1)
			}
		}

		/// <summary>
		/// Вести лог магистрали ПСН.
		/// </summary>
		public bool LogPsn {
			get => (_systemConfig.ConfigurationByte & 0x02) == 0x02;
			set {
				if (value) _systemConfig.ConfigurationByte = (byte) (_systemConfig.ConfigurationByte | 0x02);
				else _systemConfig.ConfigurationByte = (byte) (_systemConfig.ConfigurationByte & 0xFD);
			}
		}

		/// <summary>
		/// Сохранять межбайтовый интервал в обмене ПСН.
		/// </summary>
		public bool SaveByteInterval {
			get => ((_systemConfig.ConfigurationByte & 0x04) == 0x04);
			set {
				if (value) _systemConfig.ConfigurationByte = (byte) (_systemConfig.ConfigurationByte | 0x04);
				else _systemConfig.ConfigurationByte = (byte) (_systemConfig.ConfigurationByte & 0xFB); // FB = FF - 04 ;)
			}
		}

		/// <summary>
		/// Сбрасывать параметры в таблице аварий и освобождать память под дампы аварий, если считаны все файлы дампов аварий.
		/// </summary>
		public bool ResetFaultsDump {
			get => (_systemConfig.ConfigurationByte & 0x08) == 0x08;
			set {
				if (value) _systemConfig.ConfigurationByte = (byte) (_systemConfig.ConfigurationByte | 0x08);
				else _systemConfig.ConfigurationByte = (byte) (_systemConfig.ConfigurationByte & 0xF7); // F7 = FF - 08 ;)
			}
		}

		//---------------------------------------------------------------------------------------------------------------------

		#endregion




		public IRpdMeter CreateMeter() {
			
			return new RpdMeter(null, 1, "Новый измеритель");
		}


		/// <summary>
		/// Чтение конфигурации
		/// </summary>
		/// <param name="path">Путь к устройству</param>
		/// <param name="onComplete">Callback завершения операции</param>
		public void Read(string path, Action<OnCompleteEventArgs> onComplete) {
			_backWorker.AddWork(
				() => {
					try {
						var systemConfig = new SystemConfigurationBuilderFromBinaryFile(Path.Combine(path, "SYSCONF.BIN")).BuildConfiguration();
						ICustomConfiguration customConfig;
						try {
							customConfig = new CustomConfigurationBuilderFromRpdConfFile(Path.Combine(path, "RPDCONF.BIN")).BuildConfiguration();
						}
						catch {
							customConfig = new CustomConfigurationBuilderDefault().BuildConfiguration();
						}
						// TODO: fill class fields from readed configurations
						_systemConfig = systemConfig;
						_customConfig = customConfig;
						_commandConfig = new CommandConfigurationBuilderFromBinadyFile(Path.Combine(path, "COMMAND.BIN")).BuildConfiguration();
  
						if (onComplete != null)
							_uiNotifier.Notify(() => onComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Ok, "Конфигурация успешно прочитана")));
					}
					catch (Exception ex) {
						if (onComplete != null)
						_uiNotifier.Notify(() => onComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Error, "Не удалось считать конфигурацию, ошибка: " + ex)));
					}
				});
		}


		/// <summary>
		/// Запись конфигурации
		/// </summary>
		/// <param name="path">Путь к устройству</param>
		/// <param name="onComplete">Callback завершения операции</param>
		public void Write(string path, Action<OnCompleteEventArgs> onComplete) {
			_backWorker.AddWork(
				() => {
					try {
						WriteSync(path, true);
						if (onComplete != null)
							_uiNotifier.Notify(() => onComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Ok, "Конфигурация упешно записана")));
					}
					catch (Exception ex) {
						if (onComplete != null)
							_uiNotifier.Notify(() => onComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Error, "Не удалось записать конфиуграцию, причина:" + Environment.NewLine + ex)));
					}
				});
		}

		/// <summary>
		/// Сохранить синхронно
		/// </summary>
		/// <param name="devPath">Путь для сохранения устройства</param>
		/// <param name="updateConfigDateTime">Флаг необходимости обновления атрибута даты-времени конфигурации</param>
		public void WriteSync(string devPath, bool updateConfigDateTime) {
			// TODO: write commandConfig
			
			var customConfigWriter = new CustomConfigurationSaverToRpdConfFile(Path.Combine(devPath, "RPDCONF.BIN"));
			customConfigWriter.SaveConfiguration(_customConfig);

			string sysconfFilename = Path.Combine(devPath, "SYSCONF.BIN");

			// Перед записью синхронизируем Configuration и SysConf:
			RemapRpdChannelsDumpConditions();
			RecreateMetersAndRulesTablesForSysconfFromMeters(_systemConfig, RpdMeters); //Можно перенести в Sysconf

			//_systemConfig.PsnRegisterStatusMasks.Clear();
			for (int i = 0; i < _systemConfig.PsnRegisterStatusMasks.Length; ++i) {
				_systemConfig.PsnRegisterStatusMasks[i] = 0;
			}

			var psnFaultDefineFlags = RecreatePsnFaultDefineFlags(_psnChannelInfos); //Можно перенести в Sysconf
			for(int i = 0; i < psnFaultDefineFlags.Count; ++i) { 
				_systemConfig.PsnRegisterStatusMasks[i] = psnFaultDefineFlags[i];
			}
			var sysConfSaver = new SystemConfigurationSaverBinary(sysconfFilename, true);
			sysConfSaver.SaveConfiguration(_systemConfig);
		}


		/// <summary>
		/// Переопределяет ConnectionPointIndex для всех точек подключения каналов
		/// </summary>
		private void RemapRpdChannelsDumpConditions() {
			int nextFreeRuleIndex = 48; // Следующий свободный номер правила
			foreach (var meter in RpdMeters) {
				foreach (var channel in meter.Channels) {
					if (channel.DumpCondition.IsUsed) // Если правило используется, то:
					{
						if (channel.DumpCondition.ConnectionPointIndex > 47 || channel.DumpCondition.ConnectionPointIndex == 0) {
							channel.DumpCondition.ConnectionPointIndex = nextFreeRuleIndex;
							nextFreeRuleIndex++;
						}
					}
					else {
						channel.DumpCondition.ConnectionPointIndex = 0;
					}
				}
			}
		}

		/// <summary>
		/// Пересоздает таблицы правил дампов и измерителей в структуре SysconfStartPage
		/// </summary>
		/// <param name="sysconf">Что обновлять</param>
		/// <param name="meters">Список измерителей, на основании которого обновлять</param>
		private void RecreateMetersAndRulesTablesForSysconfFromMeters(ISystemConfiguration sysconf, ObservableCollection<IRpdMeter> meters) {
			//нужен метод для генерации таблицы правил с уникальными записями
			//следовательно у класса RpdDumpRule появляется метод bool IsEqual(RpdDumpRule otherRule)
			//int metersCount = metersFromUpdate.Count;
			sysconf.MetersCount = (byte) meters.Count; //запишем в структуру информацию о количестве измерителей)
			for (int i = 0; i < sysconf.MetersTable.Count; ++i) // вариант, если необходимо забить остатки FF-ками
			{
				if (i < meters.Count) {
					IRpdMeter meter = meters[i];
					var rpdMeterInfoBuilder = new RpdMeterSystemInformationBuilderFromRpdMeterSystem(meter);
					sysconf.MetersTable[i] = rpdMeterInfoBuilder.Build();
				}
				else {
					// нужно забить остатки таблицы FF-ками
					var rpdMeterInfoBuilder = new RpdMeterSystemInformationBuilderFromVoid();
					sysconf.MetersTable[i] = rpdMeterInfoBuilder.Build();
				}
			}

			//обновляется таблица правил
			foreach (var meter in meters) {
				foreach (var channel in meter.Channels) {
					if (channel.DumpCondition.IsUsed) {
						if (channel.DumpCondition.ConnectionPointIndex > 0) // TODO: а если ноль - значит что-то не так с RemapRpdChannelsDumpConditions
						{
							var dumpRuleBuilder = new RpdDumpRuleBuilderFromDumpCondition(channel.DumpCondition);
							sysconf.DumpRules[channel.DumpCondition.ConnectionPointIndex - 1] = dumpRuleBuilder.Build(); // -1 т.к. ConnectionPointIndex=0 используется для обозначения NotUseDumpCondition so sad =(
						}
					}
				}
			}
		}

		private IList<byte> RecreatePsnFaultDefineFlags(IList<IPsnChannelInfo> psnChannelInfos) {
			const int psnRegisterStatusMasksMax = 21;
			var result = new byte[psnRegisterStatusMasksMax];

			foreach (var psnChannelInfo in psnChannelInfos) {

				if (psnChannelInfo.CanBeFaultSign) // Если данный канал ПСН может быть определяющим аварию
				{
					if (psnChannelInfo.IsFaultSign) // И если он помечен пользователем как определяющий аварию, то:
					{
						// Запишем значение бита определенного байта:
						if (psnChannelInfo.Configuration.CommandParameterConfigurationInfo is IFaultDefinePsnParameterConfigurationInfo config) {
							result[config.RpdConfPosByte] += (byte) (0x01 << config.RpdConfPosBit);
						}
					}
				}
			}
			return result;
		}

		/// <summary>
		/// Модифицирует список РДП измерителей в соотв. с файлом SYSCONF.BIN (удаляет ненужные измерители - которых нет в SYSCONF.BIN, или добавляет при нехватке)
		/// </summary>
		private void FillRpdMetersFromSysconfAfterConfigRead() {
			var irm = new ObservableCollection<IRpdMeter>();
			for (int i = 0; i < _systemConfig.MetersCount; ++i) //для всех действительных записей об измерителях в файле SYSCONF.BIN:
			{
				//if (SysConfig.MetersTable[i].Crc == SysConfig.MetersTable[i].CalculateCrc()) //если CRC считалась правильно
				{
					bool found = false;
					foreach (var m in RpdMeters) //для каждого измерителя из RPDCONF.BIN
					{
						if (m.Address == _systemConfig.MetersTable[i].Address) //если адреса совпадают:
						{
							var rpdMeterBuilder = new RpdMeterBuilderFromRpdMeterInfo(_systemConfig.MetersTable[i], m.Name);
							RpdMeter newMeter = rpdMeterBuilder.Build();

							//создаем новый измеритель на основе записи из SYSCONF.BIN
							for (int j = 0; j < newMeter.Channels.Count; ++j) //копируем информацию о каналах:
							{
								newMeter.Channels[j].Name = m.Channels[j].Name;
								if (newMeter.Channels[j].DumpCondition.ConnectionPointIndex > 0 && newMeter.Channels[j].DumpCondition.ConnectionPointIndex < _systemConfig.DumpRules.Count) {
									var dumpConditionBuilder = new DumpConditionBuilderFromRpdDumpRule(_systemConfig.DumpRules[newMeter.Channels[j].DumpCondition.ConnectionPointIndex - 1], newMeter.Channels[j].DumpCondition.ConnectionPointIndex);
									newMeter.Channels[j].DumpCondition.CopyFrom(dumpConditionBuilder.Build());
									//SysConfig.DumpRules[newMeter.Channels[j].DumpCondition.ConnectionPointIndex - 1].UpdateChannelCondition(newMeter.Channels[j].DumpCondition); //нулевое праило не хранится в массиве правил
								}
							}
							irm.Add(newMeter);
							found = true;
							break;
						}
					}
					if (!found) //если в RPDCONF.BIN информации об измертеле не было
					{
						var rpdMeterBuilder = new RpdMeterBuilderFromRpdMeterInfo(_systemConfig.MetersTable[i], string.Empty);
						RpdMeter newm = rpdMeterBuilder.Build();

						newm.Name = "Измеритель (адр.=" + newm.Address + ")";
						for (int j = 0; j < newm.Channels.Count; j++) //копируем информацию о каналах:
						{
							newm.Channels[j].Name = "Канал U" + j;
							if (newm.Channels[j].DumpCondition.ConnectionPointIndex > 0) {
								var dumpConditionBuilder = new DumpConditionBuilderFromRpdDumpRule(_systemConfig.DumpRules[newm.Channels[j].DumpCondition.ConnectionPointIndex - 1], newm.Channels[j].DumpCondition.ConnectionPointIndex);
								newm.Channels[j].DumpCondition.CopyFrom(dumpConditionBuilder.Build());
								//SysConfig.DumpRules[newm.Channels[j].DumpCondition.ConnectionPointIndex - 1].UpdateChannelCondition(
								//newm.Channels[j].DumpCondition); //нулевое праило не хранится в массиве правил
							}
						}
						irm.Add(newm);
					}
				}
			}
			RpdMeters = irm;
		}

		/// <summary>
		/// Производит сквозной поиск ПСН канала в списке измерителей ПСН данной конфигурации
		/// </summary>
		/// <param name="rpdConfPosByte">Параметр поиска - № байта в конфигурации РПД</param>
		/// <param name="rpdConfPosBit">Параметр поиска - № бита в конфигурации РПД</param>
		/// <returns>Найденный ПСН канал or null</returns>
		private IPsnChannelInfo GetPsnChannelByRpdConfPosition(int rpdConfPosByte, int rpdConfPosBit) {
			IPsnChannelInfo result = null;
			foreach (var info in _psnChannelInfos) {

				if (info.CanBeFaultSign) //если данный канал ПСН может быть определяющим авраию
				{
					var config = info.Configuration.CommandParameterConfigurationInfo as IFaultDefinePsnParameterConfigurationInfo;

					if (config != null && config.RpdConfPosByte == rpdConfPosByte && config.RpdConfPosBit == rpdConfPosBit) {
						result = info;
						break;
					}
				}

			}
			return result;
		}


		/// <summary>
		/// Связывает аварию и список копий измерителей РПД из данной конфигурации
		/// </summary>
		/// <param name="linkedFaultLog">Авария для связывания</param>
		public void LinkCopiesOfRpdMetersToFault(FaultLog linkedFaultLog) {
			//Log.Global.Info("Called");
			try {
				//Log.Global.Info("RpdMeters.Count = " + RpdMeters.Count);

				if (linkedFaultLog != null) {
					linkedFaultLog.RpdMeters = new ObservableCollection<IRpdMeter>();
					foreach (IRpdMeter meter in RpdMeters) {
						var m = (RpdMeter) meter;
						linkedFaultLog.RpdMeters.Add(m.CloneAndLinkWithFault(linkedFaultLog));
					}
					//Log.Global.Info("linkedFaultLog.RpdMeters.Count = " + linkedFaultLog.RpdMeters.Count);
				}
			}
			catch  {
				// TODO: remove empty catch
			}
		}

		/// <summary>
		/// "Очистка архивов" - при вызове этого метода производится сброс счетчика аварий и выдача служебной команды для РПД, файлы архивов на самом деле не трогаются
		/// </summary>
		/// <param name="driveLetter">Буква диска, которой определилось устройство в системе</param>
		public void ClearArchives(string driveLetter) {
			_flasher.ClearArchives(driveLetter, null);
		}

		public void ClearArchivesAsync(string driveLetter, Action<OnCompleteEventArgs> onComplete) {
			_flasher.ClearArchives(driveLetter, onComplete);
		}

		public void FormatRpdAsync(string driveLetter, Action<OnCompleteEventArgs> onComplete) {
			_flasher.FormatRpd(driveLetter, onComplete);
		}

		public void TestLinkWithMetersAsync(string driveLetter, Action<OnCompleteEventArgs> onComplete) {
			_flasher.TestLinkWithMeters(driveLetter, onComplete);
		}



		public void WriteFirmware(string firmwireHexFilename, string deviceDriveLetter, Action<OnCompleteEventArgs> onComplete) {
			_flasher.WriteFirmware(firmwireHexFilename, deviceDriveLetter, onComplete);
		}

		public void SetTime(DateTime time, string deviceDriveLetter, Action<OnCompleteEventArgs> onComplete) {
			_flasher.SetTime(deviceDriveLetter, onComplete, time);
		}

		public IList<ICyclePsnCmdPartInfo> CycledCommandPartInfos => _cycledPsnCmdPartInfos;


		public override string ToString() {
			string result = string.Empty;
			result += "Название локомотива: " + LocomotiveName + Environment.NewLine;
			result += "Адрес: " + Address + Environment.NewLine;
			result += "Сетевой адрес: " + NetAddress + Environment.NewLine;
			result += "Число измерителей: " + RpdMeters.Count + Environment.NewLine;
			result += "Число сбойных блоков: " + _systemConfig.BadBlocksCount + Environment.NewLine;
			result += "Версия прошивки: " + _systemConfig.FirmwareVersion + Environment.NewLine;
			result += "Число аварий: " + _systemConfig.FaultsCount + Environment.NewLine;
			result += "Время внутренних часов РПД на момент подключения блока к компьютеру: " + (CurrentTime.HasValue ? CurrentTime.Value.ToString("yyyy.MM.dd HH:mm:ss") : "неизвестно") + Environment.NewLine;
			result += "Номер локомотива: " + LocomotiveNumber + Environment.NewLine;
			result += "Номер секции: " + SectionNumber + Environment.NewLine;
			result += "Номер протокола магистрали ПСН: " + PsnProtocolNumber;
			return result;
		}
	}
}
