using System;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.Model.DataSeries;
using GalaSoft.MvvmLight;

namespace RPD.SciChartControl.ViewModel {
	class RpdChartSeriesViewModel : ViewModelBase, IRpdChartSeriesViewModel {
		private double _multiplier = 1.0;
		private double _summand;
		private bool _isOnMainAxis;
		private string _mathFunc;
		private IDataSeries<DateTime, double> _originalDataSeries;
		private bool _isMathFuncChanged;

		public IChartSeriesViewModel ChartSeries { get; }

		public RpdChartSeriesViewModel(IChartSeriesViewModel chartSeriesViewModel) {
			ChartSeries = chartSeriesViewModel;
			MathFunc = "x*1.0";
		}



		public IDataSeries<DateTime, double> OridinalDataSeries {
			get { return _originalDataSeries; }
			private set { Set(() => OridinalDataSeries, ref _originalDataSeries, value); }
		}

		public double Multiplier {
			get { return _multiplier; }
			set { Set(() => Multiplier, ref _multiplier, value); }
		}

		public double Summand {
			get { return _summand; }
			set { Set(() => Summand, ref _summand, value); }
		}

		public string MathFunc {
			get { return _mathFunc; }
			set {
				if (_mathFunc == value)
					return;

				_mathFunc = value;
				RaisePropertyChanged(() => MathFunc);

				IsMathFuncChanged = true;
			}
		}

		public bool IsMathFuncChanged {
			get { return _isMathFuncChanged; }
			set { Set(() => IsMathFuncChanged, ref _isMathFuncChanged, value); }
		}

		public bool IsOnMainYAxis {
			get { return _isOnMainAxis; }
			set { Set(() => IsOnMainYAxis, ref _isOnMainAxis, value); }
		}

		public bool CanMoveBetweenYAxes {
			get { return true; }
		}

		public void CloneDataSeriesIfNull() {
			if (OridinalDataSeries == null)
				OridinalDataSeries = ((XyDataSeries<DateTime, double>)ChartSeries.DataSeries).Clone();
		}
	}
}
