using System.Collections.ObjectModel;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;

namespace RPD.Presentation.ViewModels.DalViewModels.DesignTime {
	internal class PsnMeterConfigDesignTime : IPsnMeterConfigViewModel {

		public string Name { get; set; }
		public ObservableCollection<IPsnChannelConfigViewModel> PsnChannelConfigs { get; private set; }
	}
}