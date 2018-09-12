using System;
using System.Collections.Generic;
using System.Linq;

namespace RPD.DAL.LowLevel.Storage.PsnConfig {
	class StorageConfigurationPsnProtocolCommandsRelayMemoryCache : IStorageConfigurationPsnProtocolCommands
	{
		private readonly IStorageConfigurationPsnProtocolCommands _relayStorage;
		private readonly Lazy<List<IConfigurationPsnProtocolCommand>> _configurations;
		public StorageConfigurationPsnProtocolCommandsRelayMemoryCache(IStorageConfigurationPsnProtocolCommands relayStorage)
		{
			_relayStorage = relayStorage;
			_configurations = new Lazy<List<IConfigurationPsnProtocolCommand>>(() => _relayStorage.ConfigurationPsnProtocolCommands.ToList());
		}

		public IEnumerable<IConfigurationPsnProtocolCommand> ConfigurationPsnProtocolCommands
		{
			get
			{
				return _configurations.Value;
			}
		}
	}
}