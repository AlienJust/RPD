using DataAbstractionLevel.Low.Storage.Contracts;

namespace DataAbstractionLevel.Low.PsnData.PsnBinHandlerMapBased.Substreams {
	internal sealed class StreamOnlyGoodPsnPagesGoodIndexRecord
	{
		public int PageIndexInAllPages { get; }
		public IPsnPageIndexRecord PsnPageIndexRecord { get; }

		public StreamOnlyGoodPsnPagesGoodIndexRecord(int pageIndexInAllPages, IPsnPageIndexRecord psnPageIndexRecord)
		{
			PageIndexInAllPages = pageIndexInAllPages;
			PsnPageIndexRecord = psnPageIndexRecord;
		}
	}
}