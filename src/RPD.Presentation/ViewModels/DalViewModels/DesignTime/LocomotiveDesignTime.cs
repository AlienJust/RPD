using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using RPD.DAL;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;

namespace RPD.Presentation.ViewModels.DalViewModels.DesignTime {
	class LocomotiveDesignTime : ViewModelBase, ILocomotiveViewModel {
		public LocomotiveDesignTime() {
			Sections = new ObservableCollection<ISectionViewModel>();
			Name = "Локомотив 1";
			IsTreeViewExpanded = true;
			//Sections.Add(new SectionDesignTime());
		}

		public bool IsTreeViewExpanded { get; set; }

		#region Implementation of ILocomotiveViewModel

		public ILocomotive Locomotive { get; set; }
		public bool AllFaultsNew { set; private get; }
		public bool CaptureNewFaults { get; set; }
		public string Name { get; set; }
		public ObservableCollection<ISectionViewModel> Sections { get; set; }

		#endregion
	}
}
