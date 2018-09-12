using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;
using RPD.Presentation.ViewModels.DalViewModels.DesignTime;

namespace RPD.Presentation.ViewModels.AddDataViewModels.DesignTime {
	class AvailableFaultsDesignTime : IAvailableFaultsViewModel {
		public AvailableFaultsDesignTime() {
			Locomotives = new ObservableCollection<ILocomotiveViewModel> { new LocomotiveDesignTime() };

			Sections = new ObservableCollection<ISectionViewModel> { new SectionDesignTime(Locomotives[0]) };
			
			Sections[0].PsnPowerOnLogs.Add(new PsnLogDesignTime { HasIntegrityErrors = true });
			Sections[0].PsnPowerOnLogs.Add(new PsnLogDesignTime());
			Sections[0].PsnPowerOnLogs.Add(new PsnLogDesignTime());
			Sections[0].PsnPowerOnLogs.Add(new PsnLogDesignTime());
			Sections[0].PsnPowerOnLogs.Add(new PsnLogDesignTime());

			Sections[0].PsnLogs.Add(new PsnLogDesignTime() { HasIntegrityErrors = true });
			Sections[0].PsnLogs[0].IsSavedToRepository = true;
			Sections[0].PsnLogs.Add(new PsnLogDesignTime());
			Sections[0].PsnLogs.Add(new PsnLogDesignTime());
			Sections[0].PsnLogs.Add(new PsnLogDesignTime());
			Sections[0].PsnLogs.Add(new PsnLogDesignTime());
			Sections[0].PsnLogs.Add(new PsnLogDesignTime());

			Sections[0].Faults.Add(new FaultDesignTime());
			Sections[0].Faults.Add(new FaultDesignTime());
			Sections[0].Faults.Add(new FaultDesignTime());
			Sections[0].Faults.Add(new FaultDesignTime());
			Sections[0].Faults.Add(new FaultDesignTime());

			NextCommand = new RelayCommand(Execute);

			SelectAllPsnPowerOnCommand = new RelayCommand(Execute);
			UnselectAllPsnPowerOnCommand = new RelayCommand(Execute);

			SelectAllPsnCommand = new RelayCommand(Execute);
			UnselectAllPsnCommand = new RelayCommand(Execute);

			SelectAllFaultsCommand = new RelayCommand(Execute);
			UnselectAllFaultsCommand = new RelayCommand(Execute);
		}

		private void Execute() {
			;
		}

		#region Implementation of IAvailableFaultsViewModel

		public ObservableCollection<ILocomotiveViewModel> Locomotives { get; set; }

		public ObservableCollection<ISectionViewModel> Sections { get; set; }

		public RelayCommand SelectAllPsnPowerOnCommand { get; set; }
		public RelayCommand UnselectAllPsnPowerOnCommand { get; set; }

		public RelayCommand SelectAllPsnCommand { get; set; }
		public RelayCommand UnselectAllPsnCommand { get; set; }

		public RelayCommand SelectAllFaultsCommand { get; set; }
		public RelayCommand UnselectAllFaultsCommand { get; set; }

		public RelayCommand NextCommand { get; set; }
		public RelayCommand BackCommand { get; set; }

		#endregion
	}
}
