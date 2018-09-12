using System.Collections.ObjectModel;
using RPD.DAL;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;

namespace RPD.Presentation.ViewModels.DalViewModels.DesignTime {
	internal class PsnConfigurationDesignTime : IPsnConfigurationViewModel {
		public PsnConfigurationDesignTime() {
			PsnMeterConfigs = new ObservableCollection<IPsnMeterConfigViewModel>();
		}
		public IPsnConfiguration Model { get; set; }
		public string Version { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string DescriptionAndVersionString { get; set; }
		public ObservableCollection<IPsnMeterConfigViewModel> PsnMeterConfigs { get; private set; }
	}
}