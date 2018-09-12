using System;
using System.Collections.ObjectModel;
using RPD.DAL;
using GalaSoft.MvvmLight;
using RPD.Presentation.Contracts.ViewModels;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;

namespace RPD.Presentation.ViewModels.DalViewModels {
	/// <summary>
	/// Модель представления аварии.
	/// </summary>
	class FaultViewModel : ViewModelBase, IFaultViewModel {
		public FaultViewModel(IFaultLog fault) {
			Fault = fault;

			FillSignals();
			FillMeters();

			if (fault.PsnFaultLog != null)
				PsnLog = new PsnLogViewModel(fault.PsnFaultLog, TrendChartType.PsnFault);
		}


		#region Private Properties Fields

		private bool _isChecked = false;
		private bool _isNew = false;
		private bool _isSavedToRepository;

		#endregion

		#region Private Methods

		private void FillSignals() {
			foreach (var signal in Fault.Signals)
				Signals.Add(new SignalViewModel(signal));
		}

		private void FillMeters() {
			foreach (IRpdMeter t in Fault.RpdMeters)
				RpdMeters.Add(new RpdMeterViewModel(t, this));
		}

		#endregion


		#region Presentation Members

		public IFaultLog Fault { get; set; }

		public DateTime AccuredAt => Fault.AccuredAt;

		public string Name => Fault.Name + " " + Fault.AccuredAt.ToShortDateString() + " " + Fault.AccuredAt.ToLongTimeString();

		public string LogReason => Fault.LogReason;

		/// <summary>
		/// Список сигналов.
		/// </summary>
		public ObservableCollection<ISignalViewModel> Signals { get; } = new ObservableCollection<ISignalViewModel>();

		/// <summary>
		/// Список измерителей.
		/// </summary>
		public ObservableCollection<IRpdMeterViewModel> RpdMeters { get; } = new ObservableCollection<IRpdMeterViewModel>();


		public IPsnLogViewModel PsnLog { get; }

		/// <summary>
		/// Признак того, что авария "отмечена" (чекнута) в дереве.
		/// </summary>
		public bool IsChecked {
			get => _isChecked;
			set { Set(() => IsChecked, ref _isChecked, value); }
		}

		public bool IsSavedToRepository {
			get => _isSavedToRepository;
			set { Set(() => IsSavedToRepository, ref _isSavedToRepository, value); }
		}

		public bool IsEnabled => !Fault.SavedToRepository;

		/// <summary>
		/// Признак того что авария новая в списке аварий.
		/// </summary>
		public bool IsNew {
			get => _isNew;
			set { Set(() => IsNew, ref _isNew, value); }
		}

		#endregion //Presentation Members
	}
}
