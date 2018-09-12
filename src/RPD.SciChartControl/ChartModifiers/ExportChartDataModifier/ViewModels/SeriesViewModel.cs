using Abt.Controls.SciChart.Visuals.RenderableSeries;
using GalaSoft.MvvmLight;

namespace RPD.SciChartControl.ChartModifiers.ExportChartDataModifier.ViewModels
{
    internal class SeriesViewModel : ViewModelBase
    {
        public SeriesViewModel(IRenderableSeries series)
        {
            RenderableSeries = series;
            Name = RenderableSeries.DataSeries.SeriesName;
        }

        private bool _isChecked;

        public bool IsChecked 
        {
            get { return _isChecked; }
            set { Set(() => IsChecked, ref _isChecked, value); }
        }

        public string Name { get; private set; }

        public IRenderableSeries RenderableSeries { get; private set; }
    }
}