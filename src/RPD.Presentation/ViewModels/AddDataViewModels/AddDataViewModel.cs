using System;
using System.Collections.Generic;
using System.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using RPD.DAL;
using RPD.EventArgs;
using RPD.Presentation.Contracts;
using RPD.Presentation.Contracts.Model;
using RPD.Presentation.Contracts.Repositories;
using RPD.Presentation.Contracts.ViewModels;
using RPD.Presentation.Contracts.ViewModels.AddDataViewModels;
using RPD.Presentation.Messages;
using RPD.Presentation.Messages.Parameters;
using RPD.Presentation.Settings;
using RPD.Presentation.Utils.Classes;
using RPD.Presentation.Utils.Messages;
using RPD.Presentation.ViewModels.AddDataViewModels.SelectPathHelperViewModels;

namespace RPD.Presentation.ViewModels.AddDataViewModels {
	internal interface IFtpParameters {
		IFtpServer FtpServer { get; set; }

		IFtpRepositoryInfo FtpRepositoryInfo { get; set; }

		string FtpLocomotiveName { get; set; }

		string FtpSectionName { get; set; }
	}

	/// <summary>
	/// Properties Common To Child View Models
	/// </summary>
	internal interface IAddDataParameters {
		/// <summary>
		/// Путь к к данным устройства.
		/// </summary>
		string DevicePath { get; set; }

		/// <summary>
		/// Номер устройства РПД.
		/// </summary>
		int DeviceNumber { get; set; }

		IPsnConfiguration PsnConfiguration { get; set; }

		/// <summary>
		/// Репозиторий из которого нужно вычитать данные.
		/// </summary>
		IRepository SourceRepository { get; set; }

		/// <summary>
		/// Считанный с устройства локомотив.
		/// </summary>
		IList<ILocomotive> ReadLocomotives { get; set; }

		/// <summary>
		/// Список аварий, которые нужно считать.
		/// </summary>
		List<IFaultLog> FaultsToRead { get; set; }

		/// <summary>
		/// Список логов ПСН, которые нужно считать.
		/// </summary>
		List<IPsnLog> PsnLogsToRead { get; set; }
	}

	interface IAddDataViewModel {
		string Title { get; }

		ISelectDataSourceViewModel SelectDataSource { get; }
		ISelectUsbDeviceViewModel SelectUsbDevice { get; }
		ISelectPathViewModel SelectPath { get; }
		IAvailableFaultsViewModel AvailableFaults { get; }
		IProgressViewModel ReadProgress { get; }
		ISelectPsnConfigurationViewModel SelectPsnConfiguration { get; }
		ISelectFtpServerViewModel SelectFtpServer { get; }
		ISelectFtpDeviceViewModel SelectFtpDevice { get; }
		IDeviceLocomotiveNameViewModel DeviceLocomotiveName { get; }
	}

	/// <summary>
	/// Модель представления окна добавления данных. Каждая страница этого окна имеет свою модель представления.
	/// Sends messages:
	/// ShowCopyProgressWindow,
	/// IAddDataParameters.
	/// </summary>
	class AddDataViewModel : ViewModelBase, IAddDataViewModel, IAddDataParameters, IMessageable, IFtpParameters, IDisposable {
		private SelectDataSourceViewModel _selectDataSource;
		private SelectUsbUsbDeviceViewModel _selectUsbUsbDevice;
		private SelectPathViewModel _selectPath;
		private IAvailableFaultsViewModel _availableFaults;
		private ISelectPsnConfigurationViewModel _selectPsnConfiguration;
		private ISelectFtpServerViewModel _selectFtpServer;
		private ISelectFtpDeviceViewModel _selectFtpDevice;
		private IDeviceLocomotiveNameViewModel _deviceLocomotiveName;

		private AddDataWindowMessages _currentDataSourcePage;

		private readonly IApplicationSettings _applicationSettings;
		private readonly ILoader _loader;
		private readonly IRepository _localRepository = null;
		private readonly IUsbRemovableDrives _usbRemovableDrives;
		private readonly IFtpServersRepository _ftpServersRepository;
		private readonly IDeviceInfoRepository _deviceInfoRepository;
		private readonly IDeviceNumberToPsnConfigurationRepository _configsRepository;

		private readonly bool _isSimpleMode;

		private IMessenger _messenger;


		private Stack<AddDataWindowMessages> _history = new Stack<AddDataWindowMessages>();
		private IProgressViewModel _readProgressViewModel;
		private string _devicePath;


