using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using RPD.DAL;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;

namespace RPD.Presentation.ViewModels.DalViewModels.DesignTime {
	class FaultDesignTime : ViewModelBase, IFaultViewModel {
		public FaultDesignTime() {
			PsnLog = new PsnLogDesignTime();
			Signals = new ObservableCollection<ISignalViewModel>();
			RpdMeters = new ObservableCollection<IRpdMeterViewModel>();

			AccuredAt = DateTime.Now;
		}

		#region Implementation of ICheckableViewModel

		public bool IsChecked { get; set; }

		#endregion

		#region Implementation of IFaultViewModel

		public IFaultLog Fault { get; set; }
		public DateTime AccuredAt { get; }

		public string Name => AccuredAt.ToShortDateString() + " " + AccuredAt.ToLongTimeString();

		public string LogReason { get; private set; }
		public ObservableCollection<ISignalViewModel> Signals { get; }
		public ObservableCollection<IRpdMeterViewModel> RpdMeters { get; }

		public IPsnLogViewModel PsnLog { get; }
		public bool IsSavedToRepository { get; set; }
		public bool IsEnabled { get; private set; }
		public bool IsNew { get; set; }

		#endregion
	}
}
