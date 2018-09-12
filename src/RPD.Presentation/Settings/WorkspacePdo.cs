using System.Collections.Generic;

namespace RPD.Presentation.Settings {
	internal class WorkspacePdo {
		public WorkspacePdo() {
			MainChartAnalogSeries = new List<TrendPdo>();
			MainChartDiscreteSeries = new List<TrendPdo>();
		}

		public List<TrendPdo> MainChartAnalogSeries;
		public List<TrendPdo> MainChartDiscreteSeries;
	}
}