using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AlienJust.Support.Concurrent.Contracts;
using AlienJust.Support.Identy;
using DataAbstractionLevel.Low.PsnConfig.Contracts;
using DataAbstractionLevel.Low.Storage.Contracts;
using DataAbstractionLevel.Low.Storage.PsnDataCustomConfigStorage;
using DataAbstractionLevel.Low.Storage.StreamableData.SingleFile;
using RPD.DAL;

namespace DebugApp {
	class LolTn : IThreadNotifier {
		public void Notify(Action notifyAction) {
			notifyAction();
		}
	}
	internal class Program {
		private static readonly object LogSync = new object();
		private static readonly AutoResetEvent ExitSignal = new AutoResetEvent(false);

		private static void Main(string[] args) {
			var signal = new ManualResetEvent(false);

			ILoader loader = new Loader(new LolTn());

			var rpdCfg = loader.CreateDeviceConfiguration();

			rpdCfg.Read("G:\\", ea => {
				Log(ea.Message);
				signal.Set();
			});
			signal.WaitOne();
			Log(rpdCfg);
			var repo = loader.GetNandFlashRepository("G:\\", loader.AvailablePsnConfigruations.First());
			repo.Open(ea =>
			{
				foreach (var loc in repo.Locomotives)
				{
					foreach (var locSection in loc.Sections)
					{
						foreach (var locSectionPsn in locSection.Psns)
						{
							Console.WriteLine("PsnLog");
							Console.WriteLine(locSectionPsn);
						}
					}
				}
			}, Console.Write);
			//TestPsnCustomConfigsStorage("D:\\psn.custom.xml");

			//var availablePsnConfigurations = loader.AvailablePsnConfigruations.ToList().OrderBy(c=>int.Parse(c.RpdId));

			//foreach (var psnCfg in availablePsnConfigurations) {
			//Log(psnCfg.Information.RpdId.PadLeft(3, '0') + ": " + psnCfg.Information.Name + ", v." + psnCfg.Information.Version + " (" + psnCfg.Information.Description + ")");
			//}


			////GetAllFtpRepos(loader, availablePsnConfigurations, "212.220.187.50", 21, "1", "1");
			//var devRepo = loader.GetNandFlashRepository("E:\\_RPD\\Альстом_есть_ли_записи", loader.AvailablePsnConfigruations.First(c => c.Name == "Альстом"));
			//OpenRepository(devRepo);

			//var localRepo = loader.GetLocalDirectoryRepository("C:\\Users\\aj01\\RPD.repository");
			//OpenRepository(localRepo);
			//localRepo.Remove();
			//DeleteAllLogsFromRepo(localRepo);

			//var remoteRepo = loader.GetNandFlashRepository("F:\\", availablePsnConfigurations.First(c => c.Information.Name == "СОПР"));
			//var remoteRepo = GetFtpRepo(loader, availablePsnConfigurations.First(c => c.Information.Name == "СОПР"), 20, "10.10.10.10", 21, "1", "1");
			//OpenRepository(remoteRepo);
			//CopyInformation(remoteRepo, localRepo);

			//var ftpRepo = loader.GetFtpFilesRepository("C:\\rpd_ftp\\", "Локомотив", "1", loader.AvailablePsnConfigruations.First(c => c.Information.Version == "36"));
			//var nandRepo = loader.GetNandFlashRepository("0l0l0", psnConfigTest);
			//var nandRepo = loader.GetNandFlashRepository("D:\\", psnConfigTest);
			//var zipRepo = loader.GetZippedRepository("C:\\_repo.zip");
			//var zipRepo = loader.GetZippedRepository("C:\\"); // testing wrong path for repo
			//var zipRepo2 = loader.GetZippedRepository("C:\\_repo2.zip");

			//var dirRepo = loader.GetLocalDirectoryRepository("C:\\rpd_repo\\");
			//var dirRepo = loader.GetLocalDirectoryRepository("axaxa:");


			//OpenRepository(nandRepo); LogRepositoryContent(nandRepo);
			//OpenRepository(dirRepo); LogRepositoryContent(dirRepo);
			//OpenRepository(zipRepo); LogRepositoryContent(zipRepo);
			//OpenRepository(ftpClientRepo); LogRepositoryContent(ftpClientRepo);
			//OpenRepository(zipRepo2); LogRepositoryContent(zipRepo2);
			//OpenRepository(ftpRepo); LogRepositoryContent(ftpRepo);
			//var srcPsnStorage = GetStorageFromRepo(nandRepo);



			//TestRelayRepo(nandRepo, psnConfigTm31);

			//CopyInformation(nandRepo, dirRepo);
			//LogRepositoryContent(dirRepo);
			//LogRepositoryContent(zipRepo);
			//CopyInformation(dirRepo, zipRepo);

			//DeleteAllLogsFromRepo(dirRepo);
			//LogRepositoryContent(dirRepo);

			//TestRelayRepo(zipRepo);
			//DeleteAllLogsFromRepo(zipRepo);
			//LogRepositoryContent(zipRepo);

			//CopyInformation(zipRepo, zipRepo2);
			//CopyInformation(zipRepo2, dirRepo);
			//CopyInformation(dirRepo, nandRepo);

			//CopyInformation(ftpRepo, dirRepo);
			//CopyInformation(ftpRepo, zipRepo);
			//CopyInformation(ftpClientRepo, zipRepo);

			Log("Press any key to exit", 0, ConsoleColor.Green);
			Console.ReadKey();
		}

