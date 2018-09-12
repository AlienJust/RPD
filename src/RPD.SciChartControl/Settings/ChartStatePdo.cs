using System;
using System.Collections.Generic;

namespace RPD.SciChartControl.Settings {
	internal class ChartStatePdo {
		public ChartStatePdo() {
			AxesState = new List<AxisSettingsPdo>();
		}

		public DateTime XVisibleRangeMin;
		public DateTime XVisibleRangeMax;

		public List<AxisSettingsPdo> AxesState;
	}
}