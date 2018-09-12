using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Forms;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.Model.DataSeries;
using Abt.Controls.SciChart.Numerics;
using Abt.Controls.SciChart.Visuals.Axes;
using Abt.Controls.SciChart.Visuals.RenderableSeries;
using Dnv.Utils.Messages;
using GalaSoft.MvvmLight.Messaging;
using NUnit.Framework;
using RPD.SciChartControl.ViewModel;

namespace Rpd.SciChartControl.Tests {
	[TestFixture]
	public class ChartControlViewModelTests {
		private ChartControlViewModel _target;
		private IMessenger _messenger;

		[SetUp]
		public void SetUp() {
			_messenger = new Messenger();
		}

		[TearDown]
		public void TearDown() {
			_messenger.Unregister(this);
		}

		private void CreateTarget() {
			_target = new ChartControlViewModel(_messenger);
		}

		private static ChartSeriesViewModel CreateChartSeries() {
			return new ChartSeriesViewModel(new XyDataSeries<int, int>(), new FastLineRenderableSeries());
		}

		private void CreateSomeSeries() {
			_target.DiscreteSeries.Add(CreateChartSeries());
			_target.DiscreteSeries.Add(CreateChartSeries());
			_target.AnalogSeries.Add(CreateChartSeries());
			_target.AnalogSeries.Add(CreateChartSeries());
		}

		[Test]
		public void AnalogSeriesPropertyChanged_PrevValueIsNull_RpdAnalogSeriesViewModelsPropertyChanged() {
			//arrange
			var chartSeries = CreateChartSeries();
			var analogSeries = new ObservableCollection<IChartSeriesViewModel> { chartSeries };
			CreateTarget();

			//act
			_target.AnalogSeries = analogSeries;

			//assert
			Assert.That(_target.RpdAnalogSeriesViewModels.Count, Is.EqualTo(1));
			Assert.That(_target.RpdAnalogSeriesViewModels[0].ChartSeries, Is.EqualTo(chartSeries));
		}

		[Test]
		public void AnalogSeriesProperty_NewValueAdded_NewValueAddedToRpdAnalogSeriesViewModelsCollection() {
			//arrange
			CreateTarget();
			var collection = new ObservableCollection<IChartSeriesViewModel> { CreateChartSeries() };
			_target.AnalogSeries = collection; // так нагляднее

			//act
			_target.AnalogSeries.Add(CreateChartSeries());

			//assert
			Assert.That(_target.RpdAnalogSeriesViewModels.Count, Is.EqualTo(2));
			Assert.That(_target.RpdAnalogSeriesViewModels[1].ChartSeries, Is.EqualTo(_target.AnalogSeries[1]));
		}

		[Test]
		public void AnalogSeriesProperty_ValueRemoved_ValueRemovedFromRpdAnalogSeriesViewModelCollection() {
			//arrange
			CreateTarget();
			_target.AnalogSeries.Add(CreateChartSeries());
			_target.AnalogSeries.Add(CreateChartSeries());

			//act
			_target.AnalogSeries.RemoveAt(0);

			//assert
			Assert.That(_target.RpdAnalogSeriesViewModels.Count, Is.EqualTo(1));
			Assert.That(_target.RpdAnalogSeriesViewModels[0].ChartSeries, Is.EqualTo(_target.AnalogSeries[0]));
		}

		[Test]
		public void AutoRangeProperty_FirstDiscreteSeriesAddedAndAnalogEmpty_AutoRangeIsOnce() {
			//arrange
			CreateTarget();

			//act
			_target.DiscreteSeries.Add(CreateChartSeries());

			//assert
			Assert.That(_target.AutoRange, Is.EqualTo(AutoRange.Once));
		}

		[Test()]
		public void AutoRangeProperty_AnalogSeriesIsNotEmpty_AutoRangeIsNever() {
			//arrange
			CreateTarget();

			//act
			_target.AnalogSeries.Add(CreateChartSeries());

			//assert
			Assert.That(_target.AutoRange, Is.EqualTo(AutoRange.Never));
		}

		[Test]
		public void AutoRangeProperty_AnalogSeriesAndDiscreteSeriesIsEmpty_AutoRangeIsOnce() {
			//arrange
			CreateTarget();
			_target.DiscreteSeries.Add(CreateChartSeries());
			_target.AnalogSeries.Add(CreateChartSeries());

			//act
			_target.DiscreteSeries.Clear();
			_target.AnalogSeries.Clear();

			//assert
			Assert.That(_target.AutoRange, Is.EqualTo(AutoRange.Once));
		}