		/*
		static IPsnDataStorage GetStorageFromRepo(IRepository repo) {
			Log("Открывается репозиторий " + repo);
			var signal = new AutoResetEvent(false);
			Exception exc = null;
			repo.Open(ea => {
			          	if (ea.ResultCode != OnCompleteEventArgs.CompleteResult.Ok)
			          		exc = new Exception(ea.Message);
			          	signal.Set();

			          }, pea => { });
			signal.WaitOne();
			if (exc != null) throw exc;
			Log("Репозиторий " + repo + " успешно открыт");
			return repo.DataStorage;
		}*/
		#region Log methods
		private static void Logg(object obj) {
			lock (LogSync) {
				var curTop = Console.CursorTop;
				var curLeft = Console.CursorLeft;
				Console.CursorTop = curTop - 1;
				Console.CursorLeft = 0;
				Log(obj);
				Console.CursorTop = curTop;
				Console.CursorLeft = curLeft;
			}
		}

		private static void Log(object obj) {
			lock (LogSync) {
				Console.WriteLine(obj.ToString());
			}
		}

		private static void Log(object obj, int level) {
			lock (LogSync) {
				string space = string.Empty;
				for (int i = 0; i < level; ++i) {
					space += " ";
				}
				Console.WriteLine(space + obj);
			}
		}

		private static void Log(object obj, int level, ConsoleColor color) {
			lock (LogSync) {
				var fc = Console.ForegroundColor;
				Console.ForegroundColor = color;
				string space = string.Empty;
				for (int i = 0; i < level; ++i) {
					space += " ";
				}
				Console.WriteLine(space + obj);
				Console.ForegroundColor = fc;
			}
		}

		private static void Log(object obj, int level, ConsoleColor color, ConsoleColor backColor) {
			lock (LogSync) {
				var fc = Console.ForegroundColor;
				var bc = Console.BackgroundColor;
				Console.ForegroundColor = color;
				Console.BackgroundColor = backColor;
				string space = string.Empty;
				for (int i = 0; i < level; ++i) {
					space += " ";
				}
				Console.WriteLine(space + obj);
				Console.ForegroundColor = fc;
				Console.BackgroundColor = bc;
			}
		}
		#endregion Log methods


		#region Storage cases tests
		private static void LoadAllStoragedSignals(IPsnDataStorage storage, IPsnProtocolConfiguration config) {
			var psnLogsInStorage = storage.PsnDatas.ToList();
			Log("Число ПСН логов на устройстве: " + psnLogsInStorage.Count);
			Log("Конфигурация ПСН: " + config.Information);
			Log("Загрузка всех сигналов для всех логов...");
			foreach (var psnLog in psnLogsInStorage) {
				Console.WriteLine("PsnLog " + psnLog);
				foreach (var psnDev in config.PsnDevices) {
					Log("Работаем с абонентом ПСН: " + psnDev.Name);
					IPsnData log = psnLog;
					IPsnProtocolDeviceConfiguration dev = psnDev;
					config.ForeachPsnMeterSignal(psnDev.Address, (cmdPartConfiguration, parameterConfiguration) => {
						var points = log.LoadTrend(config, cmdPartConfiguration, parameterConfiguration, DateTime.Now);
						Console.WriteLine(dev.Name + "/" + parameterConfiguration.Name + " = " + points.Count);
						return false;
					});
				}
			}
		}

		private static void LoadAllStoragedSignals(IRepository repo, IPsnConfiguration config) {
			var signal = new AutoResetEvent(false);
			repo.Open(
				args => {
					Log(args.Message);
					signal.Set();
				}, args => { });
			signal.WaitOne();
			signal.Reset();
			Log("Число локомотивов в репозитории = " + repo.Locomotives.Count);
			foreach (var locomotive in repo.Locomotives) {
				Log("Число секций у локомотива <<" + locomotive.Name + ">> = " + locomotive.Sections.Count);
				foreach (var section in locomotive.Sections) {
					Log("Число ПСН логов у секции " + section.Name + " = " + section.Psns.Count);
					foreach (var psnLog in section.Psns) {

					}
				}
			}
			signal.WaitOne();
		}

