using System;
using System.Windows.Forms;
using Dnv.Utils.Messages;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RPD.Presentation.Messages;
using RPD.Presentation.Properties;
using RPD.Presentation.Settings;

namespace RPD.Presentation.ViewModels.AddDataViewModels.SelectPathHelperViewModels {
	/// <summary>
	/// Выбор файла, который содержит данные РПД для импорта.
	/// Generates Messages:
	/// AppMessages.AddDataWindow.SelectDataSourcePage,
	/// AppMessages.AddDataWindow.ReadProgressPage,
	/// AppMessages.ShowSelectRdpDataPathDialog
	/// </summary>
	internal class SelectZippedRepositoryPathHelper : ISelectPathHelperViewModel {
		private readonly IMessenger _messenger;

		private readonly IApplicationSettings _applicationSettings;

		public SelectZippedRepositoryPathHelper(IMessenger messenger, IApplicationSettings applicationSettings) {
			_messenger = messenger;
			_applicationSettings = applicationSettings;

			BackCommand = new RelayCommand(() => /*_messenger.Send(AddDataWindowMessages.SelectDataSourcePage)*/
					_messenger.Send(new NavigateMessage<AddDataWindowMessages>(NavigateDirection.Backward,
							AddDataWindowMessages.SelectZippedRepositoryPage,
							AddDataWindowMessages.SelectDataSourcePage)));

			NextCommand = new RelayCommand(Execute);
		}


		private void Execute() {
			//TODO: проверка на существование файла.

			//_messenger.Send(AddDataWindowMessages.ReadProgressPage);

			_messenger.Send(new NavigateMessage<AddDataWindowMessages>(NavigateDirection.Forward,
					AddDataWindowMessages.SelectZippedRepositoryPage,
					AddDataWindowMessages.ReadProgressPage));
		}

		#region Implementation of ISelectPathHelperViewModel

		public string ToolTipText => "Пожалуйста, укажите файл с данными РПД";

		public string Label => "Файл:";

		public string LastPath {
			get => _applicationSettings.LastRpdDataImageFile;

			set {
				_applicationSettings.LastRpdDataImageFile = value;
				_applicationSettings.Save();
			}
		}

		public RelayCommand BackCommand { get; }

		public RelayCommand NextCommand { get; }

		public void ShowBrowseDialog(Action<string> browseResult) {
			var msg = new DialogMessage<OpenFileDialog>(this,
					new OpenFileDialog() {
						CheckFileExists = true,
						Multiselect = false,
						FileName = LastPath,
						Filter = Resources.ExportRpdData_Dialog_Filter
					},
					result => {
						if (result.DialogResult == DialogResult.OK) {
							browseResult(result.Dialog.FileName);
						}
					});

			_messenger.Send(msg, AppMessages.ShowSelectRdpDataPathDialog);
		}


		#endregion
	}
}
