using System;
using System.IO;
using System.Windows.Forms;
using Dnv.Utils.Messages;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RPD.Presentation.Messages;
using RPD.Presentation.Properties;
using RPD.Presentation.Settings;

namespace RPD.Presentation.ViewModels.AddDataViewModels.SelectPathHelperViewModels {
	/// <summary>
	/// Выбор файла, в который будут экспортированы данные.
	/// </summary>
	public class SelectExportImagePathHelper : ISelectPathHelperViewModel {
		private readonly IMessenger _messenger;
		private readonly IApplicationSettings _applicationSettings;

		public SelectExportImagePathHelper(IMessenger messenger, IApplicationSettings applicationSettings, Action onNavigateNext) {
			_messenger = messenger;
			_applicationSettings = applicationSettings;

			BackCommand = new RelayCommand(() => /*_messenger.Send(AddDataWindowMessages.AvailableFaultsPage)*/
					_messenger.Send(new NavigateMessage<AddDataWindowMessages>(NavigateDirection.Forward,
							AddDataWindowMessages.SelectExportPathPage,
							AddDataWindowMessages.AvailableFaultsPage)));

			NextCommand = new RelayCommand(onNavigateNext);
		}
		#region Implementation of ISelectPathHelperViewModel

		public string ToolTipText => "Пожалуйста, укажите файл в который будут записаны данные";

		public string Label => "Файл:";

		public string LastPath {
			get => _applicationSettings.LastExportFolder;
			set {
				_applicationSettings.LastExportFolder = Path.GetDirectoryName(value);
				_applicationSettings.Save();
			}
		}

		public RelayCommand BackCommand { get; }

		public RelayCommand NextCommand { get; }

		public void ShowBrowseDialog(Action<string> browseResult) {
			var msg = new DialogMessage<SaveFileDialog>(this,
					new SaveFileDialog {
						AddExtension = true,
						InitialDirectory = LastPath,
						AutoUpgradeEnabled = true,
						DefaultExt = "*.rpd",
						OverwritePrompt = true,
						Title = "Экспорт",
						Filter = Resources.ExportRpdData_Dialog_Filter
					},
					result => {
						if (result.DialogResult == DialogResult.OK)
							browseResult(result.Dialog.FileName);
					});

			_messenger.Send(msg, AppMessages.ShowSelectRdpDataPathDialog);
		}

		#endregion
	}
}