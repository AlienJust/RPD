using System;
using System.ComponentModel;
using RPD.EventArgs;

namespace RPD.DalRelease.Configuration
{
	/*
	class DeviceConfigAsyncReader
	{
		private readonly BackgroundWorker _bwReadConfig;
		private readonly DeviceConfiguration _devConf;
		public DeviceConfigAsyncReader(DeviceConfiguration config)
		{
			_devConf = config;
			_bwReadConfig = new BackgroundWorker();
			_bwReadConfig.DoWork +=BwReadConfigDoWork;
			_bwReadConfig.RunWorkerCompleted += BwReadConfigRunWorkerCompleted;
		}

		public void ReadConfigAsync(string path, Action<OnCompleteEventArgs> onComplete)
		{
			var args = new ArgsReadConfigAsync
			           	{
			           		DevConf = _devConf,
			           		Path = path,
			           		OnComplete = onComplete,
			           		Result = null
			           	};
			if (!_bwReadConfig.IsBusy)
			{
				_bwReadConfig.RunWorkerAsync(args);
			}
			else
			{
				args.Result = new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Error, "Чтение конфигурации уже выполняется.");
				args.OnComplete(args.Result);
			}
		}

		void BwReadConfigDoWork(object sender, DoWorkEventArgs e)
		{
			var args = (ArgsReadConfigAsync)e.Argument;
			if (_devConf.ReadSync(args.Path))
			{
				args.Result = new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Ok, "Конфигурация успешно считана");
			}
			else
			{
				args.Result = new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Error, "Ошибка чтения конфигурации");
			}
			e.Result = args;
		}
		void BwReadConfigRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			var args = (ArgsReadConfigAsync) e.Result;
			args.OnComplete(args.Result);
		}
	}
	class ArgsReadConfigAsync
	{
		public DeviceConfiguration DevConf { get; set; }
		public string Path { get; set; }
		public Action<OnCompleteEventArgs> OnComplete { get; set; }
		public OnCompleteEventArgs Result { get; set; }
	}*/
}
