using System;
using DataAbstractionLevel.Low.Storage.Contracts;

namespace DataAbstractionLevel.Low.PsnData {
	internal struct PsnPageHeader {
		public readonly TimeSpan TimeOffsetFromPreviousPage;
		public readonly DateTime? CreatedAt;

		public PsnPageHeader(DateTime? creationTime, TimeSpan timeOffsetFromPerviousPage, PsnPageInfo pageInfo, int pageNumber) {
			CreatedAt = creationTime;
			TimeOffsetFromPreviousPage = timeOffsetFromPerviousPage;
			PageInfo = pageInfo;
			PageNumber = pageNumber;
		}


		public readonly PsnPageInfo PageInfo;
		public readonly int PageNumber;
	}
}