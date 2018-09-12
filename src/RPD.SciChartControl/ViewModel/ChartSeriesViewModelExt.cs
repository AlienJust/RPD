using System.Windows.Media;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.Model.DataSeries;
using Abt.Controls.SciChart.Visuals.RenderableSeries;

namespace RPD.SciChartControl.ViewModel
{
    class ChartSeriesViewModelExt: ChartSeriesViewModel
    {
        public ChartSeriesViewModelExt(IDataSeries dataSeries, IRenderableSeries renderSeries) : base(dataSeries, renderSeries)
        {
        }

        public delegate void SeriesColorChangedEvent(object sender, Color newColor);

        public event SeriesColorChangedEvent OnSeriesColorChanged;
    }
}
