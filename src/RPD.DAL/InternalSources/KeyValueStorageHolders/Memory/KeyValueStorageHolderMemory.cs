using DataAbstractionLevel.Low.InternalKitchen.KeyValueStringStorage;
using DataAbstractionLevel.Low.InternalKitchen.KeyValueStringStorage.Contracts;
using RPD.DAL.KeyValueStorageHolders.Contracts;

namespace RPD.DAL.KeyValueStorageHolders.Memory {
	class KeyValueStorageHolderMemory : IKeyValueStorageHolder {
		public IKeyValueStringStorage GetStorage(string storageIdentifier) {
			return new KeyValueStringStorageMemory();
		}
	}
}