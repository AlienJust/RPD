using System;
using DataAbstractionLevel.Low.Storage.Contracts;

namespace DataAbstractionLevel.Low.PsnData.PsnBinHandlerMapBased {
	class PsnPageIndexRecord : IPsnPageIndexRecord {
		private readonly PsnPageInfo _pageInfo;
		private readonly long _absolutePositionInStream;
		private readonly DateTime? _pageTime;
		private readonly int _pageNumber;
		private readonly long _absolutePageNumber;

		public PsnPageIndexRecord(long absolutePositionInStream, PsnPageInfo pageInfo, DateTime? pageTime, int pageNumber, long absolutePageNumber) {
			_absolutePositionInStream = absolutePositionInStream;
			_pageInfo = pageInfo;
			_pageTime = pageTime;
			_pageNumber = pageNumber;
			_absolutePageNumber = absolutePageNumber;
		}

		public PsnPageInfo PageInfo {
			get { return _pageInfo; }
		}

		public long AbsolutePositionInStream {
			get { return _absolutePositionInStream; }
		}

		public long AbsolutePageNumber {
			get { return _absolutePageNumber; }
		}

		public DateTime? PageTime {
			get { return _pageTime; }
		}

		public int PageNumber {
			get { return _pageNumber; }
		}
	}
}