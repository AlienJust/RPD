using System;
using System.IO;

namespace DataAbstractionLevel.Low.InternalKitchen.Streamable {
	internal sealed class ReadOnlySubStream : Stream {
		private readonly Stream _sourceStream;
		private readonly long _beginInSource;
		private readonly long _length;
		
		public override bool CanWrite => false;

		public override bool CanSeek => true;

		public override bool CanRead => true;

		public override void Write(byte[] buffer, int offset, int count) {
			throw new NotImplementedException();
		}

		public override void WriteByte(byte value) {
			throw new NotImplementedException();
		}

		public override void SetLength(long value) {
			throw new NotImplementedException();
		}

		public override long Seek(long offset, SeekOrigin origin) {
			long requiredPositionFromBegining;
			switch (origin)
			{
				case SeekOrigin.Begin:
					requiredPositionFromBegining = offset;
					break;
				case SeekOrigin.Current:
					requiredPositionFromBegining = Position + offset;
					break;
				case SeekOrigin.End:
					requiredPositionFromBegining = _length + offset;
					break;
				default:
					throw new Exception("Unsupported SeekOrigin");
			}


			long sourceAbsolutePosition = GetSourcePosition(requiredPositionFromBegining);
			_sourceStream.Seek(sourceAbsolutePosition, SeekOrigin.Begin);

			return Position;
		}

		public override long Length => _length;

		public override void Flush() {
			_sourceStream.Flush();
		}

		public ReadOnlySubStream(Stream source, long beginInSource, long length) {
			_sourceStream = source;
			_beginInSource = beginInSource;
			_length = length;
			Seek(0, SeekOrigin.Begin);
		}

		public override int Read(byte[] buffer, int offset, int count) {
			int readedCount = 0;
			for (int i = 0; i < count; ++i) {
				if (IsMaximumPositionReached()) break;
				_sourceStream.Read(buffer, offset + i, 1);
				readedCount++;
			}
			return readedCount;
		}

		private bool IsMaximumPositionReached() {
			return !(Position < _length);
		}

		public override int ReadByte()
		{
			if (IsMaximumPositionReached()) return -1;
			return _sourceStream.ReadByte();
		}

		public override void Close() {
			_sourceStream.Close();
		}

		protected override void Dispose(bool disposing) {
			_sourceStream.Dispose();
		}

		private long GetLocalPosition(long sourcePosition) {
			return sourcePosition - _beginInSource;
		}


		private long GetSourcePosition(long localPosition) {
			return _beginInSource + localPosition;
		}

		public override long Position
		{
			get => GetLocalPosition(_sourceStream.Position);
			set => _sourceStream.Position = GetSourcePosition(value);
		}
	}
}