		[Test]
		public void NewSeriesAdded_NewSeriesPropertiesAdjustedCorrectly() {
			//arrange
			CreateTarget();
			_target.IsAntialiasingEnabled = !_target.IsAntialiasingEnabled;
			_target.IsMarkersVisible = true;

			//act
			_target.AnalogSeries.Add(CreateChartSeries());

			//assert
			Assert.That(_target.AnalogSeries[0].RenderSeries.AntiAliasing, Is.EqualTo(_target.IsAntialiasingEnabled));
			Assert.That(((BaseRenderableSeries)_target.AnalogSeries[0].RenderSeries).PointMarker, Is.Not.Null);
		}

		[Test]
		public void ShowSelectColorWindowCommand_SeriesColorChanged() {
			//arrange
			CreateTarget();
			_target.AnalogSeries.Add(CreateChartSeries());

			_messenger.Register<DialogMessage<ColorDialog>>(this,
					message => {
						message.Dialog.Color = Color.DarkKhaki;
						message.Result.DialogResult = DialogResult.OK;
						message.ProcessResult();
					});

			//act
			_target.ShowSelectColorWindowCommand.Execute(_target.RpdAnalogSeriesViewModels[0]);

			//assert
			Assert.That(_target.AnalogSeries[0].RenderSeries.SeriesColor.R, Is.EqualTo(Color.DarkKhaki.R));
			Assert.That(_target.AnalogSeries[0].RenderSeries.SeriesColor.G, Is.EqualTo(Color.DarkKhaki.G));
			Assert.That(_target.AnalogSeries[0].RenderSeries.SeriesColor.B, Is.EqualTo(Color.DarkKhaki.B));
			Assert.That(_target.AnalogSeries[0].RenderSeries.SeriesColor.A, Is.EqualTo(Color.DarkKhaki.A));
		}

		[Test]
		public void ChartCloseCommand_AnalogSeries_RemovesAnalogSeriesFromChart() {
			//arrange
			CreateTarget();
			_target.AnalogSeries.Add(CreateChartSeries());
			_target.AnalogSeries.Add(CreateChartSeries());

			//act
			_target.ChartCloseCommand.Execute(_target.RpdAnalogSeriesViewModels[0]);

			//assert
			Assert.That(_target.AnalogSeries.Count, Is.EqualTo(1));
			Assert.That(_target.RpdAnalogSeriesViewModels.Count, Is.EqualTo(1));
		}

		[Test]
		public void ChartCloseCommand_DiscreteSeries_RemovesDiscreteSeriesFromChart() {
			//arrange
			CreateTarget();
			_target.DiscreteSeries.Add(CreateChartSeries());
			_target.DiscreteSeries.Add(CreateChartSeries());

			//act
			_target.ChartCloseCommand.Execute(_target.RpdDiscreteSeriesViewModels[0]);

			//assert
			Assert.That(_target.DiscreteSeries.Count, Is.EqualTo(1));
			Assert.That(_target.RpdDiscreteSeriesViewModels.Count, Is.EqualTo(1));
		}

		[Test]
		public void IsMarkersVisibleProperty_ChangesRelatedPropertyInAllSeries() {
			//arrange
			CreateTarget();
			_target.IsMarkersVisible = false;
			CreateSomeSeries();

			//act
			_target.IsMarkersVisible = true;

			//assert
			foreach (var chartSeriesViewModel in _target.AnalogSeries)
				Assert.That(((BaseRenderableSeries)chartSeriesViewModel.RenderSeries).PointMarker, Is.Not.Null);

			foreach (var chartSeriesViewModel in _target.DiscreteSeries)
				Assert.That(((BaseRenderableSeries)chartSeriesViewModel.RenderSeries).PointMarker, Is.Not.Null);
		}

		[Test]
		public void IsAntialiasingEnabledProperty_True_ChangesRelatedPropertyInAllSeries() {
			//arrange
			CreateTarget();
			_target.IsMarkersVisible = false;
			CreateSomeSeries();

			//act
			_target.IsAntialiasingEnabled = true;

			//assert
			foreach (var chartSeriesViewModel in _target.AnalogSeries)
				Assert.That(chartSeriesViewModel.RenderSeries.AntiAliasing, Is.True);

			foreach (var chartSeriesViewModel in _target.DiscreteSeries)
				Assert.That(chartSeriesViewModel.RenderSeries.AntiAliasing, Is.True);
		}