		/// <summary>
		/// 
		/// </summary>
		/// <param name="applicationSettings">Настройки приложения.</param>
		/// <param name="loader">Загрузчик RDP DAL.</param>
		/// <param name="localRepository">Локальный репозиторий (который хранит основные данные).</param>
		/// <param name="usbRemovableDrives">Работа с USB флешками.</param>
		/// /// <param name="ftpServersRepository">Доступные сервера FTP.</param>
		/// <param name="deviceInfoRepository">Репозиторий объектов DeviceInfo.</param>
		/// <param name="configsRepository"></param>
		/// <param name="isSimpleMode">Признак, того что приложение запущено в режиме simple. В этом режиме этот 
		/// диалог является главным окном приложения, после выбора аварий для копирования отображается вид с предложеним 
		/// выбора файла для сохранения и отображается окно прогресса экспорта, вместо окна прогресса копирования.</param>
		public AddDataViewModel(IApplicationSettings applicationSettings,
														ILoader loader,
														IRepository localRepository,
														IUsbRemovableDrives usbRemovableDrives,
														IFtpServersRepository ftpServersRepository,
														IDeviceInfoRepository deviceInfoRepository,
														IDeviceNumberToPsnConfigurationRepository configsRepository,
														bool isSimpleMode) {
			_applicationSettings = applicationSettings;
			_loader = loader;
			_localRepository = localRepository;
			_usbRemovableDrives = usbRemovableDrives;
			_ftpServersRepository = ftpServersRepository;
			_deviceInfoRepository = deviceInfoRepository;
			_configsRepository = configsRepository;
			_isSimpleMode = isSimpleMode;
			Title = "Добавление данных РПД";

			FaultsToRead = new List<IFaultLog>();
			PsnLogsToRead = new List<IPsnLog>();
		}

		/// <summary>
		/// Конструктор для работы программы в режиме SimpleMode. В этом режиме окно AddDataView 
		/// главной и единственной формой приложения.
		/// </summary>
		/// <param name="applicationSettings"></param>
		/// <param name="loader"></param>
		/// <param name="usbRemovableDrives"></param>
		/// <param name="deviceInfoRepository"></param>
		/// <param name="configsRepository"></param>
		/// <param name="isSimpleMode">Признак, того что приложение запущено в режиме simple. В этом режиме этот 
		/// диалог является главным окном приложения, после выбора аварий для копирования отображается вид с предложеним 
		/// выбора файла для сохранения и отображается окно прогресса экспорта, вместо окна прогресса копирования.</param>
		/// <param name="ftpServersRepository"></param>
		public AddDataViewModel(IApplicationSettings applicationSettings,
														ILoader loader,
														IUsbRemovableDrives usbRemovableDrives,
														IFtpServersRepository ftpServersRepository,
														IDeviceInfoRepository deviceInfoRepository,
														IDeviceNumberToPsnConfigurationRepository configsRepository,
														bool isSimpleMode) :
				this(applicationSettings, loader, null, usbRemovableDrives, ftpServersRepository, deviceInfoRepository, configsRepository, isSimpleMode) {
			Title = "Сохранение данных РПД";
		}

		#region Implementation of IMessageable

		public void StartMessaging(IMessenger messenger) {
			_messenger = messenger;
			_messenger.Register<NavigateMessage<AddDataWindowMessages>>(this, OnNavigateMessage);
		}

		#endregion

		private void OnNavigateMessage(NavigateMessage<AddDataWindowMessages> message) {
			if (message.NavigateDirection == NavigateDirection.Forward) {
				_history.Push(message.From);

				_messenger.Send(message.To);
			}
			else if (message.NavigateDirection == NavigateDirection.Backward) {
				var prev = _history.Pop();
				if (prev == AddDataWindowMessages.ReadProgressPage) {
					_readProgressViewModel = null;
					prev = _history.Pop();
				}

				ResetPreviousPageViewModel(message.From);

				_messenger.Send(prev);
			}
		}

