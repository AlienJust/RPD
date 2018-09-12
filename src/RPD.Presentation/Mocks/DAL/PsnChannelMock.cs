using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using Abt.Controls.SciChart.Example.Data;
using RPD.DAL;
using RPD.EventArgs;

namespace RPD.Presentation.Mocks.DAL {
	class PsnChannelMock : IPsnChannel {
		private readonly DateTime _startTime;
		private readonly GeneratedDataType _dataType;
		private Action<OnCompleteEventArgs> _onComplete;

		private const int PointCount = 5000;

		public PsnChannelMock(IPsnMeter owner, TrendType type, DateTime startTime, GeneratedDataType dataType) {
			_startTime = startTime;
			_dataType = dataType;
			Type = type;
			OwnerPsnMeter = owner;

			IsEnabled = true;
			Trend = new List<IDataPoint>();

			UnicId = Guid.NewGuid();
		}


		#region Implementation of ILazyTrendData

		public void LoadTrendAsync(Action<OnCompleteEventArgs> onComplete) {
			_onComplete = onComplete;

			var bg = new BackgroundWorker();

			bg.DoWork += BgOnDoWork;
			bg.RunWorkerCompleted += BgOnRunWorkerCompleted;
			bg.RunWorkerAsync();
		}

		private void BgOnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs) {
			Trend = (List<IDataPoint>)runWorkerCompletedEventArgs.Result;
			IsTrendLoaded = true;

			_onComplete(new OnCompleteEventArgs(OnCompleteEventArgs.CompleteResult.Ok, ""));
		}

		readonly Random random = new Random();

		private void BgOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs) {
			var trend = new List<IDataPoint>();
			var time = _startTime;

			var series = GenerateSeriesByType();

			for (int i = 0; i < series.Count; i++) {
				time += TimeSpan.FromMilliseconds(20);
				if (Type == TrendType.Analogue)
					trend.Add(new DataPointMock(series.YData[i], time));
				else {
					var val = series.YData[i];
					trend.Add(new DataPointMock(val > 0 ? 1 : 0, time));
				}
			}

			//Start = _startTime;
			//End = time;

			doWorkEventArgs.Cancel = false;
			doWorkEventArgs.Result = trend;
		}

		public IEnumerable<Point> GetNoisySinewave(double amplitude, double phase, int pointCount, double noiseAmplitude) {
			// Uses datamanager from the Examples suite
			var sinewave = DataManager.Instance.GetSinewave(amplitude, phase, pointCount);

			// Add some noise
			foreach (var pt in sinewave) {
				yield return new Point(pt.X, pt.Y + (random.NextDouble() * noiseAmplitude - noiseAmplitude * 0.5));
			}
		}

		private DoubleSeries GenerateSeriesByType() {
			var series = new DoubleSeries();
			double startPhase = 0.12;
			switch (_dataType) {
				case GeneratedDataType.Eeg:
					series = DataManager.Instance.GenerateEEG(PointCount, ref startPhase, 15);
					break;
				case GeneratedDataType.ClusteredPoints:
					series = DataManager.Instance.GetClusteredPoints(12, 45, 1, PointCount);
					break;
				case GeneratedDataType.DampedSinewave:
					series = DataManager.Instance.GetDampedSinewave(15, 2, PointCount);
					break;
				case GeneratedDataType.ExponentialCurve:
					series = DataManager.Instance.GetExponentialCurve(15, PointCount);
					break;

				case GeneratedDataType.FourierSeries:
					series = DataManager.Instance.GetFourierSeries(15, 2, PointCount);
					break;

				case GeneratedDataType.FourierSeriesZoomed:
					series = DataManager.Instance.GetFourierSeriesZoomed(15, 2, 1, 10, PointCount);
					break;

				case GeneratedDataType.LissajousCurve:
					series = DataManager.Instance.GetLissajousCurve(0.23, 0.64, 0.87, PointCount);
					break;

				case GeneratedDataType.RandomDoubleSeries:
					series = DataManager.Instance.GetRandomDoubleSeries(PointCount);
					break;

				case GeneratedDataType.Sinewave:
					series = DataManager.Instance.GetSinewave(0.12, 0.4, PointCount);
					break;

				case GeneratedDataType.Spiral:
					series = DataManager.Instance.GenerateSpiral(0, 0, 100, PointCount);
					break;

				case GeneratedDataType.SquirlyWave:
					series = DataManager.Instance.GetSquirlyWave();
					break;

				case GeneratedDataType.StraightLine:
					series = DataManager.Instance.GetStraightLine(12, 5, PointCount);
					break;
			}
			return series;
		}


		public void FreeTrend() {
			;
		}

		public List<IDataPoint> Trend { get; private set; }
		public TrendType Type { get; private set; }
		public bool IsTrendLoaded { get; private set; }

		#endregion

		public string Name { get; set; }
		public bool IsEnabled { get; }
		public bool IsInput { get; private set; }
		public bool CanBeFaultSign { get; private set; }
		public bool IsFaultSign { get; private set; }
		public IPsnMeter OwnerPsnMeter { get; }
		public Guid UnicId { get; }
		public Guid ConfigurationId { get; set; }
	}
}
