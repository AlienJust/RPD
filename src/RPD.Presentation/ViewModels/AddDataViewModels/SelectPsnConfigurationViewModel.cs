using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RPD.DAL;
using RPD.Presentation.Contracts.Repositories;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;
using RPD.Presentation.Messages;
using RPD.Presentation.ViewModels.DalViewModels;

namespace RPD.Presentation.ViewModels.AddDataViewModels {
	internal interface ISelectPsnConfigurationViewModel {
		ObservableCollection<IPsnConfigurationViewModel> AvailableConfigurations { get; }
		IPsnConfigurationViewModel SelectedConfiguration { get; set; }

		RelayCommand NextCommand { get; }
		RelayCommand BackCommand { get; }
	}

	/// <summary>
	/// Выбор конфигурации ПСН.
	/// </summary>
	class SelectPsnConfigurationViewModel : ViewModelBase, ISelectPsnConfigurationViewModel {
		private readonly IMessenger _messenger;
		private readonly IDeviceNumberToPsnConfigurationRepository _configsRepository;
		private readonly IAddDataParameters _addDataParameters;
		private readonly int _deviceNumber;
		private IPsnConfigurationViewModel _selectedConfiguration;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="loader"></param>
		/// <param name="messenger"></param>
		/// <param name="configsRepository"></param>
		/// <param name="addDataParameters">Сюда будет записана выбранная конфигурация.</param>
		/// <param name="deviceNumber">Номер устройства для которого нужно выбрать конфигурацию.</param>
		public SelectPsnConfigurationViewModel(ILoader loader,
				IMessenger messenger,
				IDeviceNumberToPsnConfigurationRepository configsRepository,
				IAddDataParameters addDataParameters,
				int deviceNumber) {
			_messenger = messenger;
			_configsRepository = configsRepository;
			_addDataParameters = addDataParameters;
			_deviceNumber = deviceNumber;
			SelectedConfiguration = null;

			AvailableConfigurations = new ObservableCollection<IPsnConfigurationViewModel>();

			FillAvailableConfigurations(loader);

			NextCommand = new RelayCommand(NextCommandExecute, () => SelectedConfiguration != null);
			BackCommand = new RelayCommand(BackCommandExecute);
		}

		private void FillAvailableConfigurations(ILoader loader) {
			var configUid = GetPsnConfigurationForDevice(_deviceNumber);

			foreach (var psnConfig in loader.AvailablePsnConfigruations) {
				var config = new PsnConfigurationViewModel(psnConfig, null);

				if (psnConfig.Id.ToString() == configUid)
					SelectedConfiguration = config;

				AvailableConfigurations.Add(config);
			}
		}

		private string GetPsnConfigurationForDevice(int deviceNumber) {
			string configUid = "no config";
			if (_configsRepository.Configurtions.ContainsKey(deviceNumber))
				configUid = _configsRepository.Configurtions[deviceNumber];

			return configUid;
		}

		private void NextCommandExecute() {
			_addDataParameters.PsnConfiguration = _selectedConfiguration.Model;

			_configsRepository.Configurtions[_deviceNumber] = _selectedConfiguration.Model.Id.ToString();
			_configsRepository.Save();

			_messenger.Send(new NavigateMessage<AddDataWindowMessages>(NavigateDirection.Forward,
					from: AddDataWindowMessages.SelectPsnConfiguration,
					to: AddDataWindowMessages.ReadProgressPage));
		}

		private void BackCommandExecute() {
			_messenger.Send(new NavigateMessage<AddDataWindowMessages>(NavigateDirection.Backward,
							AddDataWindowMessages.SelectPsnConfiguration));
		}

		public ObservableCollection<IPsnConfigurationViewModel> AvailableConfigurations { get; }

		public IPsnConfigurationViewModel SelectedConfiguration {
			get => _selectedConfiguration;
			set { Set(() => SelectedConfiguration, ref _selectedConfiguration, value); }
		}

		public RelayCommand NextCommand { get; }
		public RelayCommand BackCommand { get; }
	}
}
