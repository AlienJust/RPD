using System;
using System.Collections.Generic;
using System.Linq;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.PsnConfig.Contracts;

namespace DataAbstractionLevel.Low.Storage.PsnConfigurationStorage {
	public class PsnConfigurationStorageLazyReadonlyRelay : IStorage<IPsnProtocolConfiguration> {
		private readonly IStorage<IPsnProtocolConfiguration> _relayStorage;
		private readonly Lazy<List<IPsnProtocolConfiguration>> _lazyStoredItems; 
		public PsnConfigurationStorageLazyReadonlyRelay(IStorage<IPsnProtocolConfiguration> relayStorage)
		{
			_relayStorage = relayStorage;
			_lazyStoredItems = new Lazy<List<IPsnProtocolConfiguration>>(()=>_relayStorage.StoredItems.ToList());
		}


		public void Add(IPsnProtocolConfiguration item)
		{
			throw new System.NotImplementedException();
		}

		public void Remove(IPsnProtocolConfiguration item)
		{
			throw new System.NotImplementedException();
		}

		public void Update(IIdentifier id, IPsnProtocolConfiguration item)
		{
			throw new System.NotImplementedException();
		}

		public IEnumerable<IPsnProtocolConfiguration> StoredItems
		{
			get { return _lazyStoredItems.Value; }
		}
	}
}