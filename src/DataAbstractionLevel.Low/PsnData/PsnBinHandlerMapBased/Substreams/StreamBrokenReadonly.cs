using System;
using System.IO;

namespace DataAbstractionLevel.Low.PsnData.PsnBinHandlerMapBased.Substreams {
	class StreamBrokenReadonly: Stream {
		private readonly Stream _source;
		private readonly long _brakePositionInSource;

		private readonly long _length;
		private readonly long _returnToSourceBeginningAfterReach;
		
		public StreamBrokenReadonly(Stream source, long brakePositionInSource)
		{
			_source = source;
			_brakePositionInSource = brakePositionInSource;

			_length = _source.Length;
			_returnToSourceBeginningAfterReach = _length - brakePositionInSource;
		}


		public override void Flush() {
			_source.Flush();
		}

		public override long Seek(long offset, SeekOrigin origin) {
			try {
				long requiredPositionFromBegining;
				switch (origin) {
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
				_source.Seek(sourceAbsolutePosition, SeekOrigin.Begin);

				return Position;
			}
			catch (Exception ex) {
				throw new Exception("Cannot seek source stream, args for current method are: " + offset + ", " + origin, ex);
			}
		}

		private long GetLocalPosition(long sourcePosition)
		{
			long localPosition;
			if (sourcePosition < _brakePositionInSource)
			{
				localPosition = sourcePosition + _returnToSourceBeginningAfterReach;
			}
			else
			{
				localPosition = sourcePosition - _brakePositionInSource;
			}
			return localPosition;
		}


		private long GetSourcePosition(long localPosition)
		{
			long sourcePosition;
			if (localPosition < _returnToSourceBeginningAfterReach)
			{
				sourcePosition = localPosition + _brakePositionInSource;
			}
			else
			{
				sourcePosition = localPosition - _returnToSourceBeginningAfterReach;
			}
			return sourcePosition;
		}

		public override long Position
		{
			get { return GetLocalPosition(_source.Position); }
			set { _source.Position = GetSourcePosition(value); }
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
	}
}