		private static void SaveLogsToStorage(IPsnDataStorage srcPsnStorage, IDeviceInformationStorage srcDevInfoStorage, IPsnDataStorage dstPsnStorage, IDeviceInformationStorage dstDevInfoStorage) {
			Log("Локальное хранилище создано, число ПСН логов в хранилище: " + dstPsnStorage.PsnDatas.Count());

			var devInfosInStorage = srcDevInfoStorage.DeviceInformations.ToList();
			var psnLogsInStorage = srcPsnStorage.PsnDatas.ToList();

			Log("Число информаций об устройстве на устройстве: " + devInfosInStorage.Count);
			foreach (var devInfo in devInfosInStorage) {
				if (!dstDevInfoStorage.DeviceInformations.Any(i => i.Name == devInfo.Name && i.Description == devInfo.Description)) {
					dstDevInfoStorage.Add(devInfo.Id, devInfo.Name, devInfo.Description);
					Log("Информация об устройстве сохранена в локальное хранилище");
				}
				else Log("Такая информация об устройстве уже есть в локальном харинилище");
			}

			Log("Число ПСН логов на устройстве: " + psnLogsInStorage.Count);
			foreach (var psnLog in srcPsnStorage.PsnDatas) {

				//Console.WriteLine("PsnLog " + psnLog.LogInformation.BeginTime.ToString());
				if (!dstPsnStorage.PsnDatas.Any(pl => pl.Id.ToString() == psnLog.Id.ToString())) {
					dstPsnStorage.Add(psnLog.Id, psnLog, pp => Console.WriteLine(pp.ToString("f2")));
					Log("Лог сохранен в локальное хранилище");
				}
				else Log("Такой лог уже есть в локальном хранилище");
			}

			Log("Число ПСН логов в хранилище после сохранения: " + dstPsnStorage.PsnDatas.Count());
		}

		private static void UpdateFirstDeviceInfoInStorage(IDeviceInformationStorage storage) {
			Log("Попробуем обновить информацию об устройстве");
			var firstDevInfo = storage.DeviceInformations.First();
			storage.Update(firstDevInfo.Id, "Электровоз", "2");
		}

		private static void ClearStorage(IPsnDataStorage storage, IDeviceInformationStorage devstorage) {
			var logs = storage.PsnDatas.ToList();
			foreach (var psnLog in logs) {
				storage.Remove(psnLog.Id);
				Log("Лог удален из хранилища");
			}

			var devInfos = devstorage.DeviceInformations.ToList();
			foreach (var deviceInformation in devInfos) {
				devstorage.Remove(deviceInformation.Id);
				Log("Информация об устройстве удалена из хранилища");
			}
		}

		private static void TestPsnCustomConfigsStorage(string filename) {
			var sf = new StreamedFile(filename);
			var psnStorage = new PsnDataCustomConfigurationsStorageRelayMemoryCache(new PsnDataCustomConfigurationsStorageXDocument(sf, sf));
			psnStorage.Add(new IdentifierStringToLowerBased("Hello"), new IdentifierStringToLowerBased("World"), "VeryInfromativePsnLog #1");
		}

		#endregion Storage cases tests



		#region Repository tests
		private static void OpenRepository(IRepository repo) {
			Log("\n\nОткрытие репозитория " + repo);
			var signal = new AutoResetEvent(false);
			repo.Open(args => {
				Log(args.Message);
				signal.Set();
			}, args => { });
			signal.WaitOne();
			Log("Открытие репозитория " + repo + " завершено");
		}

		private static void CopyInformation(IRepository source, IRepository target) {
			Console.WriteLine("\n\nКопирование данных из " + source + " в " + target);
			var signal = new AutoResetEvent(false);
			foreach (var locomotive in source.Locomotives) {
				foreach (var section in locomotive.Sections) {
					Console.WriteLine("Copying section " + section.Name + " of locomotive " + locomotive.Name);
					int lastPercent = 0;
					target.SaveDataAsync(
						source,
						section.Psns,
						null,
						args => {
							Log(args.Message);
							signal.Set();
						},
						args => {
							if (args.ProgressPercent < lastPercent) {
								Console.WriteLine("OOOps!");
							}
							Console.WriteLine(args.ProgressPercent + "%");
							lastPercent = args.ProgressPercent;
						});
				}
				signal.WaitOne();
				signal.Reset();
			}
		}

