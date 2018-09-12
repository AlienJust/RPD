using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using RPD.Presentation.ViewModels.AddDataViewModels.SelectPathHelperViewModels;

namespace RPD.Presentation.ViewModels.AddDataViewModels {
	internal interface ISelectPathViewModel {
		string Label { get; }

		string ToolTipText { get; }

		/// <summary>
		/// Путь к папке.
		/// </summary>
		string Path { get; set; }

		/// <summary>
		/// Выбрана ли папка..
		/// </summary>
		bool IsPathSet { get; }

		RelayCommand BackCommand { get; set; }
		RelayCommand NextCommand { get; set; }
		RelayCommand BrowseCommand { get; set; }
	}

	/// <summary>
	/// Модель представления окна выбора папки содежащей данные РПД.
	/// </summary>
	class SelectPathViewModel : ViewModelBase, ISelectPathViewModel {
		private readonly IAddDataParameters _addDataParameters;
		private readonly ISelectPathHelperViewModel _selectPathHelper;

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public SelectPathViewModel(IAddDataParameters addDataParameters,
				ISelectPathHelperViewModel selectPathHelper) {
			_addDataParameters = addDataParameters;
			_selectPathHelper = selectPathHelper;

			_toolTipText = _selectPathHelper.ToolTipText;
			_label = _selectPathHelper.Label;

			Path = _selectPathHelper.LastPath;

			InitializeCommands();
		}

		void InitializeCommands() {
			BackCommand = new RelayCommand(() => _selectPathHelper.BackCommand.Execute(null),
					() => _selectPathHelper.BackCommand.CanExecute(null));

			NextCommand = new RelayCommand(() => _selectPathHelper.NextCommand.Execute(null),
					() => _selectPathHelper.NextCommand.CanExecute(null));

			BrowseCommand = new RelayCommand(BrowseCommandExecute);
		}

		#region View Model Properties

		public string Label {
			get { return _label; }
		}


		public string ToolTipText {
			get { return _toolTipText; }
		}


		private string _path = "";

		/// <summary>
		/// Путь к папке.
		/// </summary>
		public string Path {
			get { return _path; }
			set {
				if (_path == value)
					return;

				_path = value;

				if (string.IsNullOrEmpty(value))
					IsPathSet = false;
				else {
					_addDataParameters.DevicePath = value;
					_selectPathHelper.LastPath = value;

					IsPathSet = true;
				}

				// Обновляем биндинги команд во View. Нужно что NextCommandCanExecute очухался.
				CommandManager.InvalidateRequerySuggested();
				RaisePropertyChanged(() => Path);
			}
		}


		private bool _isPathSet = false;
		private readonly string _label;
		private readonly string _toolTipText;

		/// <summary>
		/// Выбрана ли папка..
		/// </summary>
		public bool IsPathSet {
			get { return _isPathSet; }
			private set { Set(() => IsPathSet, ref _isPathSet, value); }
		}

		#endregion // View Model Properties


		#region Commands

		public RelayCommand BackCommand { get; set; }

		public RelayCommand NextCommand { get; set; }

		public RelayCommand BrowseCommand { get; set; }

		void BrowseCommandExecute() {
			_selectPathHelper.ShowBrowseDialog((path) => Path = path);
		}

		#endregion
	}
}