		[Test]
		public void StrokeThickness_Changed_ChangesRelatedPropertyInAnalogSeries() {
			//arrange
			CreateTarget();
			_target.StrokeThickness = 2;
			CreateSomeSeries();

			//act
			_target.StrokeThickness = 5;

			//assert
			foreach (var chartSeriesViewModel in _target.AnalogSeries)
				Assert.That(chartSeriesViewModel.RenderSeries.StrokeThickness, Is.EqualTo(5));
		}

		[Test]
		public void StrokeThickness_Changed_DiscreteSeriesStrokeThicknessNotChanged() {
			//arrange
			CreateTarget();
			_target.StrokeThickness = 2;
			CreateSomeSeries();
			var thickness = _target.DiscreteSeries[0].RenderSeries.StrokeThickness;

			//act
			_target.StrokeThickness = thickness + 2;

			//assert
			foreach (var chartSeriesViewModel in _target.DiscreteSeries)
				Assert.That(chartSeriesViewModel.RenderSeries.StrokeThickness, Is.EqualTo(thickness));
		}

		[Test]
		public void SelectedResamplingMode_Changed_ChangesRelatedPropertyInAnalogSeries() {
			//arrange
			CreateTarget();
			_target.SelectedResamplingMode = ResamplingMode.Max;
			CreateSomeSeries();

			//act
			_target.SelectedResamplingMode = ResamplingMode.Min;

			//assert
			foreach (var chartSeriesViewModel in _target.AnalogSeries)
				Assert.That(chartSeriesViewModel.RenderSeries.ResamplingMode, Is.EqualTo(ResamplingMode.Min));
		}

		[Test]
		public void SelectedResamplingMode_Changed_DiscreteSeriesStrokeThicknessNotChanged() {
			//arrange
			CreateTarget();
			_target.SelectedResamplingMode = ResamplingMode.Max;
			CreateSomeSeries();
			var resampling = _target.DiscreteSeries[0].RenderSeries.ResamplingMode; // запоминаем изначальное значение

			//act
			_target.SelectedResamplingMode = ResamplingMode.Min;

			//assert
			Assert.That(resampling, Is.Not.EqualTo(_target.SelectedResamplingMode)); // убеждаемся что изначальное значение не совпало с выбранным

			foreach (var chartSeriesViewModel in _target.DiscreteSeries)
				Assert.That(chartSeriesViewModel.RenderSeries.ResamplingMode, Is.EqualTo(resampling));
		}

		[Test]
		public void MoveToOwnYAxisCommand_SeriesIsOnMainAxis_SeriesMovedToNewAxis() {
			//arrange
			CreateTarget();
			CreateSomeSeries();

			//act
			_target.MoveToOwnYAxis.Execute(_target.RpdAnalogSeriesViewModels[0]);

			//assert
			Assert.That(_target.YAxes.Count, Is.EqualTo(2));
			Assert.That(_target.RpdAnalogSeriesViewModels[0].ChartSeries.RenderSeries.YAxisId, Is.Not.EqualTo(ChartControlViewModel.MainYAxisId));
			Assert.That(_target.RpdAnalogSeriesViewModels[0].IsOnMainYAxis, Is.False);
		}

		[Test]
		public void MoveToMainYAxis_SeriesIsOnOwnAxis_SeriesMoverToMainAxisAndOwnAxisRemoved() {
			//arrange
			CreateTarget();
			CreateSomeSeries();
			_target.MoveToOwnYAxis.Execute(_target.RpdAnalogSeriesViewModels[0]);

			//act
			_target.MoveToMainYAxis.Execute(_target.RpdAnalogSeriesViewModels[0]);

			//assert
			Assert.That(_target.YAxes.Count, Is.EqualTo(1));
			Assert.That(_target.RpdAnalogSeriesViewModels[0].IsOnMainYAxis, Is.True);
			Assert.That(_target.YAxes[0].Id, Is.EqualTo(ChartControlViewModel.MainYAxisId));
		}


		[Test]
		public void ChartCloseCommand_SeriesIsNotOnMainAxis_RelatedAxisRemoved() {
			//arrange
			CreateTarget();
			CreateSomeSeries();
			_target.MoveToOwnYAxis.Execute(_target.RpdAnalogSeriesViewModels[0]);

			//act
			_target.ChartCloseCommand.Execute(_target.RpdAnalogSeriesViewModels[0]);

			//assert
			Assert.That(_target.YAxes.Count, Is.EqualTo(1));
		}

	}
}