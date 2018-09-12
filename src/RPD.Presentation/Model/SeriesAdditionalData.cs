using Abt.Controls.SciChart;
using AlienJust.Support.Text;
using RPD.Presentation.Contracts.ViewModels;
using RPD.SciChartControl;

namespace RPD.Presentation.Model {
	internal class SeriesAdditionalData : ISeriesAdditionalData {
		public ITrendViewModel TrendViewModel { get; }

		public SeriesAdditionalData(IChartSeriesViewModel chartSeriesViewModel, ITrendViewModel trendViewModel) {
			TrendViewModel = trendViewModel;
			ChartSeries = chartSeriesViewModel;
		}

		public IChartSeriesViewModel ChartSeries { get; set; }

		public PointMetadata GetPointMetadata(int pointIndex) {
			var d = TrendViewModel.TrendData.Trend[pointIndex];
			return new PointMetadata { DataPosition = d.DataPosition, IsValid = d.Valid, CmdData = d.GetCommandBytes().ToText()};
		}
	}
}