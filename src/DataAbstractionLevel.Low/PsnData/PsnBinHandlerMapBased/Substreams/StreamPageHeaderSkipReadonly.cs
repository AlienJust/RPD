using System;
using System.IO;

namespace DataAbstractionLevel.Low.PsnData.PsnBinHandlerMapBased.Substreams {
	class StreamPageHeaderSkipReadonly: Stream {
		private readonly Stream _source;
		private readonly int _pageLength;
		private readonly int _pageHeaderLength;
		private readonly int _pageDataLength;

		private readonly long _length;

		public StreamPageHeaderSkipReadonly(Stream source, int pageLength, int pageHeaderLength) {
			_source = source;
			_pageLength = pageLength;
			_pageHeaderLength = pageHeaderLength;
			_pageDataLength = _pageLength - _pageHeaderLength;

			_length = (source.Length/_pageLength)*_pageDataLength + source.Length%_pageLength;
		}


		public override void Flush() {
			_source.Flush();
		}

		public override long Seek(long offset, SeekOrigin origin) {
			try {
				long requiredPositionFromBeginning;
				switch (origin) {
					case SeekOrigin.Begin:
						requiredPositionFromBeginning = offset;
						break;
					case SeekOrigin.Current:
						requiredPositionFromBeginning = Position + offset;
						break;
					case SeekOrigin.End:
						requiredPositionFromBeginning = _length + offset;
						break;
					default:
						throw new Exception("Unsupported SeekOrigin");
				}
				// TODO: seek source stream;
				var sourceAbsolutePosition = GetSourcePosition(requiredPositionFromBeginning);
				_source.Seek(sourceAbsolutePosition, SeekOrigin.Begin);

				return Position;
			}
			catch (Exception ex) {
				throw new Exception("Cannot seek source stream, args for current method are: " + offset + ", " + origin, ex);
			}
		}

		private long GetSourcePosition(long localPosition) {
			return localPosition + (1 + (localPosition / _pageDataLength)) * _pageHeaderLength;
		}

		private long GetLocalPosition(long sourcePosition)
		{
			var positionInPageOffset = sourcePosition % _pageLength;
			if (positionInPageOffset < _pageHeaderLength) throw new Exception("Cannot get local position for source one as " + sourcePosition + ", unable to map such source sourcePosition to skipstream");
			return (sourcePosition / 2048) * _pageDataLength + positionInPageOffset - _pageHeaderLength;
		}

		public override void SetLength(long value) {
			throw new NotImplementedException();
		}

		public override int Read(byte[] buffer, int offset, int count) {
			for (int i = 0; i < count; ++i) {
				var nextLocalPosition = Position + 1;
				_source.Read(buffer, offset + i, 1);
				_source.Seek(GetSourcePosition(nextLocalPosition), SeekOrigin.Begin);
			}
			return count;
		}

		public override void Write(byte[] buffer, int offset, int count) {
			throw new NotImplementedException();
		}

		public override bool CanRead => true;
		public override bool CanSeek => true;
		public override bool CanWrite => false;
		public override long Length => _length;

		public override long Position {
			get { return GetLocalPosition(_source.Position); }
			set { _source.Position = GetSourcePosition(value); }
		}
	}
}