namespace DataAbstractionLevel.Low.InternalKitchen.KeyValueStringStorage.Contracts {
	public interface IKeyValueStringStorage {
		string GetValue(string key);
		void SetValue(string key, string value);
		void Remove(string key);
	}
}