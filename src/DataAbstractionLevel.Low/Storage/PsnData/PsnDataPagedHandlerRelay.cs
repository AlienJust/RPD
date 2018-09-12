using System.Collections.Generic;
using DataAbstractionLevel.Low.PsnData.PsnBinHandlerMapBased.Contracts;
using DataAbstractionLevel.Low.Storage.Contracts;

namespace DataAbstractionLevel.Low.Storage.PsnData {
	class PsnDataPagedHandlerRelay : IPsnDataPaged {
		private readonly IPsnDataHandlerIndexed _handler;
		public PsnDataPagedHandlerRelay(IPsnDataHandlerIndexed handler) {
			_handler = handler;
		}

		public IEnumerable<IPsnPageIndexRecord> GetPagesIndex() {
			return _handler.GetPagesIndex();
		}
	}
}