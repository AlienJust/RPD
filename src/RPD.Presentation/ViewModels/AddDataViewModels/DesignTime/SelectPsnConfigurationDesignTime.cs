using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;
using PsnConfigurationDesignTime = RPD.Presentation.ViewModels.DalViewModels.DesignTime.PsnConfigurationDesignTime;

namespace RPD.Presentation.ViewModels.AddDataViewModels.DesignTime {
	internal class SelectPsnConfigurationDesignTime : ISelectPsnConfigurationViewModel {
		public SelectPsnConfigurationDesignTime() {
			AvailableConfigurations.Add(new PsnConfigurationDesignTime() { Version = "1", Name = "ТРАМВАЙ", Description = "Трамвайная прошивка" });
			AvailableConfigurations.Add(new PsnConfigurationDesignTime() { Version = "1.1", Name = "Локомотив", Description = "Локомотивная прошивка" });
			AvailableConfigurations.Add(new PsnConfigurationDesignTime() { Version = "2", Name = "Локомотив РП", Description = "Локомотивная прошивка РП" });
		}

		public ObservableCollection<IPsnConfigurationViewModel> AvailableConfigurations { get; private set; }
		public IPsnConfigurationViewModel SelectedConfiguration { get; set; }

		public RelayCommand NextCommand => throw new System.NotImplementedException();

		public RelayCommand BackCommand => throw new System.NotImplementedException();
	}
}