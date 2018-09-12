using DataAbstractionLevel.Low.InternalKitchen.KeyValueStringStorage.Contracts;

namespace RPD.DAL.KeyValueStorageHolders.Contracts {
	// Holds a numerous of storages that available by theirs identifiers
	internal interface IKeyValueStorageHolder {
		IKeyValueStringStorage GetStorage(string storageIdentifier);
	}
}