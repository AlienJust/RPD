using DataAbstractionLevel.Low.Storage.Contracts;

namespace DataAbstractionLevel.Low.PsnData.PsnBinHandlerMapBased.Substreams {
	internal sealed class StreamOnlyGoodPsnPagesIndexRecord {
		public IPsnPageIndexRecord PsnPageIndexRecord { get; }
		public int GoodPagesCountBefore { get; }

		public StreamOnlyGoodPsnPagesIndexRecord(IPsnPageIndexRecord psnPageIndexRecord, int goodPagesCountBefore) {
			PsnPageIndexRecord = psnPageIndexRecord;
			GoodPagesCountBefore = goodPagesCountBefore;
		}
	}
}