		private static void LogRepositoryContent(IRepository repo) {
			Log("\n\nВывод информации о репозитории " + repo);
			Log("Число локомотивов в репозитории: " + repo.Locomotives.Count);
			foreach (var locomotive in repo.Locomotives) {
				Log("Локомотив: " + locomotive.Name);
				foreach (var section in locomotive.Sections) {
					Console.WriteLine("  Секция: " + section.Name + ", число ПСН логов: " + section.Psns.Count);
				}
			}
		}

		private static void TestRelayRepo(IRepository repo) {
			var signal = new AutoResetEvent(false);
			Log("Число локомотивов в репозитории = " + repo.Locomotives.Count);
			foreach (var locomotive in repo.Locomotives) {
				Log("Число секций у локомотива <<" + locomotive.Name + ">> = " + locomotive.Sections.Count);
				foreach (var section in locomotive.Sections) {
					Log("Число ПСН логов у секции " + section.Name + " = " + section.Psns.Count);
					foreach (var psnLog in section.Psns) {
						var log = psnLog;
						foreach (var psnMeter in psnLog.Meters) {
							foreach (var psnChannel in psnMeter.Channels) {
								var channel = psnChannel;
								var meter = psnMeter;
								channel.LoadTrendAsync(args => {
									Log(log + " > " + meter.Name + " > " + channel.Name + " > " + channel.Trend.Count());
									signal.Set();
								});
								signal.WaitOne();
								signal.Reset();
							}
						}

					}
				}
			}
		}

		private static void DeleteAllLogsFromRepo(IRepository repo) {
			Console.WriteLine("\n\nУдаление данных из " + repo);
			var signal = new AutoResetEvent(false);
			Log("Число локомотивов в репозитории = " + repo.Locomotives.Count);
			var locs = repo.Locomotives.ToList();
			foreach (var locomotive in locs) {
				var secs = locomotive.Sections.ToList();
				Log("Число секций у локомотива <<" + locomotive.Name + ">> = " + locomotive.Sections.Count);
				foreach (var section in secs) {
					Log("Число ПСН логов у секции " + section.Name + " = " + section.Psns.Count);
					repo.Remove(section.Psns, args => {
						Log("Логи ПСН удалены, сообщение: " + args.Message);
						signal.Set();
					});
					signal.WaitOne();
					signal.Reset();
				}
			}
		}

		private static void GetAllFtpRepos(ILoader loader, IEnumerable<IPsnConfiguration> availablePsnConfigurations, string host, int port, string username, string password) {
			var psnConfigs = availablePsnConfigurations.ToList();
			var repos = new List<IRepository>();
			var signal = new ManualResetEvent(false);
			//loader.GetFtpRepositoryInfosAsync("10.10.10.10", 21, "1", "1", (eventArgs, infos) => {
			Console.Write("Getting ftp repos async... ");
			loader.GetFtpRepositoryInfosAsync(host, port, username, password, (eventArgs, infos) => {
				Console.WriteLine("done!");
				foreach (var info in infos) {
					Console.WriteLine(info.DeviceNumber);
					repos.Add(loader.GetFtpRepository(info, "1", "1", psnConfigs.First()));
					Console.WriteLine("Builded repo on information: " + info.ToString());
				}
				Console.WriteLine("All repositories builded on it's informations");
				signal.Set();
			});
			signal.WaitOne();
			signal.Reset();
			Console.WriteLine("FTP repos taken: " + repos.Count);
			foreach (var repo in repos) {
				IRepository repo1 = repo;
				Console.WriteLine(repo1 + " > opening...");
				repo.Open(ea => {
					Console.WriteLine(repo1 + " > opened: " + ea.Message);
					Console.WriteLine(repo1 + " > l[0].s[0].PsnLogs count = " + repo1.Locomotives[0].Sections[0].Psns.Count);
					signal.Set();
				}, ea => Console.WriteLine(repo1 + " > " + ea.ProgressPercent + "%"));
				signal.WaitOne();
				signal.Reset();
			}
		}
		private static IRepository GetFtpRepo(ILoader loader, IPsnConfiguration psnConfig, int deviceNumber, string host, int port, string username, string password) {
			IRepository repo = null;
			Exception backException = null;
			var signal = new ManualResetEvent(false);
			Console.Write("Getting ftp repos async... ");
			loader.GetFtpRepositoryInfosAsync(host, port, username, password, (eventArgs, infos) => {
				try {
					Console.WriteLine("done!");
					repo = loader.GetFtpRepository(infos.First(i => i.DeviceNumber == deviceNumber), "ftp loc " + deviceNumber, "1", psnConfig);
					Console.WriteLine("Repo taken async");
				}
				catch (Exception ex) {
					backException = ex;
				}
				finally {
					signal.Set();
				}
			});
			signal.WaitOne();
			if (backException != null) throw backException;
			return repo;
		}

		#endregion Repository tests
	}
}

