using System.Collections.ObjectModel;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.Visuals.RenderableSeries;

namespace RPD.SciChartControl.ViewModel
{
    internal interface IDiscreteChartSeriesViewModel : IRpdChartSeriesViewModel
    {
        ChartDataObject SeriesData { get; set; }

        /// <summary>
        /// Содержит один единственный элемент IRpdChartSeriesViewModel.ChartSeries.RenderSeries. 
        /// Используется для того-чтобы можно было прибиндится к модели представления из дискретного чарта.
        /// </summary>
        ObservableCollection<IRenderableSeries> RenderableSeries { get; set; }
    }


    class DiscreteChartSeriesViewModel : RpdChartSeriesViewModel, IDiscreteChartSeriesViewModel
    {
        private ChartDataObject _seriesData = new ChartDataObject();

        public DiscreteChartSeriesViewModel(IChartSeriesViewModel chartSeriesViewModel) : base(chartSeriesViewModel)
        {
            RenderableSeries = new ObservableCollection<IRenderableSeries> {chartSeriesViewModel.RenderSeries};
        }

        #region Implementation of IDiscreteChartSeriesViewModel

        public ChartDataObject SeriesData
        {
            get { return _seriesData; }
            set { Set(() => SeriesData, ref _seriesData, value); }
        }

        public ObservableCollection<IRenderableSeries> RenderableSeries { get; set; }

        #endregion

        public new bool CanMoveBetweenYAxes => false;
    }
}
