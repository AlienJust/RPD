using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RPD.Presentation.Contracts.Model;
using RPD.Presentation.Contracts.Repositories;
using RPD.Presentation.Messages;
using RPD.Presentation.Settings;
using RPD.Presentation.ViewModels.DalViewModels;

namespace RPD.Presentation.ViewModels.AddDataViewModels {
	internal interface ISelectFtpServerViewModel {
		ObservableCollection<FtpServerViewModel> Servers { get; }

		FtpServerViewModel SelectedServer { get; set; }

		RelayCommand AddServerCommand { get; }
		RelayCommand EditServerCommand { get; }
		RelayCommand DeleteServerCommand { get; }

		RelayCommand NextCommand { get; }
		RelayCommand BackCommand { get; }
	}

	internal class SelectFtpServerViewModel : ViewModelBase, ISelectFtpServerViewModel {
		private readonly IMessenger _messenger;
		private readonly IFtpParameters _addDataParameters;
		private readonly IApplicationSettings _applicationSettings;
		private readonly ObservableCollection<FtpServerViewModel> _servers;
		private FtpServerViewModel _selectedServer;

		public SelectFtpServerViewModel(IMessenger messenger, IFtpParameters addDataParameters,
				IFtpServersRepository ftpServersRepository, IApplicationSettings applicationSettings) {
			_messenger = messenger;
			_addDataParameters = addDataParameters;
			_applicationSettings = applicationSettings;

			_servers = new ObservableCollection<FtpServerViewModel>();

			foreach (var server in ftpServersRepository.ListAll())
				_servers.Add(new FtpServerViewModel(server));

			IntializeCommands();

			SetPreviousSelectedServer();
		}

		private void IntializeCommands() {
			AddServerCommand = new RelayCommand(AddServerCommandExecute);
			EditServerCommand = new RelayCommand(EditServerCommandExecute, () => SelectedServer != null);
			DeleteServerCommand = new RelayCommand(DeleteServerCommandExecute, () => SelectedServer != null);
			NextCommand = new RelayCommand(NextCommandExecute, () => SelectedServer != null);
			BackCommand = new RelayCommand(BackCommandExecute);
		}

		private void SetPreviousSelectedServer() {
			if (!string.IsNullOrWhiteSpace(_applicationSettings.LastSelectedFtpServerName))
				SelectedServer = _servers.FirstOrDefault(x => x.Name == _applicationSettings.LastSelectedFtpServerName);
		}

		public ObservableCollection<FtpServerViewModel> Servers => _servers;

		public FtpServerViewModel SelectedServer {
			get => _selectedServer;
			set {
				Set(() => SelectedServer, ref _selectedServer, value);
				CommandManager.InvalidateRequerySuggested();
			}
		}

		#region Commands

		public RelayCommand AddServerCommand { get; private set; }

		private void AddServerCommandExecute() {
			throw new NotImplementedException();
		}


		public RelayCommand EditServerCommand { get; private set; }

		private void EditServerCommandExecute() {
			throw new NotImplementedException();
		}


		public RelayCommand DeleteServerCommand { get; private set; }

		private void DeleteServerCommandExecute() {
			throw new NotImplementedException();
		}


		public RelayCommand NextCommand { get; private set; }

		private void NextCommandExecute() {
			_addDataParameters.FtpServer = (IFtpServer)SelectedServer.Model;
			_applicationSettings.LastSelectedFtpServerName = SelectedServer.Model.Name;
			_applicationSettings.Save();

			_messenger.Send(new NavigateMessage<AddDataWindowMessages>(NavigateDirection.Forward,
					AddDataWindowMessages.SelectFtpServerPage,
					AddDataWindowMessages.SelectFtpDevicePage));
		}


		public RelayCommand BackCommand { get; private set; }

		private void BackCommandExecute() {
			_messenger.Send(new NavigateMessage<AddDataWindowMessages>(NavigateDirection.Backward,
					AddDataWindowMessages.SelectFtpServerPage));
		}

		#endregion //Commands
	}
}
