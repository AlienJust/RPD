using System.Collections.ObjectModel;
using RPD.DAL;

namespace RPD.Presentation.Contracts.ViewModels.DalViewModels {
	public interface IPsnLogViewModel : ICheckableViewModel, ILogDataViewModel, ITreeViewItemViewModel {
		IPsnLog PsnLog { get; set; }

		/// <summary>
		/// Последний лог на устройстве.
		/// </summary>
		bool IsLastDeviceLog { get; }
		string Name { get; }
		string Label { get; set; }
		ObservableCollection<IPsnMeterViewModel> PsnMeters { get; }

		/// <summary>
		/// Имеются ли ошибки целостности
		/// </summary>
		bool HasIntegrityErrors { get; }

		string Hint { get; }

		bool IsInEditLabelMode { get; set; }
	}
}

