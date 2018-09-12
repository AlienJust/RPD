using System.Collections.Generic;
using System.Linq;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.Contracts;

namespace DataAbstractionLevel.Low.Storage.PsnDataCustomConfigStorage
{
	public class PsnDataCustomConfigurationsStoragePredefined : IPsnDataCustomConfigurationsStorage {
		private readonly List<IPsnDataCustomConfigration> _configs;

		public PsnDataCustomConfigurationsStoragePredefined(IEnumerable<IPsnDataCustomConfigration> configs) {
			_configs = configs.ToList();
		}

		public IEnumerable<IPsnDataCustomConfigration> Configurations {
			get { return _configs; }
		}

		public void Add(IIdentifier psnDataCustomConfigurationId, IIdentifier psnConfigruationId, string customLogName)
		{
		}

		public void Update(IIdentifier psnDataCustomConfigurationId, IIdentifier setPsnConfigruationId, string setCustomLogName)
		{
		}

		public void Remove(IIdentifier psnDataCustomConfigurationId)
		{
		}
	}
}
