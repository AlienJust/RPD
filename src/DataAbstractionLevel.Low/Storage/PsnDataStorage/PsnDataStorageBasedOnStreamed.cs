using System;
using System.Collections.Generic;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.Contracts;
using DataAbstractionLevel.Low.Storage.StreamableData.Contracts;
using DataAbstractionLevel.Low.Storage.StreamableStorages.Contracts;

namespace DataAbstractionLevel.Low.Storage.PsnDataStorage
{
	class PsnDataStorageBasedOnStreamed : IPsnDataStorage {
		private readonly IStreamableDataStorage _storage;
		//private IEnumerable<IPsnData> _psnDatas;

		public IEnumerable<IPsnData> PsnDatas {
			get {
				throw new NotImplementedException("TODO"); // TODO
				//return _storage.Datas.Select(sd=>new PsnDataRelayStream(sd.Data, sd.Id)*/)
			}
		}

		public IStreamedData Add(IIdentifier id, IStreamedData data, Action<double> progressChangeAction)
		{
			throw new NotImplementedException();
		}

		public void Remove(IIdentifier id) {
			throw new NotImplementedException();
		}
	}
}
