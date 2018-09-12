using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RPD.DAL;
using RPD.Presentation.Contracts;
using RPD.Presentation.Contracts.ViewModels;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;
using RPD.Presentation.Messages;
using RPD.Presentation.Messages.Parameters;
using RPD.Presentation.Utils.Messages;
using RPD.Presentation.ViewModels.DalViewModels;

namespace RPD.Presentation.ViewModels {
	internal class ChangePsnConfigurationViewModel : ViewModelBase, IChangePsnConfigurationViewModel, IMessageable {
		private IMessenger _messenger;
		private IPsnConfigurationViewModel _selectedConfiguration;
		private LogsParameters _parameters;

		public ChangePsnConfigurationViewModel(ILoader loader) {
			AvailableConfigurations = new ObservableCollection<IPsnConfigurationViewModel>();

			foreach (var psnConfig in loader.AvailablePsnConfigruations)
				AvailableConfigurations.Add(new PsnConfigurationViewModel(psnConfig, null));

			ApplyCommand = new RelayCommand(ApplyCommandExecute);
		}

		public void StartMessaging(IMessenger messenger) {
			_messenger = messenger;
			_messenger.Register<SetViewModelParametersMessage<LogsParameters>>(this, p => _parameters = p.Parameter);
		}

		public ObservableCollection<IPsnConfigurationViewModel> AvailableConfigurations { get; }

		public IPsnConfigurationViewModel SelectedConfiguration {
			get => _selectedConfiguration;
			set { Set(() => SelectedConfiguration, ref _selectedConfiguration, value); }
		}

		public RelayCommand ApplyCommand { get; }

		private void ApplyCommandExecute() {
			foreach (var psnLog in _parameters.PsnLogs) {
				psnLog.Update(SelectedConfiguration.Model, psnLog.PsnConfiguration.Name);
			}

			_messenger.Send(new ViewMessage(ViewAction.Close), Views.Views.ChangePsnConfiguration);
		}
	}
}