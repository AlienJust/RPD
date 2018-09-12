using System.Collections.Generic;
using DataAbstractionLevel.Low.Storage.Contracts;

namespace DataAbstractionLevel.Low.PsnData.PsnBinHandlerMapBased {
	internal class FirstRunResult {
		public List<IPsnPageIndexRecord> FullIndex { get; set; }
		public List<IPsnPageIndexRecord> TimeSortedGoodPages { get; set; }
	}
}