using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RPD.Presentation.Contracts;
using RPD.Presentation.Contracts.Model.SelectionMasks;
using RPD.Presentation.Contracts.ViewModels;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;
using RPD.Presentation.Model.SelectionMasks;
using RPD.Presentation.ViewModels.DalViewModels.DesignTime;
using RPD.SciChartControl.Events;
using PsnChannelDesignTime = RPD.Presentation.ViewModels.DalViewModels.DesignTime.PsnChannelDesignTime;
using PsnMeterDesignTime = RPD.Presentation.ViewModels.DalViewModels.DesignTime.PsnMeterDesignTime;

namespace RPD.Presentation.ViewModels {
	class MainViewModelDesign : ViewModelBase, IMainViewModel {
		private readonly RelayCommand _collapseTreeCmd;

		public MainViewModelDesign() {
			var colors = new ColorsStorage("");
			MainChart = new SciChartViewModel(colors);

			InitializeCommands();

			SelectionMasksStorage = new SelectionMasksStorage();

			IsTrendLoading = false;
			IsTrendNotLoading = true;
			IsRepositoryLoaded = true;
			IsExportDataMode = true;
			IsTreeViewExpanded = true;
			IsPopupOpen = false;
			IsDebugMode = true;

			Repository = new RepositoryDesignTime();

			var loco = new LocomotiveDesignTime();
			Repository.Locomotives.Add(loco);

			var section = new SectionDesignTime(loco);
			loco.Sections.Add(section);

			GenerateFault(section);
			GenerateFault(section);
			GeneratePsnLogDesignTime(section);
			_collapseTreeCmd = new RelayCommand(CollapseTree, ()=>true);
		}

		private void CollapseTree()
		{
			
		}

		private void InitializeCommands() {
			ShowAddDataCommand = new RelayCommand(Execute);
			ShowSettings = new RelayCommand(Execute);
			SaveSelectionMaskCommand = new RelayCommand(Execute);
			LoadSelectionMaskCommand = new RelayCommand<ISelectionMask>(LoadSelectionMaskCommandExecute);
			SetContextMenuTrendsContainerCommand = new RelayCommand<ITrendsContainer>(Execute);
			LoadAllTrendsCommand = new RelayCommand(Execute);
			Close = new RelayCommand(Execute);
			Initialize = new RelayCommand(Execute);
			ShowRpdConfigurator = new RelayCommand(Execute);
			TestCommand = new RelayCommand(Execute);
			ShowHelpWindow = new RelayCommand(Execute);
			RemoveAllRpdSignalsFromChart = new RelayCommand(Execute);
			RemoveAllPsnSignalsFromChart = new RelayCommand(Execute);
			RemoveAllPsnPowerOnSignalsFromChart = new RelayCommand(Execute);
			ExportDataCommand = new RelayCommand(Execute);
			RemoveDataCommand = new RelayCommand(Execute);
			ChangeConfigurationCommand = new RelayCommand(Execute);
			ShowDefaultColorSettings = new RelayCommand(Execute);
			ShowAboutProgramWindow = new RelayCommand(Execute);
			ShowIntegrityErrorsStatistics = new RelayCommand(Execute);
			SetContextMenuPsnLogCommand = new RelayCommand<IPsnLogViewModel>(Exc);
			SeriesColorChangedEventCommand = new RelayCommand<SeriesColorChangedEventArgs>(Fg);
			UncheckAllSignalsCommand = new RelayCommand(Execute);
		}

		private void KeyExecute(KeyEventArgs obj) {
			throw new System.NotImplementedException();
		}

		private void Fg(SeriesColorChangedEventArgs obj) {
			throw new System.NotImplementedException();
		}

		private void Exc(IPsnLogViewModel obj) {
			throw new System.NotImplementedException();
		}

		private void Execute(ITrendsContainer obj) {
			throw new System.NotImplementedException();
		}

		private void LoadSelectionMaskCommandExecute(ISelectionMask obj) {
			throw new System.NotImplementedException();
		}

		private void Execute() {
			throw new System.NotImplementedException();
		}

		private static void GeneratePsnLogDesignTime(SectionDesignTime section) {
			var log = new PsnLogDesignTime() { HasIntegrityErrors = true };

			var meter = new PsnMeterDesignTime();
			meter.PsnChannels.Add(new PsnChannelDesignTime());
			meter.PsnChannels.Add(new PsnChannelDesignTime());
			meter.PsnChannels.Add(new PsnChannelDesignTime());
			meter.PsnChannels.Add(new PsnChannelDesignTime());
			meter.PsnChannels.Add(new PsnChannelDesignTime());
			meter.PsnChannels.Add(new PsnChannelDesignTime());
			log.PsnMeters.Add(meter);

			section.PsnLogs.Add(log);
			section.PsnLogs.Add(new PsnLogDesignTime());
			section.PsnLogs.Add(new PsnLogDesignTime());
			section.PsnLogs.Add(new PsnLogDesignTime());
			section.PsnLogs.Add(new PsnLogDesignTime());
			section.PsnLogs.Add(new PsnLogDesignTime());
			section.PsnLogs.Add(new PsnLogDesignTime());
			section.PsnLogs.Add(new PsnLogDesignTime());
			section.PsnLogs.Add(new PsnLogDesignTime());
			section.PsnLogs.Add(new PsnLogDesignTime());
			section.PsnLogs.Add(new PsnLogDesignTime());
			section.PsnLogs.Add(new PsnLogDesignTime());
			section.PsnLogs.Add(new PsnLogDesignTime());
			section.PsnLogs.Add(new PsnLogDesignTime() { IsNew = true });
			section.PsnLogs.Add(new PsnLogDesignTime() { IsNew = true });
			section.PsnLogs.Add(new PsnLogDesignTime() { IsNew = true });
			section.PsnLogs.Add(new PsnLogDesignTime() { IsNew = true });
			section.PsnLogs.Add(new PsnLogDesignTime() { IsNew = true });
		}

