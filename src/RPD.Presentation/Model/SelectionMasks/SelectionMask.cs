using System.Collections.ObjectModel;
using System.Linq;
using RPD.Presentation.Contracts.Model.SelectionMasks;

namespace RPD.Presentation.Model.SelectionMasks {
	/// <summary>
	/// Шаблон выделения (набор элементов, которе можно выделить).
	/// </summary>
	internal class SelectionMask : ISelectionMask {
		public SelectionMask() {
			Items = new ObservableCollection<ISelectionItem>();
		}

		/// <summary>
		/// Название.
		/// </summary>
		public string Name {
			get {
				return Items.Aggregate("", (current, selectionItem) => current + selectionItem.Name + ", ").TrimEnd(',', ' ');
			}
		}

		/// <summary>
		/// Список элементов.
		/// </summary>
		public ObservableCollection<ISelectionItem> Items { get; set; }


		public bool ContainsItemNumber(int number) {
			return Items.Any(x => x.Number == number);
		}
	}
}