		/// <summary>
		/// Сбрасывает (обнуляет) МП предыдущей страницы (с которой вернулись обратно).
		/// </summary>
		/// <param name="prevPage"></param>
		private void ResetPreviousPageViewModel(AddDataWindowMessages prevPage) {
			switch (prevPage) {
				case AddDataWindowMessages.SelectDataSourcePage:
					_selectDataSource = null;
					break;
				case AddDataWindowMessages.SelectUsbDevicePage:
					_selectUsbUsbDevice = null;
					break;
				case AddDataWindowMessages.SelectZippedRepositoryPage:
					_selectPath = null;
					break;
				case AddDataWindowMessages.SelectFolderPage:
					_selectPath = null;
					break;
				case AddDataWindowMessages.SelectExportPathPage:
					_selectPath = null;
					break;
				case AddDataWindowMessages.AvailableFaultsPage:
					_availableFaults = null;
					break;
				case AddDataWindowMessages.ReadProgressPage:
					_readProgressViewModel = null;
					break;
				case AddDataWindowMessages.SelectPsnConfiguration:
					_selectPsnConfiguration = null;
					break;
				case AddDataWindowMessages.SelectFtpServerPage:
					_selectFtpServer = null;
					break;
				case AddDataWindowMessages.SelectFtpDevicePage:
					_selectFtpDevice = null;
					break;
				case AddDataWindowMessages.DeviceLocomotiveNameViewModel:
					_deviceLocomotiveName = null;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		#region Implementation of IAddDataParameters

		private bool _waitForDeviceNumber;
		private readonly AutoResetEvent _waitForDeviceNumberEvent = new AutoResetEvent(false);
		private string _title;

		public string DevicePath {
			get => _devicePath;
			set {
				if (_devicePath == value)
					return;

				_devicePath = value;

				ReadDeviceNumberFromPath(_devicePath);
			}
		}


		private void ReadDeviceNumberFromPath(string devicePath) {
			_waitForDeviceNumber = true;
			_waitForDeviceNumberEvent.Reset();
			var config = _loader.CreateDeviceConfiguration();
			config.Read(devicePath,
					args => {
						if (args.ResultCode == OnCompleteEventArgs.CompleteResult.Ok)
							DeviceNumber = config.NetAddress;

						_waitForDeviceNumber = false;
						_waitForDeviceNumberEvent.Set();
					});
		}

		private void WaitForDeviceNumber() {
			if (_waitForDeviceNumber)
				_waitForDeviceNumberEvent.WaitOne(5000);
		}

		public int DeviceNumber { get; set; }

		public IPsnConfiguration PsnConfiguration { get; set; }

		public IRepository SourceRepository { get; set; }

		/// <summary>
		/// Считанные с устройства локомотивы.
		/// </summary>
		public IList<ILocomotive> ReadLocomotives { get; set; }

		public List<IFaultLog> FaultsToRead { get; set; }

		public List<IPsnLog> PsnLogsToRead { get; set; }

		#endregion //Implementation of IAddDataParameters


		#region IFtpParameters

		public IFtpServer FtpServer { get; set; }

		public IFtpRepositoryInfo FtpRepositoryInfo { get; set; }

		public string FtpLocomotiveName { get; set; }

		public string FtpSectionName { get; set; }

		#endregion //IFtpParameters



		public string Title {
			get => _title;
			set { Set(() => Title, ref _title, value); }
		}

		public ISelectDataSourceViewModel SelectDataSource => _selectDataSource ?? (_selectDataSource = new SelectDataSourceViewModel(NavigateAfterSelectDataSource, _isSimpleMode));

		private void NavigateAfterSelectDataSource(AddDataWindowMessages navigateTo) {
			switch (navigateTo) {
				case AddDataWindowMessages.SelectFolderPage:
					_selectPath = new SelectPathViewModel(this, new SelectRpdDataDirectoryHelper(_messenger, _applicationSettings));
					_currentDataSourcePage = AddDataWindowMessages.SelectFolderPage;
					break;

				case AddDataWindowMessages.SelectZippedRepositoryPage:
					_selectPath = new SelectPathViewModel(this, new SelectZippedRepositoryPathHelper(_messenger, _applicationSettings));
					_currentDataSourcePage = AddDataWindowMessages.SelectZippedRepositoryPage;
					break;

				case AddDataWindowMessages.SelectUsbDevicePage:
					_currentDataSourcePage = AddDataWindowMessages.SelectUsbDevicePage;
					break;

				case AddDataWindowMessages.SelectFtpServerPage:
					_currentDataSourcePage = AddDataWindowMessages.SelectFtpServerPage;
					break;
			}

			_messenger.Send(new NavigateMessage<AddDataWindowMessages>(NavigateDirection.Forward,
					AddDataWindowMessages.SelectDataSourcePage, _currentDataSourcePage));
		}

		public ISelectPsnConfigurationViewModel SelectPsnConfiguration {
			get {
				if (_selectPsnConfiguration == null) {
					// ждём пока вычитается номер устройства (в сеттере DevicePath).
					WaitForDeviceNumber();

					_selectPsnConfiguration = new SelectPsnConfigurationViewModel(_loader, _messenger,
							_configsRepository, this, DeviceNumber);
				}

				return _selectPsnConfiguration;
			}
		}

		public ISelectFtpServerViewModel SelectFtpServer => _selectFtpServer ?? (_selectFtpServer = new SelectFtpServerViewModel(_messenger, this, _ftpServersRepository, _applicationSettings));

		public ISelectFtpDeviceViewModel SelectFtpDevice => _selectFtpDevice ?? (_selectFtpDevice = new SelectFtpDeviceViewModel(_messenger, _loader, this, this, _deviceInfoRepository));

		public IDeviceLocomotiveNameViewModel DeviceLocomotiveName => _deviceLocomotiveName ?? (_deviceLocomotiveName = new DeviceLocomotiveNameViewModel(_messenger, this, this, _deviceInfoRepository));

		public IProgressViewModel ReadProgress {
			get {
				if (_readProgressViewModel == null) {
					switch (_currentDataSourcePage) {
						case AddDataWindowMessages.SelectFolderPage:
							SourceRepository = _loader.GetNandFlashRepository(DevicePath, PsnConfiguration);
							break;

						case AddDataWindowMessages.SelectZippedRepositoryPage:
							SourceRepository = _loader.GetZippedRepository(DevicePath);
							break;

						case AddDataWindowMessages.SelectUsbDevicePage:
							SourceRepository = _loader.GetNandFlashRepository(DevicePath, PsnConfiguration);
							break;
						case AddDataWindowMessages.SelectFtpServerPage:
							SourceRepository = _loader.GetFtpRepository(FtpRepositoryInfo, FtpLocomotiveName,
									FtpSectionName, PsnConfiguration);
							break;
					}

					_readProgressViewModel = new ReadProgressViewModel(_messenger, SourceRepository, this);
				}

				return _readProgressViewModel;
			}
		}

		public IAvailableFaultsViewModel AvailableFaults => _availableFaults ?? (_availableFaults = new AvailableFaultsViewModel(_messenger, ReadLocomotives, this, _localRepository, NavigateAfterAvailableFaultsPage, _isSimpleMode));

		private void NavigateAfterAvailableFaultsPage() {
			if (_isSimpleMode) {
				_selectPath = new SelectPathViewModel(this,
						new SelectExportImagePathHelper(_messenger,
								_applicationSettings,
								onNavigateNext: ShowExportProggress));

				_messenger.Send(new NavigateMessage<AddDataWindowMessages>(NavigateDirection.Forward,
						from: AddDataWindowMessages.AvailableFaultsPage,
						to: AddDataWindowMessages.SelectExportPathPage));
				//_messenger.Send(AppMessages.AddDataWindow.SelectExportPathPage);
			}
			else
				ShowCopyProgress();
		}

		public ISelectUsbDeviceViewModel SelectUsbDevice => _selectUsbUsbDevice ?? (_selectUsbUsbDevice = new SelectUsbUsbDeviceViewModel(_messenger, this, _usbRemovableDrives));

		public ISelectPathViewModel SelectPath => _selectPath;

		private void ShowCopyProgress() {
			_messenger.Send(new ViewMessage(ViewAction.Show), Views.Views.CopyProggress);
			_messenger.Send(new SetViewModelParametersMessage<IAddDataParameters>(this));
		}

		private void ResetViewModels() {
			_selectDataSource = null;
			_selectUsbUsbDevice = null;
			_selectPath = null;
			_availableFaults = null;
			_selectPsnConfiguration = null;
			_selectFtpServer = null;
			_selectFtpDevice = null;
			_deviceLocomotiveName = null;
		}

		// Используется только в режиме SimpleMode
		private void ShowExportProggress() {
			// Т.к. SimpleMode, то SourceRepository
			var parameters = new ExportLogsParametersBase {
				Repository = SourceRepository,
				FileName = DevicePath,
				RpdLogs = FaultsToRead,
				PsnLogs = PsnLogsToRead,
			};

			_messenger.Send(new ViewMessage(ViewAction.Show), Views.Views.ExportProgress);
			_messenger.Send(new SetViewModelParametersMessage<ExportLogsParametersBase>(parameters));
		}

		public void Dispose() {
			_waitForDeviceNumberEvent.Dispose();
		}
	}
}
