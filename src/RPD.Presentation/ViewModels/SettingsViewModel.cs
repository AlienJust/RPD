using System.IO;
using System.Windows.Forms;
using System.Windows.Input;
using Dnv.Utils.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RPD.Presentation.Contracts.ViewModels;
using RPD.Presentation.Settings;
using RPD.Presentation.Utils.Messages;

namespace RPD.Presentation.ViewModels {
	/// <summary>
	/// Модель представления окна настроек.
	/// Generates messages: FolderBrowserDialog, AppMessages.CloseSettingsWindow.
	/// </summary>
	class SettingsViewModel : ViewModelBase, ISettingsViewModel {
		/// <summary>
		/// Предыдущее значение _applicationSettings.IsRepositoryPathSetted.
		/// </summary>
		readonly bool _isRepositoryPathSettedOld;

		private readonly IMessenger _messenger;
		private readonly IApplicationSettings _applicationSettings;

		#region Private Properties Fields

		private string _repositoryPath = "";
		private bool _isRepositoryPathSetted = false;
		private string _statusString = "";

		#endregion

		public SettingsViewModel(IApplicationSettings applicationSettings, IMessenger messenger) {
			_applicationSettings = applicationSettings;
			_messenger = messenger;

			if (_applicationSettings.IsRepositoryPathSetted) {
				RepositoryPath = _applicationSettings.RepositoryPath;
				if (Directory.Exists(_applicationSettings.RepositoryPath))
					_isRepositoryPathSettedOld = _applicationSettings.IsRepositoryPathSetted;
			}
			else {
				// Чтобы сработал сеттер.
				RepositoryPath = string.Empty;
			}

			InitializeCommands();
		}

		void InitializeCommands() {
			BrowseRepositoryPath = new RelayCommand(BrowseRepositoryPathExecute);
			OK = new RelayCommand(OkExecute, OkCanExecute);
			Cancel = new RelayCommand(CancelExecute, CanCancel);
		}

		#region Presentation Members   

		/// <summary>
		/// Путь к хранилищу
		/// </summary>
		public string RepositoryPath {
			get => _repositoryPath;

			set {
				if (value.Length == 0) {
					IsRepositoryPathSetted = false;
				}
				else if (!Directory.Exists(value)) {
					IsRepositoryPathSetted = false;
					StatusString = "Указанная папка не существует.";
				}
				else
					IsRepositoryPathSetted = true;

				Set(() => RepositoryPath, ref _repositoryPath, value);
				CommandManager.InvalidateRequerySuggested();
			}
		}



		/// <summary>
		/// Установлена ли директория хранилища.
		/// </summary>
		public bool IsRepositoryPathSetted {
			get { return _isRepositoryPathSetted; }

			set {
				if (value == false)
					StatusString = "Укажите папку хранилища.";
				else
					StatusString = "";

				Set(() => IsRepositoryPathSetted, ref _isRepositoryPathSetted, value);
			}
		}



		/// <summary>
		/// Текст строки статуса.
		/// </summary>
		public string StatusString {
			get { return _statusString; }

			set {
				Set(() => StatusString, ref _statusString, value);
				RaisePropertyChanged(() => IsStatusStringVisible);
			}
		}



		/// <summary>
		/// Видимость строки статуса.
		/// </summary>
		public bool IsStatusStringVisible {
			get { return StatusString.Length > 0; }
		}

		#endregion //Presentation Members


		#region Commands

		public RelayCommand BrowseRepositoryPath { get; set; }

		void BrowseRepositoryPathExecute() {
			// отображаем диалог выбора папки.
			var dialog = new FolderBrowserDialog() {
				ShowNewFolderButton = true,
				SelectedPath = RepositoryPath
			};

			_messenger.Send(new DialogMessage<FolderBrowserDialog>(this, dialog, BrowseResult), Views.Views.Settings);
		}


		public RelayCommand OK { get; set; }

		private void OkExecute() {
			_applicationSettings.IsRepositoryPathSetted = IsRepositoryPathSetted;
			_applicationSettings.RepositoryPath = RepositoryPath;
			_applicationSettings.Save();

			CloseView();
		}

		private bool OkCanExecute() {
			return IsRepositoryPathSetted;
		}


		public RelayCommand Cancel { get; set; }

		public bool CanClose() {
			return _applicationSettings.IsRepositoryPathSetted;
		}

		void CancelExecute() {
			CloseView();
		}

		private bool CanCancel() {
			return _isRepositoryPathSettedOld;
		}


		#endregion //Commands

		private void CloseView() {
			_messenger.Send(new ViewMessage(ViewAction.Close), Views.Views.Settings);
		}

		/// <summary>
		/// Результат нажатия на кнопку "Обзор..."
		/// </summary>
		/// <param name="result"></param>
		private void BrowseResult(DialogMessage<FolderBrowserDialog>.DialogMessageResult result) {
			// Путь к хранилищу вообще не задан.
			if (!_applicationSettings.IsRepositoryPathSetted && result.DialogResult != DialogResult.OK)
				IsRepositoryPathSetted = false;

			if (result.DialogResult == DialogResult.OK) {
				IsRepositoryPathSetted = true;
				RepositoryPath = result.Dialog.SelectedPath;
			}
		}
	}
}