using System;
using System.Windows.Forms;
using Dnv.Utils.Messages;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RPD.Presentation.Messages;
using RPD.Presentation.Settings;

namespace RPD.Presentation.ViewModels.AddDataViewModels.SelectPathHelperViewModels {
	/// <summary>
	/// Выбор папки c данными РПД.
	/// Generates Messages:
	/// AppMessages.AddDataWindow.SelectDataSourcePage, 
	/// AppMessages.AddDataWindow.ReadProgressPage,
	/// AppMessages.ShowSelectRdpDataPathDialog
	/// </summary>
	internal class SelectRpdDataDirectoryHelper : ISelectPathHelperViewModel {
		private readonly IMessenger _messenger;
		private readonly IApplicationSettings _applicationSettings;

		public SelectRpdDataDirectoryHelper(IMessenger messenger, IApplicationSettings applicationSettings) {
			_messenger = messenger;
			_applicationSettings = applicationSettings;

			BackCommand = new RelayCommand(() => /*_messenger.Send(AddDataWindowMessages.SelectDataSourcePage)*/
					_messenger.Send(new NavigateMessage<AddDataWindowMessages>(NavigateDirection.Backward,
							AddDataWindowMessages.SelectFolderPage,
							AddDataWindowMessages.SelectDataSourcePage)));

			NextCommand = new RelayCommand(() => /*_messenger.Send(AddDataWindowMessages.SelectPsnConfiguration)*/
					_messenger.Send(new NavigateMessage<AddDataWindowMessages>(NavigateDirection.Forward,
							AddDataWindowMessages.SelectFolderPage,
							AddDataWindowMessages.SelectPsnConfiguration)));
		}

		#region Implementation of ISelectPathHelperViewModel

		public string ToolTipText => "Пожалуйста, укажите папку с данными РПД";

		public string Label => "Папка:";

		public string LastPath {
			get => _applicationSettings.LastAddDataFolder;
			set {
				_applicationSettings.LastAddDataFolder = value;
				_applicationSettings.Save();
			}
		}

		public RelayCommand BackCommand { get; }

		public RelayCommand NextCommand { get; }

		public void ShowBrowseDialog(Action<string> browseResult) {
			var msg = new DialogMessage<FolderBrowserDialog>(this,
					new FolderBrowserDialog {
						ShowNewFolderButton = false,
						SelectedPath = LastPath
					},
					result => {
						if (result.DialogResult == DialogResult.OK)
							browseResult(result.Dialog.SelectedPath);
					});

			_messenger.Send(msg, AppMessages.ShowSelectRdpDataPathDialog);
		}

		#endregion
	}
}
