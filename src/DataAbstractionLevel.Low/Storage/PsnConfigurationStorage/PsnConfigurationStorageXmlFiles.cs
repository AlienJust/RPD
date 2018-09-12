using System.Collections.Generic;
using System.IO;
using System.Linq;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.PsnConfig;
using DataAbstractionLevel.Low.PsnConfig.Contracts;

namespace DataAbstractionLevel.Low.Storage.PsnConfigurationStorage
{
	public class PsnConfigurationStorageXmlFiles : IStorage<IPsnProtocolConfiguration> {
		private readonly string _directory;
		private readonly string _filesMask;
		public PsnConfigurationStorageXmlFiles(string directory, string filesMask) {
			_directory = directory;
			_filesMask = filesMask;
		}


		public void Add(IPsnProtocolConfiguration item) {
			throw new System.NotImplementedException();
		}

		public void Remove(IPsnProtocolConfiguration item) {
			throw new System.NotImplementedException();
		}

		public void Update(IIdentifier id, IPsnProtocolConfiguration item) {
			throw new System.NotImplementedException();
		}

		public IEnumerable<IPsnProtocolConfiguration> StoredItems {
			get {
				var files = new DirectoryInfo(_directory).GetFiles(_filesMask);
				return files.Select(fileInfo => new PsnProtocolXmlSerializer(fileInfo.FullName).LoadProtocol());
			}
		}
	}
}
