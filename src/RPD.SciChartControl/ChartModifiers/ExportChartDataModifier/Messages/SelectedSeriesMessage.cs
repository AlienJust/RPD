using System.Collections.Generic;
using Abt.Controls.SciChart.Visuals.RenderableSeries;

namespace RPD.SciChartControl.ChartModifiers.ExportChartDataModifier.Messages
{
    internal class SelectedSeriesMessage
    {
        public SelectedSeriesMessage(List<IRenderableSeries> series)
        {
            Series = series;
        }

        public List<IRenderableSeries> Series { get; private set; }
    }
}