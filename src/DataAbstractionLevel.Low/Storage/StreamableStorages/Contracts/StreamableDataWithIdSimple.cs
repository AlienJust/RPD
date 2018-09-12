using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.StreamableData.Contracts;

namespace DataAbstractionLevel.Low.Storage.StreamableStorages.Contracts {
	class StreamableDataWithIdSimple : IStreamableDataWithId {
		private readonly IIdentifier _id;
		private readonly IStreamedData _data;
		public StreamableDataWithIdSimple(IIdentifier id, IStreamedData data) {
			_id = id;
			_data = data;
		}

		public IIdentifier Id {
			get { return _id; }
		}

		public IStreamedData Data {
			get { return _data; }
		}
	}
}