using System.IO;
using DataAbstractionLevel.Low.Storage.StreamableData.Contracts;

namespace RPD.DAL {
	class StreamReadableObjectBasedOnData : IStreamReadableObject {
		private readonly IStreamedData _data;
		public StreamReadableObjectBasedOnData(IStreamedData data) {
			_data = data;
		}

		public Stream GetStreamForReading() {
			return _data.GetStreamForReading();
		}
	}
}