using System;
using System.Collections.Generic;
using System.Linq;
using AlienJust.Support.Identy;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.PsnData;
using DataAbstractionLevel.Low.Storage.StreamableData.Contracts;
using DataAbstractionLevel.Low.Storage.StreamableData.FilePart;
using DataAbstractionLevel.Low.Storage.StreamableData.SingleFile;
using DataAbstractionLevel.Low.Storage.StreamableStorages.Contracts;

namespace DataAbstractionLevel.Low.Storage.StreamableStorages.SingleFile
{
	class StreamableStorageSingleFile : IStreamableDataStorage {
		private readonly string _filename;
		private readonly IEnumerable<IIntervalWithId<long>> _intervals;

		public StreamableStorageSingleFile(string filename, IEnumerable<IIntervalWithId<long>> intervals)
		{
			_filename = filename;
			_intervals = intervals;
		}

		public IEnumerable<IStreamableDataWithId> Datas
		{
			get {
				return _intervals.Select(
					interval =>
						new StreamableDataWithIdSimple(
							new IdentifierStringToLowerBased(interval.Interval.Begin.ToString("x8") + interval.Interval.End.ToString("x8")),
							new StreamReadableObjectRelaySubStream(new StreamedFile(_filename), interval.Interval.Begin, interval.Interval.End)));
			}
		}

		public void Add(IIdentifier id, IStreamedData data) {
			throw new NotSupportedException("Cannot add data to storage, operation is not supported");
		}

		public void Remove(IIdentifier id) {
			throw new NotSupportedException("Cannot add remove data from storage, operation is not supported");
		}
	}
}
