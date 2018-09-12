using System.Collections.ObjectModel;
using System.Linq;
using RPD.Presentation.Contracts.Model.SelectionMasks;

namespace RPD.Presentation.Model.SelectionMasks {
	/// <summary>
	/// Коллекция шаблонов выделений.
	/// </summary>
	internal class SelectionMasksCollection : ISelectionMasksCollection {
		public SelectionMasksCollection() {
			Items = new ObservableCollection<ISelectionMask>();
		}

		/// <summary>
		/// Название коллекции.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Коллекция шаблонов.
		/// </summary>
		public ObservableCollection<ISelectionMask> Items { get; set; }


		public bool Contains(ISelectionMask mask) {
			return Items.Any(selectionMask => selectionMask.Name == mask.Name);
		}
	}
}
