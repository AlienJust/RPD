using System.Collections.Generic;
using System.Collections.ObjectModel;
using RPD.DAL;

namespace RPD.Presentation.Contracts.ViewModels.DalViewModels {
	public interface IPsnMeterViewModel : ITreeViewItemViewModel {
		IPsnMeter Model { get; }

		string Name { get; }
		ObservableCollection<IPsnChannelViewModel> PsnChannels { get; }
		IEnumerable<ITrendViewModel> Trends { get; }
	}
}