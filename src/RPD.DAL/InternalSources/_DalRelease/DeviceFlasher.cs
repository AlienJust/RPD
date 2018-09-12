using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using AlienJust.Support.Concurrent.Contracts;
using RPD.EventArgs;
using RpdCommandSystem.FileSystemRelease;

namespace RPD.DalRelease
{
	internal class DeviceFlasher {
		private readonly IWorker<Action> _backWorker;
		private readonly IThreadNotifier _uiNotifier;

		public DeviceFlasher (IWorker<Action> backWorker, IThreadNotifier uiNotifier) {
			_backWorker = backWorker;
			_uiNotifier = uiNotifier;
		}

		public void WriteFirmware(string firmwireHexFilename, string deviceDriveLetter, Action<OnCompleteEventArgs> onComplete) {
			_backWorker.AddWork(
				() => {
					try {
						var cmdWriter = new RpdCommandWriter(deviceDriveLetter);
						var cmd = new CmdWriteFirmware();
						cmdWriter.WriteCommandSync(cmd, 3); // wait 3 seconds after writing
						{
							var p = new Process {
							                    	StartInfo = {
							                    	            	CreateNoWindow = false,
							                    	            	UseShellExecute = false,
							                    	            	FileName = "batchisp",
							                    	            	Arguments = "-device at32uc3b0256 -hardware usb -operation erase f memory flash blankcheck loadbuffer "
							                    	            	            + "\"" + firmwireHexFilename + "\""
							                    	            	            + " program verify"
							                    	            }
							                    };
							p.Start();
							p.WaitForExit();
							//processOutput = p.StandardOutput.ReadToEnd();
							//if (!processOutput.Contains("Summary:  Total 10   Passed 10   Failed 0"))
							//processResultOk = false;
						}
						//if (processResultOk)
						{
							var p = new Process {
							                    	StartInfo = {CreateNoWindow = false, UseShellExecute = false, FileName = "batchisp", Arguments = "-device at32uc3b0256 -hardware usb -operation memory configuration 0xFFFFEFF8"}
							                    };
							//p.StartInfo.CreateNoWindow = true;
							//p.StartInfo.RedirectStandardOutput = true;
							p.Start();
							p.WaitForExit();
							//processOutput = p.StandardOutput.ReadToEnd();
							//if (!processOutput.Contains("ISP done."))
							//processResultOk = false;
						}
						//if (processResultOk)
						{
							var p = new Process {StartInfo = {CreateNoWindow = false, UseShellExecute = false, FileName = "batchisp", Arguments = "-device at32uc3b0256 -hardware usb -operation start reset 0"}};
							//p.StartInfo.CreateNoWindow = true;
							//p.StartInfo.RedirectStandardOutput = true;
							p.Start();
							p.WaitForExit();
							//processOutput = p.StandardOutput.ReadToEnd();
							//if (!processOutput.Contains("Summary:  Total 5   Passed 5   Failed 0"))
							//processResultOk = false;
						}
						Thread.Sleep(1000);
						if (onComplete != null)
							_uiNotifier.Notify(() => onComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Ok, "Прошивка ПО завершена.")));
					}
					catch (Exception ex) {
						if (onComplete != null)
							_uiNotifier.Notify(() => onComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Error, "Не удалось прошить ПО. Ошибка: " + ex)));
					}
				});
		}

		public void ClearArchives(string deviceDriveLetter, Action<OnCompleteEventArgs> onComplete) {
			_backWorker.AddWork(
				() => {
					try {
						var cmdWriter = new RpdCommandWriter(deviceDriveLetter);
						var cmd = new CmdClearArchives();
						cmdWriter.WriteCommandSync(cmd, 5);
						if (onComplete != null)
							_uiNotifier.Notify(() => onComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Ok, "Очистка архивов завершена.")));
					}
					catch (Exception ex) {
						if (onComplete != null)
							_uiNotifier.Notify(() => onComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Error, "Не удалось очистить архивы. Ошибка: " + ex)));
					}
				});
		}

		public void FormatRpd(string deviceDriveLetter, Action<OnCompleteEventArgs> onComplete)
		{
			_backWorker.AddWork(
				() =>
				{
					try
					{
						var cmdWriter = new RpdCommandWriter(deviceDriveLetter);
						var cmd = new CmdFormatRpd();
						cmdWriter.WriteCommandSync(cmd, 15);
						if (onComplete != null)
							_uiNotifier.Notify(() => onComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Ok, "Переформатирование РПД завершено.")));
					}
					catch (Exception ex)
					{
						if (onComplete != null)
							_uiNotifier.Notify(() => onComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Error, "Не удалось очистить архивы. Ошибка: " + ex)));
					}
				});
		}

		public void TestLinkWithMeters(string deviceDriveLetter, Action<OnCompleteEventArgs> onComplete) {
			_backWorker.AddWork(
				() => {
					try {
						var cmdWriter = new RpdCommandWriter(deviceDriveLetter);
						var cmd = new CmdTestLinkWithMeters();
						cmdWriter.WriteCommandSync(cmd, 5);
						if (onComplete != null)
							_uiNotifier.Notify(() => onComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Ok, "Связь с измерителями проверена.")));
					}
					catch (Exception ex) {
						if (onComplete != null)
							_uiNotifier.Notify(() => onComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Error, "Не удалось проверить связь с измерителями. Ошибка: " + ex)));
					}
				});
		}

		public void SetTime(string deviceDriveLetter, Action<OnCompleteEventArgs> onComplete, DateTime time) {
			_backWorker.AddWork(
				() => {
					try {
						var cmdWriter = new RpdCommandWriter(deviceDriveLetter);
						var cmd = new CmdSetTime(time);
						cmdWriter.WriteCommandSync(cmd, 5);
						if (onComplete != null)
							_uiNotifier.Notify(() => onComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Ok, "Время РПД задано успешно.")));
					}
					catch (Exception ex) {
						if (onComplete != null)
							_uiNotifier.Notify(() => onComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Error, "Не удалось задать время. Ошибка: " + ex)));
					}
				});
		}
	}
}
