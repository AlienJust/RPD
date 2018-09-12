using System.IO;
using DataAbstractionLevel.Low.InternalKitchen.Streamable;
using DataAbstractionLevel.Low.Storage.StreamableData.Contracts;

namespace DataAbstractionLevel.Low.Storage.StreamableData.ZippedFile
{
	public class StreamedZippedFile : IStreamedData, IStreamedDataWritable {
		private readonly string _zipFilename;
		private readonly string _inzipFilename;

		public StreamedZippedFile(string zipFilename, string inzipFilename) {
			_zipFilename = zipFilename;
			_inzipFilename = inzipFilename;
		}

		public Stream GetStreamForReading() {
			return new ZipFileStream(_zipFilename, _inzipFilename, "R", true);
		}

		public Stream GetStreamForWriting() {
			return new ZipFileStream(_zipFilename, _inzipFilename, "W", false);
		}
	}
}
