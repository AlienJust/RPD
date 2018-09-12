using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.IO;
using System.Windows.Input;
using RPD.Presentation.Messages;
using RPD.Presentation.Utils.Classes;

namespace RPD.Presentation.ViewModels.AddDataViewModels {
	internal interface ISelectUsbDeviceViewModel {
		/// <summary>
		/// Список подключенных накопителей.
		/// </summary>
		IUsbRemovableDrives Drives { get; }

		/// <summary>
		/// Выбранный накопитель.
		/// </summary>
		DriveInfo SelectedDrive { get; set; }

		RelayCommand NextCommand { get; set; }
		RelayCommand BackCommand { get; set; }
	}

	/// <summary>
	/// Модель представления выбора USB устройства для считывания.
	/// Generates messages: AppMessages.AddDataWindow.ReadProgressPage, 
	/// AppMessages.AddDataWindow.SelectDataSourcePage.
	/// </summary>
	class SelectUsbUsbDeviceViewModel : ViewModelBase, ISelectUsbDeviceViewModel {
		private DriveInfo _selectedDrive = null;

		private readonly IMessenger _messenger;
		private readonly IAddDataParameters _addDataParameters;

		/// <summary>
		/// Конструктор.
		/// </summary>
		public SelectUsbUsbDeviceViewModel(IMessenger messenger, IAddDataParameters addDataParameters, IUsbRemovableDrives usbRemovableDrives) {
			_messenger = messenger;
			_addDataParameters = addDataParameters;
			Drives = usbRemovableDrives;

			InitializeCommands();
		}

		void InitializeCommands() {
			NextCommand = new RelayCommand(NextCommandExecute,
					() => SelectedDrive != null);

			BackCommand = new RelayCommand(BackCommandExecute);
		}

		#region Presentation Properties

		/// <summary>
		/// Список подключенных накопителей.
		/// </summary>
		public IUsbRemovableDrives Drives { get; }

		/// <summary>
		/// Выбранный накопитель.
		/// </summary>
		public DriveInfo SelectedDrive {
			get => _selectedDrive;

			set {
				Set(() => SelectedDrive, ref _selectedDrive, value);

				// Обновляем биндинги команд во View. Нужно чтобы NextCommandCanExecute очухался.
				CommandManager.InvalidateRequerySuggested();
			}
		}


		#endregion // Presentation Properties


		#region Commands

		public RelayCommand NextCommand { get; set; }

		void NextCommandExecute() {
			// Устанавливаем путь к устройству в контекст.
			if (_selectedDrive != null)
				_addDataParameters.DevicePath = _selectedDrive.Name;

			_messenger.Send(new NavigateMessage<AddDataWindowMessages>(NavigateDirection.Forward,
					from: AddDataWindowMessages.SelectUsbDevicePage,
					to: AddDataWindowMessages.SelectPsnConfiguration));
		}


		public RelayCommand BackCommand { get; set; }

		void BackCommandExecute() {
			_messenger.Send(new NavigateMessage<AddDataWindowMessages>(NavigateDirection.Backward,
					AddDataWindowMessages.SelectUsbDevicePage));
		}

		#endregion // Commands
	}

}