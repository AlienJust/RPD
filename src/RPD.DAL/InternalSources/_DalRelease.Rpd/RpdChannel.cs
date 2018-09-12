using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using RPD.DAL.DataExtraction.SimpleRelease;
using RPD.DalRelease;
using RPD.DalRelease.RPD;
using RPD.EventArgs;
using RPD.InternalContracts;

namespace RPD.DAL
{
	/// <summary>
	/// Описывает 1 канал измерителя
	/// </summary>
	internal class RpdChannel : IRpdChannel
	{
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
		public TrendType Type { get; private set; }
		//----------------------------------------------------------
		public List<IDataPoint> Trend { get; set; }
		public bool IsTrendLoaded { get; set; }
		public bool IsEnabled { get; set; }
		public bool IsService { get; set; }
		public bool IsTrendExists { get; set; }

		public string Name { get; set; }
		public int Number { get; set; }
		public IDataPoint CurrentValue { get; set; }
		public IRpdMeter OwnerMeter { get; set; }
		public IDumpCondition DumpCondition { get; set; }

		private Action<OnCompleteEventArgs> OnComplete { get; set; }
		private BackgroundWorker _bgWorkerLoadTrend;
		//----------------------------------------------------------
		public RpdChannel CloneAndLinkWithMeter(RpdMeter m)
		{
			var ch = new RpdChannel(m, Number, Name, IsEnabled, IsService, Type);
			ch.DumpCondition.CopyFrom(DumpCondition);
			return ch;
		}

		public RpdChannel(IRpdMeter owner, int number, string name, bool isEnabled, bool isService, TrendType type)
		{
			InitWorker();
			OwnerMeter = owner;
			Name = name;
			Number = number;
			IsEnabled = isEnabled;
			IsService = isService;
			CurrentValue = null;
			Type = type;
			//-----------------------------
			Trend = new List<IDataPoint>();
			IsTrendExists = false;
			IsTrendLoaded = false;

			DumpCondition = new RpdChannellDumpCondition();
			//this.Start = new DateTime();
			//this.Start = new DateTime();
		}

		/// <summary>
		/// Перекидывает ссылку на тренд этого канала и взводит все необходимые флаги у другого канала
		/// </summary>
		/// <param name="anotherChannel">канал, в который будет произведено копирование ссылки на тренд</param>
		public void CopyTrendToAnotherChannel(RpdChannel anotherChannel)
		{
			anotherChannel.Trend = this.Trend;
			anotherChannel.IsTrendExists = this.IsTrendExists;
			anotherChannel.IsTrendLoaded = this.IsTrendLoaded;
		}

		//-----------------------------------------------------------------------------------------------------------------
		//мб нужна неблокирующая функция с возвращением прогресса ;)?
		public void LoadTrend()
		{
			this.Trend = new List<IDataPoint>();
			ReadChannelTrendResult result = LoadTrendCore();
			if (result != null)
			{
				RpdMeter ownerMeter = (RpdMeter) this.OwnerMeter;
				result.Result.CopyThisSettingsToAnotherMeter(ownerMeter);
				((RpdChannel) result.Result.Channels[0]).CopyTrendToAnotherChannel(this); //взводим флаги и получаем ссылку на тренд в основном потоке :)
			}
		}

		//private void LoadTrendCore(List<IDataPoint> trend)
		private ReadChannelTrendResult LoadTrendCore()
		{
			FaultLog ownerFault = (FaultLog) this.OwnerMeter.OwnerFault;
			//ReadChannelTrendResult result = ownerFault.ReadChannelTrendFromBinaryFile(ownerFault.BinFileInfo.Name, this);
			ReadChannelTrendResult result = ownerFault.ReadChannelTrendFromBinaryFile(this);
			/*
			RpdMeter OwnerPsnMeter = (RpdMeter)this.OwnerPsnMeter;
			OwnerPsnMeter.SettingsReaded = result.Result.SettingsReaded;
			owner
			*/
			//IsTrendLoaded = true;
			return result;
		}

		private void LoadTrendEmulation(List<IDataPoint> trend)
		{
			if (trend != null) trend.Clear();
			int timeRes = 25;
			var r = new Random();
			int g = r.Next(100);
			if (g == 0)
				g = 99;
			double P = r.Next(100)/100.0;
			for (int i = 0; i < 100000; i++)
			{
				this.Trend.Add(new DataPointSimple(Math.Sin(4.0*Math.PI/100000.0*i + P)*1.0*i/10000.0, new DateTime(timeRes*i), true, 0, null));
			}
			IsTrendLoaded = true;
		}

		//----------------------------------------------------------
		private void InitWorker()
		{
			_bgWorkerLoadTrend = new BackgroundWorker();
			_bgWorkerLoadTrend.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BgWorkerLoadTrendRunWorkerCompleted);
			_bgWorkerLoadTrend.DoWork += new DoWorkEventHandler(BgWorkerLoadTrendDoWork);
		}

		private void BgWorkerLoadTrendDoWork(object sender, DoWorkEventArgs e)
		{
			ReadChannelTrendResult result = LoadTrendCore();
			e.Result = result;
		}

		private void BgWorkerLoadTrendRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			try
			{
				var result = (ReadChannelTrendResult) e.Result;
				if (result != null)
				{
					var ownerMeter = (RpdMeter) this.OwnerMeter;
					result.Result.CopyThisSettingsToAnotherMeter(ownerMeter);
					((RpdChannel) result.Result.Channels[0]).CopyTrendToAnotherChannel(this); //взводим флаги и получаем ссылку на тренд в основном потоке :)

					OnComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Ok, "Тренд загружен!"));
				}
				else // результат = null в случае, когда метод FaultLog.ReadChannelTrendFromBinaryFile передал управление блоку catch{}, в общем, когда выпало какое-либо исключение
				{
					OnComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Error, "Невозможно загрузить тренд."));
				}
			}
			catch // ху кэас?
			{
			}
		}

		// НЕБЛОКИРУЮЩАЯ ФУНКЦИЯ HAS BEEN RELEASED!!! =)
		public void LoadTrendAsync(Action<OnCompleteEventArgs> onComplete)
		{
			if (_bgWorkerLoadTrend.IsBusy)
			{
				onComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Error, "executing in progress, cannot run this method twice a time"));
			}
			else
			{
				Trend = new List<IDataPoint>();
				OnComplete = onComplete;
				_bgWorkerLoadTrend.RunWorkerAsync();
			}
		}

		public void FreeTrend()
		{
			Trend.Clear();
			IsTrendLoaded = false;
		}

	}
}
