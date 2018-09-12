using RPD.Presentation.Contracts.Model.SelectionMasks;

namespace RPD.Presentation.Model.SelectionMasks {
	/// <summary>
	/// Элемент в шаблоне выделения.
	/// </summary>
	internal class SelectionItem : ISelectionItem {
		/// <summary>
		/// Номер эелемента в списке.
		/// </summary>
		public int Number { get; set; }

		/// <summary>
		/// Название элемента.
		/// </summary>
		public string Name { get; set; }
	}
}
