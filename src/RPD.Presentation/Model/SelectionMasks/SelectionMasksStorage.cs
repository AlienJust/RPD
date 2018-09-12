using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RPD.Presentation.Contracts.Model.SelectionMasks;

namespace RPD.Presentation.Model.SelectionMasks {
	public class SelectionMaskConverter : CustomCreationConverter<ISelectionMask> {
		public override ISelectionMask Create(Type objectType) {
			return new SelectionMask();
		}
	}

	public class SelectionItemConverter : CustomCreationConverter<ISelectionItem> {
		public override ISelectionItem Create(Type objectType) {
			return new SelectionItem();
		}
	}

	class SelectionMasksStorage : ISelectionMasksStorage {
		public SelectionMasksStorage() {
			SelectionMasks = new Dictionary<string, ISelectionMasksCollection>();
		}

		/// <summary>
		/// Таблица коллекция шаблонов выделений.
		/// string - название коллекции.
		/// </summary>
		public Dictionary<string, ISelectionMasksCollection> SelectionMasks { get; set; }

		public void Load(string fileName) {
			var dic = JsonConvert.DeserializeObject<Dictionary<string, SelectionMasksCollection>>(File.ReadAllText(fileName),
					new SelectionMaskConverter(), new SelectionItemConverter());

			SelectionMasks.Clear();
			foreach (var pair in dic) {
				SelectionMasks.Add(pair.Key, pair.Value);
			}
		}

		public void Save(string fileName) {
			var outputJson = JsonConvert.SerializeObject(SelectionMasks, Formatting.Indented);
			File.WriteAllText(fileName, outputJson);
		}

	}
}
