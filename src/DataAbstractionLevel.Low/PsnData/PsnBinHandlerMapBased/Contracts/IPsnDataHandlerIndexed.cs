using System.Collections.Generic;
using DataAbstractionLevel.Low.Storage.Contracts;

namespace DataAbstractionLevel.Low.PsnData.PsnBinHandlerMapBased.Contracts {
	internal interface IPsnDataHandlerIndexed {
		IEnumerable<IPsnPageIndexRecord> GetPagesIndex();
	}
}