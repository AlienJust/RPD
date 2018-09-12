using System.Linq;
using Dnv.Utils.Collections;
using RPD.DAL;
using System.Collections.ObjectModel;
using RPD.Presentation.Contracts.ViewModels;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;

namespace RPD.Presentation.ViewModels.DalViewModels {
	class PsnLogViewModel : TreeItemViewModelBase, IPsnLogViewModel {
		private readonly TrendChartType _trendChartType;
		public IPsnLog PsnLog { get; set; }

		bool _isChecked;

		ObservableCollectionsConnector<IPsnMeter, IPsnMeterViewModel> _psnMetersConnector;
		private bool _savedToRepository;
		private bool _hasIntegrityErrors;
		private bool _isNew;
		private bool _isInEditLabelMode;

		public PsnLogViewModel(IPsnLog psnLog, TrendChartType trendChartType) {
			_trendChartType = trendChartType;
			PsnLog = psnLog;

			UpdateHasIntergiryErrors();

			PsnLog.IsSomethingWrongWithLogChanged += (sender, args) => UpdateHasIntergiryErrors();
			FillPsnMeters(psnLog, trendChartType);

			_isInEditLabelMode = false;
		}

		private void UpdateHasIntergiryErrors() {
			_hasIntegrityErrors = PsnLog.LogIntegrity == PsnLogIntegrity.PagesFlowError;

			RaisePropertyChanged(() => HasIntegrityErrors);
		}

		private void FillPsnMeters(IPsnLog psnLog, TrendChartType trendChartType) {
			foreach (var psnMeter in psnLog.Meters)
				PsnMeters.Add(new PsnMeterViewModel(psnMeter, trendChartType));

			_psnMetersConnector = new ObservableCollectionsConnector<IPsnMeter, IPsnMeterViewModel>(psnLog.Meters, PsnMeters,
					OnConstructDestItem,
					OnGetDestItem);
		}

		private IPsnMeterViewModel OnConstructDestItem(IPsnMeter meter) {
			return new PsnMeterViewModel(meter, _trendChartType);
		}

		private IPsnMeterViewModel OnGetDestItem(IPsnMeter meter) {
			return PsnMeters.FirstOrDefault(e => e.Model == meter);
		}


		public bool IsChecked {
			get => _isChecked;
			set { Set(() => IsChecked, ref _isChecked, value); }
		}

		public bool IsSavedToRepository {
			get => _savedToRepository;
			set { Set(() => IsSavedToRepository, ref _savedToRepository, value); }
		}

		/// <summary>
		/// Признак того что авария новая в списке аварий.
		/// </summary>
		public bool IsNew {
			get => _isNew;
			set { Set(() => IsNew, ref _isNew, value); }
		}

		public bool IsLastDeviceLog => PsnLog.IsLastDeviceLog;

		public string Name {
			get {
				string name;
				if (PsnLog.BeginTime.HasValue) {
					name = PsnLog.BeginTime.Value.ToString("yyyy.MM.dd HH:mm:ss");
					if (PsnLog.EndTime.HasValue) {
						name += " - " + PsnLog.EndTime.Value.ToString("yyyy.MM.dd HH:mm:ss");
					}
				}
				else if (PsnLog.SaveTime.HasValue) name = "Сохранено " + PsnLog.SaveTime.Value.ToString("yyyy.MM.dd HH:mm:ss");
				else name = "Время неизвестно";
				return name;
			}
		}

		public string Label {
			get => PsnLog.Name;
			set {
				if (PsnLog.Name != value) {
					PsnLog.Update(PsnLog.PsnConfiguration, value);
					RaisePropertyChanged(() => Label);
				}
				IsInEditLabelMode = false;
			}
		}

		public ObservableCollection<IPsnMeterViewModel> PsnMeters { get; } = new ObservableCollection<IPsnMeterViewModel>();

		public bool HasIntegrityErrors {
			get => _hasIntegrityErrors;
			private set { Set(() => HasIntegrityErrors, ref _hasIntegrityErrors, value); }
		}

		public string Hint {
			get {
				var hint =
					"Конфигурация: " + PsnLog.PsnConfiguration.Name +
					"\nОписание: " + PsnLog.PsnConfiguration.Description +
					"\nВерсия: " + PsnLog.PsnConfiguration.Version +
					"\nID в БД: " + PsnLog.Id.UnicString +
					(PsnLog.LogIntegrity == PsnLogIntegrity.PagesFlowError ? "\nИмеются проблемы с целостностью лога" : "");

				return hint;
			}

		}


		public bool IsInEditLabelMode {
			get => _isInEditLabelMode;
			set {
				if (_isInEditLabelMode != value) {
					_isInEditLabelMode = value;
					RaisePropertyChanged(() => IsInEditLabelMode);
				}
			}
		}
	}
}