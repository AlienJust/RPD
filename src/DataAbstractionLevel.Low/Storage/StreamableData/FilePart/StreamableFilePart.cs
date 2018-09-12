using System.IO;
using DataAbstractionLevel.Low.InternalKitchen.Streamable;
using DataAbstractionLevel.Low.Storage.StreamableData.Contracts;

namespace DataAbstractionLevel.Low.Storage.StreamableData.FilePart {
	internal class StreamReadableObjectRelaySubStream : IStreamedData {
		private readonly IStreamedData _streamReadableObject;
		private readonly long _beginPositionInStream;
		private readonly long _streamLength;

		public StreamReadableObjectRelaySubStream(IStreamedData relayOnStreamed, long beginPositionInStream, long streamLength) {
			_streamReadableObject = relayOnStreamed;
			_beginPositionInStream = beginPositionInStream;
			_streamLength = streamLength;
		}

		public Stream GetStreamForReading() {
			return new ReadOnlySubStream(_streamReadableObject.GetStreamForReading(), _beginPositionInStream, _streamLength);
		}
	}
}
