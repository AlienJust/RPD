using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.Model.DataSeries;
using Abt.Controls.SciChart.Numerics;
using Abt.Controls.SciChart.Visuals.Annotations;
using Abt.Controls.SciChart.Visuals.RenderableSeries;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RPD.DAL;
using RPD.EventArgs;
using RPD.Presentation.Contracts.ViewModels;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;
using RPD.Presentation.Model;
using RPD.SciChartControl;

namespace RPD.Presentation.ViewModels {
	class SciChartViewModel : ViewModelBase, ISciChartViewModel {
		#region Private Properties

		private readonly IColorsStorage _colorsStorage;

		private ObservableCollection<IChartSeriesViewModel> _analogSeries = new ObservableCollection<IChartSeriesViewModel>();
		private ObservableCollection<IChartSeriesViewModel> _discreteSeries = new ObservableCollection<IChartSeriesViewModel>();
		private readonly ObservableCollection<IAnnotation> _annotations = new ObservableCollection<IAnnotation>();

		private readonly List<IFaultViewModel> _faults = new List<IFaultViewModel>();
		private readonly List<ITrendViewModel> _trends = new List<ITrendViewModel>();
		private ObservableCollection<ISeriesAdditionalData> _analogSeriesAdditionalData = new ObservableCollection<ISeriesAdditionalData>();
		private ObservableCollection<ISeriesAdditionalData> _discreteSeriesAdditionalData = new ObservableCollection<ISeriesAdditionalData>();

		private static readonly List<Color> Colors = new List<Color>();
		private static int _lastColorIndex = 0;

		#endregion
		static SciChartViewModel() {
			foreach (var solidColorBrush in DefaultColors.ColorBrushes)
				Colors.Add(solidColorBrush.Color);
		}


		public SciChartViewModel(IColorsStorage colorsStorage) {
			_colorsStorage = colorsStorage;
			AnalogSeries.CollectionChanged += SeriesOnCollectionChanged;
			DiscreteSeries.CollectionChanged += SeriesOnCollectionChanged;

			MouseRightButtonUpCommand = new RelayCommand<object>(MouseRightButtonUpExecute);
		}

		private void MouseRightButtonUpExecute(object o)
		{
			//MessageBox.Show("LOL");
		}

		private void SeriesOnCollectionChanged(object sender,
				NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs) {
			if (notifyCollectionChangedEventArgs.Action == NotifyCollectionChangedAction.Remove) {
				// Срабатывает когда юзер удалил тренд с графика (нажал кнопку закрыть).
				foreach (var oldItem in notifyCollectionChangedEventArgs.OldItems) {
					var seriesViewModel = (IChartSeriesViewModel)oldItem;
					var renderSeries = (BaseRenderableSeries)seriesViewModel.RenderSeries;
					var trend = (ITrendViewModel)renderSeries.Tag;

					CheckIfColorChanged(trend, seriesViewModel);

					// после этого будет вызван RemoveTrend.
					trend.IsOnPlot = false;
				}
			}
		}

		private void CheckIfColorChanged(ITrendViewModel trend, IChartSeriesViewModel seriesViewModel) {
			var color = _colorsStorage.GetColor(trend.ConfigGuid);
			if (seriesViewModel.RenderSeries.SeriesColor != color) {
				_colorsStorage.SetColor(trend.ConfigGuid, seriesViewModel.RenderSeries.SeriesColor);
				_colorsStorage.Save();
			}
		}

		private static Color GetColorForNewLine() {
			if (_lastColorIndex >= Colors.Count)
				_lastColorIndex = 0;

			return Colors[_lastColorIndex++];
		}

		private void UpdateFaults() {
			_faults.Clear();
			_annotations.Clear();

			foreach (var trendViewModel in _trends) {
				foreach (var faultViewModel in trendViewModel.Faults) {
					if (!_faults.Contains(faultViewModel)) {
						_faults.Add(faultViewModel);

						var line = new VerticalLineAnnotation() {
							X1 = faultViewModel.AccuredAt,
							LabelValue = faultViewModel.Name
						};
						Annotations.Add(line);
					}
				}
			}
		}

		private IChartSeriesViewModel CreateChartSeriesViewModel(ITrendViewModel trendViewModel, bool discrete) {
			var dataSeries = new XyDataSeries<DateTime, double> { SeriesName = trendViewModel.Title };

			CopyPointsFromTrendToSeries(trendViewModel, dataSeries);

			IRenderableSeries series = null;

			if (discrete) {
				series = new FastMountainRenderableSeries {
					DataSeries = dataSeries,
					Tag = trendViewModel,
					AntiAliasing = false,
					IsDigitalLine = true,
					ResamplingMode = ResamplingMode.None
				};
			}
			else {
				series = new FastLineRenderableSeries {
					DataSeries = dataSeries,
					Tag = trendViewModel,
				};
			}

			ChooseSeriesColor(trendViewModel, series);
			return new ChartSeriesViewModel(dataSeries, series);
		}

		private static void CopyPointsFromTrendToSeries(ITrendViewModel trendViewModel, XyDataSeries<DateTime, double> dataSeries) {
			foreach (var dataPoint in trendViewModel.TrendData.Trend)
				dataSeries.Append(dataPoint.Time, dataPoint.Value);
		}