		private static void GenerateFault(SectionDesignTime section) {
			var fault = new FaultDesignTime();
			section.Faults.Add(fault);

			var rpdMeter = new RpdMeterDesignTime("УРАН 1");
			fault.RpdMeters.Add(rpdMeter);
			rpdMeter.Channels.Add(new RpdChannelDesignTime("Канал 1"));
			rpdMeter.Channels.Add(new RpdChannelDesignTime("Канал 2"));
			rpdMeter.Channels.Add(new RpdChannelDesignTime("Канал 3"));
			rpdMeter.Channels.Add(new RpdChannelDesignTime("Канал 4"));
			rpdMeter.Channels.Add(new RpdChannelDesignTime("Канал 5"));
			rpdMeter.Channels.Add(new RpdChannelDesignTime("Канал 6"));

			var psnMeter = new PsnMeterDesignTime();
			fault.PsnLog.PsnMeters.Add(psnMeter);
			psnMeter.PsnChannels.Add(new PsnChannelDesignTime("Канал 1"));
			psnMeter.PsnChannels.Add(new PsnChannelDesignTime("Канал 2"));
			psnMeter.PsnChannels.Add(new PsnChannelDesignTime("Канал 3"));
			psnMeter.PsnChannels.Add(new PsnChannelDesignTime("Канал 4"));
			psnMeter.PsnChannels.Add(new PsnChannelDesignTime("Канал 5"));


			fault.Signals.Add(new SignalDesignTime("Сигнал 1"));
			fault.Signals.Add(new SignalDesignTime("Сигнал 2"));
			fault.Signals.Add(new SignalDesignTime("Сигнал 3"));
			fault.Signals.Add(new SignalDesignTime("Сигнал 4"));
		}


		public RelayCommand ShowAddDataCommand { get; set; }
		public RelayCommand ShowSettings { get; set; }
		public RelayCommand SaveSelectionMaskCommand { get; set; }
		public RelayCommand<ISelectionMask> LoadSelectionMaskCommand { get; set; }
		public RelayCommand<ITrendsContainer> SetContextMenuTrendsContainerCommand { get; set; }
		public RelayCommand LoadAllTrendsCommand { get; set; }
		public RelayCommand Close { get; set; }
		public RelayCommand ExportDataCommand { get; set; }
		public RelayCommand RemoveDataCommand { get; set; }
		public RelayCommand ChangeConfigurationCommand { get; set; }
		public RelayCommand Initialize { get; set; }
		public RelayCommand ShowRpdConfigurator { get; set; }
		public RelayCommand TestCommand { get; set; }
		public RelayCommand ShowHelpWindow { get; set; }
		public RelayCommand ShowDefaultColorSettings { get; set; }
		public RelayCommand ShowAboutProgramWindow { get; set; }
		public RelayCommand RemoveAllRpdSignalsFromChart { get; set; }
		public RelayCommand RemoveAllPsnSignalsFromChart { get; set; }
		public RelayCommand RemoveAllPsnPowerOnSignalsFromChart { get; set; }

		public RelayCommand UncheckAllSignalsCommand { get; set; }

		public RelayCommand ShowIntegrityErrorsStatistics { get; set; }
		public RelayCommand<IPsnLogViewModel> SetContextMenuPsnLogCommand { get; set; }
		public RelayCommand<SeriesColorChangedEventArgs> SeriesColorChangedEventCommand { get; set; }

		public IPsnLogViewModel ContextMenuPsnLog { get; set; }
		public bool IsTrendLoading { get; set; }
		public bool IsRepositoryLoaded { get; set; }
		public bool IsTrendNotLoading { get; private set; }
		public bool IsExportDataMode { get; set; }
		public bool IsRemoveDataMode { get; set; }
		public bool IsChangeConfigurationMode { get; set; }
		public bool IsExportDataModeAvailable { get; set; }
		public bool IsRemoveDataModeAvailable { get; set; }
		public bool IsChangeConfigurationModeAvailable { get; set; }
		public bool IsDataSelectMode { get; set; }
		public bool IsDebugMode { get; set; }
		public IRepositoryViewModel Repository { get; set; }
		public ISciChartViewModel MainChart { get; set; }
		public ISelectionMasksStorage SelectionMasksStorage { get; set; }
		public ITrendsContainer ContextMenuTrendsContainer { get; set; }
		public bool IsPopupOpen { get; set; }
		public bool IsTreeViewExpanded { get; set; }
		public object Test { get; set; }
		public ICommand CollapseTreeCmd => _collapseTreeCmd;
	}
}
