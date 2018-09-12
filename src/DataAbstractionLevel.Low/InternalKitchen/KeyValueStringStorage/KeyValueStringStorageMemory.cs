using System;
using System.Collections.Generic;
using DataAbstractionLevel.Low.InternalKitchen.KeyValueStringStorage.Contracts;

namespace DataAbstractionLevel.Low.InternalKitchen.KeyValueStringStorage {
	public class KeyValueStringStorageMemory : IKeyValueStringStorage {
		private readonly Dictionary<string, string> _records = new Dictionary<string, string>();
		public string GetValue(string key) {
			if (_records.ContainsKey(key)) {
				_records.Add(key, Guid.NewGuid().ToString());
			}
			return _records[key];
		}

		public void SetValue(string key, string value) {
			if (_records.ContainsKey(key)) {
				_records[key] = value;
			}
			else {
				_records.Add(key, value);
			}
		}

		public void Remove(string key) {
			if (_records.ContainsKey(key)) {
				_records.Remove(key);
			}
		}
	}
}