using System;
using System.Collections.Generic;
using System.Linq;
using AlienJust.Support.Identy;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.StreamableData.Contracts;
using DataAbstractionLevel.Low.Storage.StreamableStorages.Contracts;

namespace DataAbstractionLevel.Low.Storage.StreamableStorages.Relay
{
	class StreamableStorageRelay : IStreamableDataStorage
	{
		private readonly Lazy<List<IStreamableDataWithId>> _dataIdentifiers;
		private readonly IStreamableDataStorage _storage;
		public StreamableStorageRelay(IStreamableDataStorage storage) {
			_storage = storage;
			_dataIdentifiers = new Lazy<List<IStreamableDataWithId>>(() => _storage.Datas.ToList());
		}

		public IEnumerable<IStreamableDataWithId> Datas
		{
			get { return _dataIdentifiers.Value; }
		}

		public void Add(IIdentifier id, IStreamedData data) {
			_storage.Add(new IdentifierStringToLowerBased(id.IdentyString), data);
			var storedData = _storage.Datas.First(sd => sd.Id.IdentyString == id.IdentyString);
			_dataIdentifiers.Value.Add(new StreamableDataWithIdSimple(new IdentifierStringToLowerBased(id.IdentyString), storedData.Data)); // data must be taken from storage after adding
		}

		public void Remove(IIdentifier id) {
			var localData = _dataIdentifiers.Value.First(d => d.Id.IdentyString == id.IdentyString); // can throw if not exist
			_storage.Remove(id); // can also throw
			_dataIdentifiers.Value.Remove(localData); // cannot throw without crossthread access
		}
	}
}
