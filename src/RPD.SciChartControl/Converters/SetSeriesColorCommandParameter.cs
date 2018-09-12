using System.Windows.Media;
using RPD.SciChartControl.ViewModel;

namespace RPD.SciChartControl.Converters
{
    internal class SetSeriesColorCommandParameter
    {
        public Color Color { get; set; }
        public IRpdChartSeriesViewModel ViewModel { get; set; }
    }
}
