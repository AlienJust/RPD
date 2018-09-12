using System;
using System.Collections.Generic;
using System.Linq;

namespace RPD.DAL.LowLevel.Storage.PsnConfig {
	class StorageConfigurationPsnProtocolsRelayMemoryCache : IStorageConfigurationPsnProtocols {
		private readonly IStorageConfigurationPsnProtocols _relayStorage;
		private readonly Lazy<List<IConfigurationPsnProtocol>> _configurations;
		public StorageConfigurationPsnProtocolsRelayMemoryCache(IStorageConfigurationPsnProtocols relayStorage) {
			_relayStorage = relayStorage;
			_configurations = new Lazy<List<IConfigurationPsnProtocol>>(()=>_relayStorage.ConfigurationPsnProtocols.ToList());
		}

		public IEnumerable<IConfigurationPsnProtocol> ConfigurationPsnProtocols {
			get {
				return _configurations.Value;
			}
		}
	}
}