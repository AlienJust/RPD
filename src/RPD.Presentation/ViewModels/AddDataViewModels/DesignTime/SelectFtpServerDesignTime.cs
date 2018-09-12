using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RPD.Presentation.Model;
using RPD.Presentation.ViewModels.DalViewModels;

namespace RPD.Presentation.ViewModels.AddDataViewModels.DesignTime {
	class SelectFtpServerDesignTime : ViewModelBase, ISelectFtpServerViewModel {
		public SelectFtpServerDesignTime() {
			Servers = new ObservableCollection<FtpServerViewModel>(new List<FtpServerViewModel> {
				new FtpServerViewModel(new FtpServer {
					Name = "Горизонт"})});

			SelectedServer = Servers[0];

			AddServerCommand = new RelayCommand(() => { });
			EditServerCommand = new RelayCommand(() => { });
			DeleteServerCommand = new RelayCommand(() => { });
			NextCommand = new RelayCommand(() => { });
			BackCommand = new RelayCommand(() => { });
		}

		public ObservableCollection<FtpServerViewModel> Servers { get; }
		public FtpServerViewModel SelectedServer { get; set; }
		public RelayCommand AddServerCommand { get; }
		public RelayCommand EditServerCommand { get; }
		public RelayCommand DeleteServerCommand { get; }
		public RelayCommand NextCommand { get; }
		public RelayCommand BackCommand { get; }
	}
}
