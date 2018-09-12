using System;
using Abt.Controls.SciChart.Model.DataSeries;

namespace RPD.SciChartControl.ChartModifiers.ExportChartDataModifier
{
    internal interface ISeriesDataExporter
    {
        void AddSeries(IDataSeries series, int startIndex, int endIndex);

        void ExportAll();
    }
}