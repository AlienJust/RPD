using System;
using System.Collections.Generic;
using System.Linq;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.Contracts;
using DataAbstractionLevel.Low.Storage.PsnData;
using DataAbstractionLevel.Low.Storage.StreamableData.Contracts;

namespace DataAbstractionLevel.Low.Storage.PsnDataStorage
{
	public class StreamedDataStorageRelayMemoryCache : IPsnDataStorage {
		private readonly List<IPsnData> _streamedDatas;
		private readonly IPsnDataStorage _streamedDataStorage;

		public StreamedDataStorageRelayMemoryCache(IPsnDataStorage streamedDataStorage) {
			_streamedDataStorage = streamedDataStorage;
			_streamedDatas = _streamedDataStorage.PsnDatas.ToList();
		}

		public IEnumerable<IPsnData> PsnDatas {
			get { return _streamedDatas; } // TODO: lazy
		}

		public IStreamedData Add(IIdentifier id, IStreamedData data, Action<double> progressChangeAction)
		{
			var newlyStoredData = _streamedDataStorage.Add(id, data, progressChangeAction);
			_streamedDatas.Add(new PsnDataRelayStream(newlyStoredData, id));
			return newlyStoredData;
		}

		public void Remove(IIdentifier id)
		{
			_streamedDataStorage.Remove(id);
			var datasToRemove = _streamedDatas.Where(d => d.Id.IdentyString == id.IdentyString).ToList();
			foreach (var streamedData in datasToRemove) {
				_streamedDatas.Remove(streamedData);
			}
		}
	}
}