		private void ChooseSeriesColor(ITrendViewModel trendViewModel, IRenderableSeries lineSeries) {
			var color = _colorsStorage.GetColor(trendViewModel.ConfigGuid);
			if (color.HasValue) {
				lineSeries.SeriesColor = color.Value;
			}
			else {
				lineSeries.SeriesColor = GetColorForNewLine();
				_colorsStorage.SetColor(trendViewModel.ConfigGuid, lineSeries.SeriesColor);
				_colorsStorage.Save();
			}
		}


		#region Implementation of ISciChartViewModel

		public ObservableCollection<IChartSeriesViewModel> AnalogSeries {
			get => _analogSeries;
			set { Set(() => AnalogSeries, ref _analogSeries, value); }
		}

		public ObservableCollection<ISeriesAdditionalData> AnalogSeriesAdditionalData {
			get => _analogSeriesAdditionalData;
			set { Set(() => AnalogSeriesAdditionalData, ref _analogSeriesAdditionalData, value); }
		}

		public ObservableCollection<IChartSeriesViewModel> DiscreteSeries {
			get => _discreteSeries;
			set { Set(() => DiscreteSeries, ref _discreteSeries, value); }
		}

		public ObservableCollection<ISeriesAdditionalData> DiscreteSeriesAdditionalData {
			get => _discreteSeriesAdditionalData;
			set { Set(() => DiscreteSeriesAdditionalData, ref _discreteSeriesAdditionalData, value); }
		}

		public ObservableCollection<IAnnotation> Annotations => _annotations;

		public RelayCommand<object> MouseRightButtonUpCommand { get; set; }


		public void AddTrend(ITrendViewModel trendViewModel, Action<OnCompleteEventArgs> onComplete) {
			trendViewModel.TrendData.LoadTrendAsync(
					result => {
						// выполняется в основном потоке приложения
						if (result.ResultCode != OnCompleteEventArgs.CompleteResult.Error) {
							if (trendViewModel.TrendData.Type == TrendType.Discrete) {
								var vm = CreateChartSeriesViewModel(trendViewModel, true);
								var metadata = new SeriesAdditionalData(vm, trendViewModel);
								DiscreteSeries.Add(vm);
								DiscreteSeriesAdditionalData.Add(metadata);
							}
							else {
								var vm = CreateChartSeriesViewModel(trendViewModel, false);
								var metadata = new SeriesAdditionalData(vm, trendViewModel);
								AnalogSeries.Add(vm);
								AnalogSeriesAdditionalData.Add(metadata);
							}
						}
						else
							MessageBox.Show(result.Message, "Ошибка загрузки данных тренда.");

						_trends.Add(trendViewModel);
						UpdateFaults();

						onComplete(result);
					});
		}

		public void RemoveTrend(ITrendViewModel trendViewModel) {
			switch (trendViewModel.TrendData.Type)
			{
				case TrendType.Discrete:
				{
					var ser = DiscreteSeries.
						SingleOrDefault(x => ((BaseRenderableSeries)x.RenderSeries).Tag == trendViewModel);
					if (ser != null)
						DiscreteSeries.Remove(ser);

					var meta =
						DiscreteSeriesAdditionalData.SingleOrDefault(x => ((SeriesAdditionalData)x).TrendViewModel == trendViewModel);
					if (meta != null)
						DiscreteSeriesAdditionalData.Remove(meta);
					break;
				}
				case TrendType.Analogue:
				{
					var ser = AnalogSeries.SingleOrDefault(x => ((BaseRenderableSeries)x.RenderSeries).Tag == trendViewModel);
					if (ser != null)
						AnalogSeries.Remove(ser);

					var meta =
						AnalogSeriesAdditionalData.SingleOrDefault(x => ((SeriesAdditionalData)x).TrendViewModel == trendViewModel);
					if (meta != null)
						AnalogSeriesAdditionalData.Remove(meta);
					break;
				}
				default: throw new NotImplementedException("No such TrendDataType support");
			}

			_trends.Remove(trendViewModel);
			UpdateFaults();

			trendViewModel.TrendData.FreeTrend();
		}

		public void SeriesColorChanged(IChartSeriesViewModel changedSeries, Color color) {
			// Меняем цвет у всех трендов с таким же ConfigId
			ChangeColorsOnEqualConfigIdTrends(changedSeries, color, AnalogSeriesAdditionalData);
			ChangeColorsOnEqualConfigIdTrends(changedSeries, color, DiscreteSeriesAdditionalData);
		}

		private void ChangeColorsOnEqualConfigIdTrends(IChartSeriesViewModel changedSeries, Color color, ObservableCollection<ISeriesAdditionalData> seriesAdditionDataCollection) {
			var changedAdditionalData =
					(SeriesAdditionalData)seriesAdditionDataCollection.SingleOrDefault(x => x.ChartSeries == changedSeries);
			if (changedAdditionalData != null) {
				foreach (var seriesAdditionalData in seriesAdditionDataCollection) {
					var ser = (SeriesAdditionalData)seriesAdditionalData;
					if (ser.TrendViewModel.ConfigGuid == changedAdditionalData.TrendViewModel.ConfigGuid) {
						ser.TrendViewModel.Color = color;
						ser.ChartSeries.RenderSeries.SeriesColor = color;
					}
				}
			}
		}

		#endregion
	}
}
