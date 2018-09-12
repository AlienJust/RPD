
namespace RPD.Presentation.Contracts.ViewModels {
	/// <summary>
	/// Модель представления которую можно как-либо выделить (отметить) в UI.
	/// </summary>
	public interface ICheckableViewModel {
		bool IsChecked { get; set; }
	}
}
