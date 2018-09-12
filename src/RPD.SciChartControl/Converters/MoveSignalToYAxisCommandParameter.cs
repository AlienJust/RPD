using Abt.Controls.SciChart.Visuals.Axes;
using RPD.SciChartControl.ViewModel;

namespace RPD.SciChartControl.Converters
{
    internal class MoveSignalToYAxisCommandParameter
    {
        public IAxis Axis { get; set; }
        public IRpdChartSeriesViewModel SeriesVm { get; set; }
    }
}