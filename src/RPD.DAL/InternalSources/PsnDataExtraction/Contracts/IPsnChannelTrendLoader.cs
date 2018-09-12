using System.Collections.Generic;

namespace RPD.DAL.PsnDataExtraction.Contracts {
	interface IPsnChannelTrendLoader {
		List<IDataPoint> LoadTrend();
		void FreeTrend();
	}
}