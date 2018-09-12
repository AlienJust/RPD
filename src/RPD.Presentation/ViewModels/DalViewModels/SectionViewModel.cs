using System.Collections.ObjectModel;
using System.Linq;
using Dnv.Utils.Collections;
using RPD.DAL;
using GalaSoft.MvvmLight;
using RPD.Presentation.Contracts.ViewModels;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;

namespace RPD.Presentation.ViewModels.DalViewModels {
	/// <summary>
	/// Модель представления секции локомотива.
	/// </summary>
	class SectionViewModel : ViewModelBase, ISectionViewModel {
		public SectionViewModel(ISection section, int number, ILocomotiveViewModel ownerLocomotive) {
			OwnerLocomotive = ownerLocomotive;
			CaptureNewFaults = false;
			Section = section;
			_number = number;

			FillFaults();

			FillPsnLogs();
		}


		#region Private Fields

		// Эти объекты связывают коллекции моделей и коллекции ViewModels для отслеживания появляения новых элементов и удаления
		private ObservableCollectionsConnector<IFaultLog, IFaultViewModel> _faultsLinker;
		private ObservableCollectionsConnector<IPsnLog, IPsnLogViewModel> _psnLogsLinker;
		private ObservableCollectionsConnector<IPsnLog, IPsnLogViewModel> _psnPowerOnLogsLinker;

		readonly int _number;

		#endregion


		#region Private Methods


		void FillFaults() {
			for (int i = 0; i < Section.Faults.Count; i++)
				Faults.Add(new FaultViewModel(Section.Faults[i]));

			_faultsLinker = new ObservableCollectionsConnector<IFaultLog, IFaultViewModel>(Section.Faults, Faults, ConstructNewFault,
					log => Faults.FirstOrDefault(e => e.Fault == log));
		}


		void FillPsnLogs() {
			foreach (var psnLog in Section.Psns) {
				if (psnLog.LogType == PsnLogType.FixedLength)
					PsnLogs.Add(new PsnLogViewModel(psnLog, TrendChartType.Psn));
				else if (psnLog.LogType == PsnLogType.PowerDepended)
					PsnPowerOnLogs.Add(new PsnLogViewModel(psnLog, TrendChartType.Psn));
			}

			_psnLogsLinker = new ObservableCollectionsConnector<IPsnLog, IPsnLogViewModel>
					(Section.Psns, PsnLogs, ConstructNewPsnLog, GetPsnLogsDestItem);

			_psnPowerOnLogsLinker = new ObservableCollectionsConnector<IPsnLog, IPsnLogViewModel>
					(Section.Psns, PsnPowerOnLogs, ConstructNewPsnPowerOnLog, GetPsnPowerOnLogDestItem);
		}

		private IPsnLogViewModel GetPsnLogsDestItem(IPsnLog psnLog) {
			var t = PsnLogs.FirstOrDefault(e => e.PsnLog == psnLog);
			return t;
		}

		private IPsnLogViewModel GetPsnPowerOnLogDestItem(IPsnLog psnLog) {
			var t = PsnPowerOnLogs.FirstOrDefault(e => e.PsnLog == psnLog);
			return t;
		}


		PsnLogViewModel ConstructNewPsnLog(IPsnLog psnLog) {
			if (psnLog.LogType == PsnLogType.FixedLength)
				return new PsnLogViewModel(psnLog, TrendChartType.Psn) {
					IsNew = CaptureNewFaults
				};

			return null;
		}

		PsnLogViewModel ConstructNewPsnPowerOnLog(IPsnLog psnLog) {
			if (psnLog.LogType == PsnLogType.PowerDepended)
				return new PsnLogViewModel(psnLog, TrendChartType.Psn) {
					IsNew = CaptureNewFaults
				};

			return null;
		}

		/// <summary>
		/// Вызывается при добавленнии новой аварии в список аварий в модели данных (ISection.Faults).
		/// </summary>
		/// <param name="fault">Авария.</param>
		/// <returns>Модель представления аварии.</returns>
		FaultViewModel ConstructNewFault(IFaultLog fault) {
			return new FaultViewModel(fault) {
				IsNew = CaptureNewFaults
			};
		}

		#endregion // Private Methods



		#region Private Properties Fields

		private ObservableCollection<IFaultViewModel> _faults = new ObservableCollection<IFaultViewModel>();
		private ObservableCollection<IPsnLogViewModel> _psnLogs = new ObservableCollection<IPsnLogViewModel>();
		private ObservableCollection<IPsnLogViewModel> _psnPowerOnLogs = new ObservableCollection<IPsnLogViewModel>();

		#endregion



		#region Public Properties

		public ISection Section { get; }

		/// <summary>
		/// Если true, то у всех аварий на секции будет выставлен флаг IsNew.
		/// </summary>
		public bool AllFaultsNew {
			set {
				foreach (var fault in Faults)
					fault.IsNew = value;
			}
		}

		/// <summary>
		/// Если true, то у всех аварий, которые добаляются в 
		/// список аварий будет выставлен флаг IsNew.
		/// </summary>
		public bool CaptureNewFaults { get; set; }

		public string Name => "Секция " + _number;

		public ILocomotiveViewModel OwnerLocomotive { get; }

		/// <summary>
		/// Список аварий на секции.
		/// </summary>
		public ObservableCollection<IFaultViewModel> Faults {
			get => _faults;
			set { Set(() => Faults, ref _faults, value); }
		}

		/// <summary>
		/// Список логов ПСН (PsnLogType.FixedLength).
		/// </summary>
		public ObservableCollection<IPsnLogViewModel> PsnLogs {
			get => _psnLogs;
			set { Set(() => PsnLogs, ref _psnLogs, value); }
		}

		/// <summary>
		/// Список логов ПСН по включению (PsnLogType.PowerDepended).
		/// </summary>
		public ObservableCollection<IPsnLogViewModel> PsnPowerOnLogs {
			get => _psnPowerOnLogs;
			set { Set(() => PsnPowerOnLogs, ref _psnPowerOnLogs, value); }
		}

		#endregion // Public Properties

		public bool IsTreeViewExpanded { get; set; }
	}
}
