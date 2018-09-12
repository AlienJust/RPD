using System;
using System.Collections.ObjectModel;
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
	/// Представляет собой аналоговый сигнал для отображения
	/// </summary>
	internal class Signal : ISignal
	{
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
		public TrendType Type { get; private set; }

		public Signal(FaultLog owner, string name, string mathOperation)
		{
			this.OwnerFault = owner;
			this.Name = name;
			this.MathOperation = mathOperation;
			this.Channels = new ObservableCollection<IRpdChannel>();

			Trend = new List<IDataPoint>();
			IsTrendExists = false;
			IsTrendLoaded = false;

			InitWorker();
		}

		public List<IDataPoint> Trend { get; set; }

		public bool IsTrendLoaded { get; set; }

		//мб нужна неблокирующая функция с возвращением прогресса ;)?
		public void LoadTrend()
		{
			LoadTrendCore(this.Trend);
		}

		private void LoadTrendCore(List<IDataPoint> trend)
		{
			if (trend != null) trend.Clear();
			const int timeRes = 25;
			var r = new Random();
			int g = r.Next(100);
			if (g == 0)
				g = 99;
			double P = r.Next(100)/100.0;
			double amp = r.NextDouble() - 0.5;
			double y = r.NextDouble() - 0.5;
			int mode = r.Next(10) - 5;

			if (mode > 0)
			{
				for (int i = 0; i < 100000; i++)
				{
					Trend.Add(new DataPointSimple(y + Math.Sin(4.0*Math.PI/100000.0*i + P)*amp*1.0*i/10000.0, new DateTime(timeRes*i), true, 0, null));
				}
			}
			else
			{
				for (int i = 0; i < 100000; i++)
				{
					Trend.Add(new DataPointSimple(y + Math.Exp(i / 100000.0) * amp * 0.05 * i / 10000.0, new DateTime(timeRes * i), true, 0, null));
				}
			}
			IsTrendLoaded = true;
		}

		private BackgroundWorker _bgWorkerLoadTrend;

		private void InitWorker()
		{
			_bgWorkerLoadTrend = new BackgroundWorker();
			_bgWorkerLoadTrend.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BgWorkerLoadTrendRunWorkerCompleted);
			_bgWorkerLoadTrend.DoWork += new DoWorkEventHandler(BgWorkerLoadTrendDoWork);
		}

		private void BgWorkerLoadTrendDoWork(object sender, DoWorkEventArgs e)
		{
			var someTrend = new List<IDataPoint>();
			LoadTrendCore(someTrend);
			e.Result = someTrend;
		}

		private void BgWorkerLoadTrendRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			try
			{
				this.Trend = (List<IDataPoint>) e.Result;
				OnComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Ok, "job's done!"));
			}
			catch
			{
			}
		}

		private Action<OnCompleteEventArgs> OnComplete { get; set; }
		//НЕБЛОКИРУЮЩАЯ ФУНКЦИЯ HAS BEEN RELEASED!!! =)
		public void LoadTrendAsync(Action<OnCompleteEventArgs> onComplete)
		{
			if (_bgWorkerLoadTrend.IsBusy)
			{
				onComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Error, "executing in progress, cannot run this method twice a time"));
			}
			else
			{
				OnComplete = onComplete;
				_bgWorkerLoadTrend.RunWorkerAsync();
			}
		}

		public void FreeTrend()
		{
			this.Trend.Clear();
			IsTrendLoaded = false;
		}

		public bool IsTrendExists { get; set; }

		//----------------------------------------------------------

		public ObservableCollection<IRpdChannel> Channels { get; set; }

		public string MathOperation { get; set; }

		public string Name { get; set; }

		public IFaultLog OwnerFault { get; set; }
	}
}
