using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight.Messaging;
using RPD.DAL;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;
using RPD.Presentation.Messages;
using RPD.Presentation.ViewModels.DalViewModels;

namespace RPD.Presentation.ViewModels.AddDataViewModels {
	/// <summary>
	/// Модель представления списка доступных аварий.
	/// </summary>
	internal interface IAvailableFaultsViewModel {
		ObservableCollection<ILocomotiveViewModel> Locomotives { get; set; }

		ObservableCollection<ISectionViewModel> Sections { get; set; }

		RelayCommand SelectAllPsnPowerOnCommand { get; set; }
		RelayCommand UnselectAllPsnPowerOnCommand { get; set; }

		RelayCommand SelectAllPsnCommand { get; set; }
		RelayCommand UnselectAllPsnCommand { get; set; }

		RelayCommand SelectAllFaultsCommand { get; set; }
		RelayCommand UnselectAllFaultsCommand { get; set; }

		RelayCommand NextCommand { get; set; }
		RelayCommand BackCommand { get; set; }
	}

	/// <summary>
	/// Список логов на устройстве.
	/// </summary>
	class AvailableFaultsViewModel : ViewModelBase, IAvailableFaultsViewModel {
		private readonly IAddDataParameters _addDataParameters;
		private readonly Action _navigateNextPageAction;
		private readonly IMessenger _messenger;
		private readonly IRepository _localRepository;

		private ObservableCollection<ILocomotiveViewModel> _locomotives;
		private ObservableCollection<ISectionViewModel> _sections = new ObservableCollection<ISectionViewModel>();


		/// <param name="messenger"></param>
		/// <param name="locomotives"></param>
		/// <param name="addDataParameters"></param>
		/// <param name="localRepository"> </param>
		/// <param name="navigateNextPageAction">Функция осуществляющая навигацию на следующую страницу.</param>
		/// <param name="isSimpleMode"></param>
		public AvailableFaultsViewModel(IMessenger messenger, IList<ILocomotive> locomotives, IAddDataParameters addDataParameters,
				IRepository localRepository, Action navigateNextPageAction, bool isSimpleMode) {
			_messenger = messenger;
			_localRepository = localRepository;

			_addDataParameters = addDataParameters;
			_navigateNextPageAction = navigateNextPageAction;

			FillLocomotivesAndSections(locomotives);

			InitializeCommands();

			if (!isSimpleMode)
				SetIsSavedToRepository();
		}

		private void InitializeCommands() {
			NextCommand = new RelayCommand(NextCommandExecute, NextCommandCanExecute);
			BackCommand = new RelayCommand(BackCommandExecute);

			SelectAllPsnPowerOnCommand = new RelayCommand(SelectAllPsnPowerOnCommandExecute);
			UnselectAllPsnPowerOnCommand = new RelayCommand(UnselectAllPsnPowerOnCommandExecute);
			SelectAllPsnCommand = new RelayCommand(SelectAllPsnCommandExecute);
			UnselectAllPsnCommand = new RelayCommand(UnselectAllPsnCommandExecute);
			SelectAllFaultsCommand = new RelayCommand(SelectAllFaultsCommandExecute);
			UnselectAllFaultsCommand = new RelayCommand(UnselectAllFaultsCommandExecute);
		}



		private void SetIsSavedToRepository() {
			foreach (var section in Sections) {
				foreach (var psnLog in section.PsnPowerOnLogs)
					psnLog.IsSavedToRepository = _localRepository.IsExist(psnLog.PsnLog);

				foreach (var psnLog in section.PsnLogs)
					psnLog.IsSavedToRepository = _localRepository.IsExist(psnLog.PsnLog);

				foreach (var fault in section.Faults)
					fault.IsSavedToRepository = _localRepository.IsExist(fault.Fault);
			}
		}

		private void FillLocomotivesAndSections(IEnumerable<ILocomotive> locomotives) {
			_locomotives = new ObservableCollection<ILocomotiveViewModel>();
			foreach (var locomotive in locomotives) {
				var loco = new LocomotiveViewModel(locomotive);
				_locomotives.Add(loco);

				for (int i = 0; i < locomotive.Sections.Count; i++)
					Sections.Add(new SectionViewModel(locomotive.Sections[i], i + 1, loco));
			}
		}

		#region Private Methods

		private void UnselectAllFaultsCommandExecute() {
			foreach (var sectionViewModel in Sections)
				foreach (var log in sectionViewModel.Faults)
					log.IsChecked = false;
		}

		private void SelectAllFaultsCommandExecute() {
			foreach (var sectionViewModel in Sections)
				foreach (var log in sectionViewModel.Faults)
					log.IsChecked = true;
		}

		private void UnselectAllPsnCommandExecute() {
			foreach (var sectionViewModel in Sections)
				foreach (var log in sectionViewModel.PsnLogs)
					log.IsChecked = false;
		}

		private void SelectAllPsnCommandExecute() {
			foreach (var sectionViewModel in Sections)
				foreach (var log in sectionViewModel.PsnLogs)
					log.IsChecked = true;
		}

		private void UnselectAllPsnPowerOnCommandExecute() {
			foreach (var sectionViewModel in Sections)
				foreach (var log in sectionViewModel.PsnPowerOnLogs)
					log.IsChecked = false;
		}

		private void SelectAllPsnPowerOnCommandExecute() {
			foreach (var sectionViewModel in Sections)
				foreach (var log in sectionViewModel.PsnPowerOnLogs)
					log.IsChecked = true;
		}

		void NextCommandExecute() {
			foreach (var sectionViewModel in Sections) {
				_addDataParameters.FaultsToRead.AddRange(sectionViewModel.Faults.
						Where(f => f.IsChecked).
						Select(f => f.Fault));

				_addDataParameters.PsnLogsToRead.AddRange(sectionViewModel.PsnLogs.
						Where(l => l.IsChecked).
						Select(l => l.PsnLog));

				_addDataParameters.PsnLogsToRead.AddRange(sectionViewModel.PsnPowerOnLogs.
						Where(l => l.IsChecked).
						Select(l => l.PsnLog));
			}

			_navigateNextPageAction();
		}

		private void BackCommandExecute() {
			_messenger.Send(new NavigateMessage<AddDataWindowMessages>(NavigateDirection.Backward,
					from: AddDataWindowMessages.AvailableFaultsPage));
		}

		bool NextCommandCanExecute() {
			foreach (var sectionViewModel in Sections) {
				if (sectionViewModel.Faults.Any(f => f.IsChecked) ||
						sectionViewModel.PsnLogs.Any(p => p.IsChecked) ||
						sectionViewModel.PsnPowerOnLogs.Any(p => p.IsChecked))
					return true;
			}

			return false;
		}

		#endregion Private Methods


		#region Presentation Members

		public ObservableCollection<ILocomotiveViewModel> Locomotives {
			get => _locomotives;
			set { Set(() => Locomotives, ref _locomotives, value); }
		}

		public ObservableCollection<ISectionViewModel> Sections {
			get => _sections;
			set { Set(() => Sections, ref _sections, value); }
		}

		#endregion //Presentation Members


		#region Commands

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