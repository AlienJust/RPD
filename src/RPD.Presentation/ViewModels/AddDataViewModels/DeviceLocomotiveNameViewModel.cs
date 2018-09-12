using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RPD.Presentation.Contracts.Repositories;
using RPD.Presentation.Messages;
using RPD.Presentation.Model;

namespace RPD.Presentation.ViewModels.AddDataViewModels {

	internal interface IDeviceLocomotiveNameViewModel {
		string DeviceNumber { get; }
		string LocomotiveName { get; set; }
		string SectionNumber { get; set; }

		ObservableCollection<string> Sections { get; }

		RelayCommand NextCommand { get; }
		RelayCommand BackCommand { get; }
	}

	/// <summary>
	/// Страница ввода названия локомотива и номера секции выбранного устройства.
	/// </summary>
	internal class DeviceLocomotiveNameViewModel : ViewModelBase, IDeviceLocomotiveNameViewModel {
		private readonly IMessenger _messenger;
		private readonly IFtpParameters _ftpParameters;
		private readonly IAddDataParameters _addDataParameters;
		private readonly IDeviceInfoRepository _deviceInfoRepository;
		private string _locomotiveName;
		private string _sectionNumber;

		public DeviceLocomotiveNameViewModel(IMessenger messenger, IFtpParameters ftpParameters,
				IAddDataParameters addDataParameters, IDeviceInfoRepository deviceInfoRepository) {
			_messenger = messenger;
			_ftpParameters = ftpParameters;
			_addDataParameters = addDataParameters;
			_deviceInfoRepository = deviceInfoRepository;

			LocomotiveName = _ftpParameters.FtpLocomotiveName;
			SectionNumber = _ftpParameters.FtpSectionName;

			Sections = new ObservableCollection<string> { "1", "2" };
			SectionNumber = "1";

			NextCommand = new RelayCommand(NextCommandExecute, () => !string.IsNullOrWhiteSpace(LocomotiveName));
			BackCommand = new RelayCommand(BackCommandExecute);
		}

		public string DeviceNumber => _addDataParameters.DeviceNumber.ToString(CultureInfo.InvariantCulture);

		public string LocomotiveName {
			get => _locomotiveName;
			set {
				Set(() => LocomotiveName, ref _locomotiveName, value);
				CommandManager.InvalidateRequerySuggested();
			}
		}

		public string SectionNumber {
			get => _sectionNumber;
			set { Set(() => SectionNumber, ref _sectionNumber, value); }
		}

		#region Commands

		public ObservableCollection<string> Sections { get; }

		public RelayCommand NextCommand { get; }

		private void NextCommandExecute() {
			SaveDeviceInfo();

			_ftpParameters.FtpLocomotiveName = _locomotiveName;
			_ftpParameters.FtpSectionName = _sectionNumber;

			_messenger.Send(new NavigateMessage<AddDataWindowMessages>(NavigateDirection.Forward,
					AddDataWindowMessages.DeviceLocomotiveNameViewModel,
					AddDataWindowMessages.SelectPsnConfiguration));
		}

		private void SaveDeviceInfo() {
			var device = _deviceInfoRepository.GetByDeviceNumber(_ftpParameters.FtpRepositoryInfo.DeviceNumber);
			if (device == null)
				device = new DeviceInfo();

			device.DeviceNumber = _ftpParameters.FtpRepositoryInfo.DeviceNumber;
			device.LocomotiveName = _locomotiveName;
			device.SectionNumber = int.Parse(_sectionNumber);

			_deviceInfoRepository.AddOrUpdate(device);
		}


		public RelayCommand BackCommand { get; }

		private void BackCommandExecute() {
			_messenger.Send(new NavigateMessage<AddDataWindowMessages>(NavigateDirection.Backward,
					AddDataWindowMessages.DeviceLocomotiveNameViewModel));
		}

		#endregion // Commands
	}
}
