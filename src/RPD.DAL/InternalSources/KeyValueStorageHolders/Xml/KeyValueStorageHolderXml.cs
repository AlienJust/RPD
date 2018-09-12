using System.IO;
using DataAbstractionLevel.Low.InternalKitchen.KeyValueStringStorage;
using DataAbstractionLevel.Low.InternalKitchen.KeyValueStringStorage.Contracts;
using RPD.DAL.KeyValueStorageHolders.Contracts;
using RPD.Supports;

namespace RPD.DAL.KeyValueStorageHolders.Xml {
	class KeyValueStorageHolderXml : IKeyValueStorageHolder {
		public IKeyValueStringStorage GetStorage(string storageIdentifier) {
			return new KeyValueStringStorageXmlMemCache(Path.Combine(Support.GetAppDataDirectoryPathAndCreateItIfNeeded(), storageIdentifier + ".xml"));
		}
	}
}