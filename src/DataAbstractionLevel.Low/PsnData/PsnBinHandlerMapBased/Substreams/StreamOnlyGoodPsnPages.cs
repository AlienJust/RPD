using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataAbstractionLevel.Low.Storage.Contracts;

namespace DataAbstractionLevel.Low.PsnData.PsnBinHandlerMapBased.Substreams {
	class StreamOnlyGoodPsnPages : Stream
	{
		private readonly Stream _source;
		private readonly int _pageLength;
		
		private readonly long _length;
		private readonly List<StreamOnlyGoodPsnPagesIndexRecord> _pagesIndexed;
		private readonly List<StreamOnlyGoodPsnPagesGoodIndexRecord> _pagesGoodIndexed;
		public StreamOnlyGoodPsnPages(Stream source, IList<IPsnPageIndexRecord> psnPageIndexRecords, int pageLength)
		{
			if (source == null) throw new Exception("source stream must be not null");
			_source = source;

			if (psnPageIndexRecords == null) throw new Exception("список страниц должен быть не null");
			
			if (pageLength < 1) throw new Exception("длина страницы должна быть больше 0");
			_pageLength = pageLength;
			
			_length = psnPageIndexRecords.Count(p => p.PageInfo == PsnPageInfo.NormalPage) * _pageLength;

			_pagesIndexed = new List<StreamOnlyGoodPsnPagesIndexRecord>();
			_pagesGoodIndexed = new List<StreamOnlyGoodPsnPagesGoodIndexRecord>();
			int goodPagesCount = 0;
			for (int i = 0; i < psnPageIndexRecords.Count; ++i)
			{
				_pagesIndexed.Add(new StreamOnlyGoodPsnPagesIndexRecord(psnPageIndexRecords[i], goodPagesCount));
				if (psnPageIndexRecords[i].PageInfo == PsnPageInfo.NormalPage)
				{
					_pagesGoodIndexed.Add(new StreamOnlyGoodPsnPagesGoodIndexRecord(i, psnPageIndexRecords[i]));
					goodPagesCount++;
				}
			}
		}


		public override void Flush()
		{
			_source.Flush();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
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

				var sourceAbsolutePosition = GetSourcePosition(requiredPositionFromBeginning);
				_source.Seek(sourceAbsolutePosition, SeekOrigin.Begin);
				//_position = requiredPositionFromBeginning;

				return Position;
			}
			catch (Exception ex) {
				throw new Exception("Cannot seek source stream, args for current method are: " + offset + ", " + origin, ex);
			}
		}

		private long GetSourcePosition(long localPosition) {
			var pageNumber = (int)localPosition/_pageLength;
			try {
				return _pagesGoodIndexed[pageNumber].PageIndexInAllPages * 2048 + localPosition % _pageLength;
			}
			catch(Exception ex) {
				throw new Exception("Cannot get position in source stream for local position = " + localPosition + ", calculated !zero based! (it must be less than pages count in index) page number in index = " + pageNumber + ", total pages count in index = " + _pagesGoodIndexed.Count, ex);
			}
		}

		private long GetLocalPosition(long sourcePosition) {
			var pageNumber = (int) sourcePosition/_pageLength;
			if (_pagesIndexed[pageNumber].PsnPageIndexRecord.PageInfo == PsnPageInfo.BadPage) {
				throw new Exception("Cannot get position in local stream for source position " + sourcePosition + ", page number " + pageNumber + " is not psn data page");
			}
			
			// TODO: индексировать страницы при создании потока данных:
			// страница
			// номер страницы
			// число хороших страниц перед ней
			try { return _pagesIndexed[pageNumber].GoodPagesCountBefore * _pageLength + sourcePosition % _pageLength; }
			catch (Exception ex) {
				throw new Exception("Cannot get position in local stream for source position " + sourcePosition + ", page number " + pageNumber + " is not psn data page", ex);
			}
		}

		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			for (int i = 0; i < count; ++i) {
				var nextLocalPosition = Position + 1;
				_source.Read(buffer, offset + i, 1);
				_source.Seek(GetSourcePosition(nextLocalPosition), SeekOrigin.Begin);
			}
			return count;
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		public override bool CanRead => true;

		public override bool CanSeek => true;

		public override bool CanWrite => false;

		public override long Length => _length;

		public override long Position
		{
			get { return GetLocalPosition(_source.Position); }
			set { _source.Position = GetSourcePosition(value); }
		}
	}
}