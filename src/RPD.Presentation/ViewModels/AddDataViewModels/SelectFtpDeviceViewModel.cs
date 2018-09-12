using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RPD.DAL;
using RPD.EventArgs;
using RPD.Presentation.Contracts.Repositories;
using RPD.Presentation.Contracts.ViewModels.AddDataViewModels;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;
using RPD.Presentation.Messages;
using RPD.Presentation.ViewModels.DalViewModels;

namespace RPD.Presentation.ViewModels.AddDataViewModels {
	/// <summary>
	/// Выбор устройства на FTP сервере.
	/// </summary>
	internal class SelectFtpDeviceViewModel : ViewModelBase, ISelectFtpDeviceViewModel {
		private readonly IMessenger _messenger;
		private readonly IFtpParameters _ftpParamaters;
		private readonly IAddDataParameters _addDataParameters;
		private readonly IDeviceInfoRepository _deviceInfoRepository;
		private IFtpRepositoryInfoViewModel _selectedItem;
		private bool _isBusy;

		public SelectFtpDeviceViewModel(IMessenger messenger, ILoader loader,
				IFtpParameters ftpParamaters, IAddDataParameters addDataParameters, IDeviceInfoRepository deviceInfoRepository) {
			_messenger = messenger;
			_ftpParamaters = ftpParamaters;
			_addDataParameters = addDataParameters;
			_deviceInfoRepository = deviceInfoRepository;

			Items = new ObservableCollection<IFtpRepositoryInfoViewModel>();

			NextCommand = new RelayCommand(NextCommandExecute, () => SelectedItem != null);
			BackCommand = new RelayCommand(BackCommandExecute);

			FillDevicesList(loader, ftpParamaters);
		}

		private void FillDevicesList(ILoader loader, IFtpParameters addDataParameters) {
			IsBusy = true;

			var ftpServer = addDataParameters.FtpServer;

			loader.GetFtpRepositoryInfosAsync(ftpServer.Host, ftpServer.Port, ftpServer.User, ftpServer.Password,
																				OnFtpRepositoryInfosLoaded);
		}

		private void OnFtpRepositoryInfosLoaded(OnCompleteEventArgs onCompleteEventArgs, IEnumerable<IFtpRepositoryInfo> ftpRepositoryInfos) {
			if (onCompleteEventArgs.ResultCode == OnCompleteEventArgs.CompleteResult.Error &&
					!onCompleteEventArgs.Message.Contains("Список устройств получен")) {
				_messenger.Send(new DialogMessage(this, onCompleteEventArgs.Message,
						(res) => _messenger.Send(AddDataWindowMessages.SelectDataSourcePage)) { Caption = "Ошибка" },
						AppMessages.ErrorDialogMessage);
				return;
			}

			IsBusy = false;
			foreach (var ftpRepositoryInfo in ftpRepositoryInfos)
				Items.Add(new FtpRepositoryInfoViewModel(ftpRepositoryInfo, _deviceInfoRepository));
		}


		#region Presentation Properties

		public bool IsBusy {
			get => _isBusy;
			set { Set(() => IsBusy, ref _isBusy, value); }

		}

		public IFtpRepositoryInfoViewModel SelectedItem {
			get => _selectedItem;
			set {
				Set(() => SelectedItem, ref _selectedItem, value);
				CommandManager.InvalidateRequerySuggested();
			}
		}

		public ObservableCollection<IFtpRepositoryInfoViewModel> Items { get; private set; }

		#endregion // Presentation Properties


		#region Commands

		public RelayCommand NextCommand { get; private set; }

		private void NextCommandExecute() {
			_ftpParamaters.FtpRepositoryInfo = SelectedItem.Model;
			_addDataParameters.DeviceNumber = SelectedItem.Model.DeviceNumber;

			if (SelectedItem.IsNameSet) {
				_ftpParamaters.FtpSectionName = SelectedItem.SectionName;
				_ftpParamaters.FtpLocomotiveName = SelectedItem.LocomotiveName;
			}

			_messenger.Send(new NavigateMessage<AddDataWindowMessages>(NavigateDirection.Forward,
					AddDataWindowMessages.SelectFtpDevicePage,
					AddDataWindowMessages.DeviceLocomotiveNameViewModel));
		}


		public RelayCommand BackCommand { get; private set; }

		private void BackCommandExecute() {
			_messenger.Send(new NavigateMessage<AddDataWindowMessages>(NavigateDirection.Backward,
					AddDataWindowMessages.SelectFtpDevicePage));
		}

		#endregion // Commands
	}
}
