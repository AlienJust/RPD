using System.IO;
using System.Windows.Forms;
using Dnv.Utils.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using RPD.DAL;
using RPD.EventArgs;
using GalaSoft.MvvmLight.Messaging;
using RPD.Presentation.Contracts;
using RPD.Presentation.Contracts.Model.SelectionMasks;
using RPD.Presentation.Contracts.ViewModels;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;
using RPD.Presentation.Extensions;
using RPD.Presentation.Messages;
using RPD.Presentation.Messages.Parameters;
using RPD.Presentation.Model;
using RPD.Presentation.Model.SelectionMasks;
using RPD.Presentation.Properties;
using RPD.Presentation.Settings;
using RPD.Presentation.Utils.Messages;
using RPD.SciChartControl.Events;

namespace RPD.Presentation.ViewModels {
	/// <summary>
	/// Модель представления главного окна программы.
	/// Generates messages: AppMessages.ShowPsnFakeWindow, DialogMessage, AppMessages.ShowAddDataWindow, 
	/// AppMessages.ShowSettingsWindow, AppMessages.CloseApplication.
	/// </summary>
	internal class MainViewModel : ViewModelBase, IMainViewModel {
		private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

		private IRepository _repository;
		private readonly IUnityContainer _container;
		private readonly ILoader _loader;
		private readonly IApplicationSettings _applicationSettings;
		private readonly IMessenger _messenger;
		private readonly IRepositoryViewModelFactory _repositoryViewModelFactory;
		private readonly IColorsStorage _colorsStorage;

		private bool _isTrendLoading;
		private bool _isRepositoryLoaded;
		private IRepositoryViewModel _repositoryViewModel;
		private object _test = new object();
		private ITrendsContainer _contextMenuTrendsContainer;
		private bool _isPopupOpen;
		private bool _isExportDataMode;
		private IPsnLogViewModel _contextMenuPsnLog;
		private bool _isRemoveDataMode;
		private bool _isChangeConfigurationMode;
		private bool _isExportDataModeAvailable = true;
		private bool _isRemoveDataModeAvailable = true;
		private bool _isChangeConfigurationModeAvailable = true;
		private bool _isDataSelectMode;

		private ISciChartViewModel _mainChart;
		private readonly RelayCommand _collapseTreeCmd;
		private readonly RelayCommand _collapsePowerOnTreeCmd;

		public bool IsTreeViewExpanded { get; set; }

		/// <summary>
		/// Начальная инициализация
		/// </summary>
		public RelayCommand Initialize { get; set; }

		/// <summary>
		/// Вызывает диалог добавления данных в проект.
		/// </summary>
		public RelayCommand ShowAddDataCommand { get; set; }

		/// <summary>
		/// Показать окно настроек.
		/// </summary>
		public RelayCommand ShowSettings { get; set; }

		/// <summary>
		/// Выйти из программы.
		/// </summary>
		public RelayCommand Close { get; set; }

		public RelayCommand LoadAllTrendsCommand { get; set; }
		public RelayCommand<ITrendsContainer> SetContextMenuTrendsContainerCommand { get; set; }

		public RelayCommand ShowIntegrityErrorsStatistics { get; set; }
		public RelayCommand RenamePsnLogCmd { get; set; }

		public RelayCommand<IPsnLogViewModel> SetContextMenuPsnLogCommand { get; set; }
		public RelayCommand<SeriesColorChangedEventArgs> SeriesColorChangedEventCommand { get; set; }
		public RelayCommand SaveSelectionMaskCommand { get; set; }
		public RelayCommand<ISelectionMask> LoadSelectionMaskCommand { get; set; }
		public RelayCommand ExportDataCommand { get; set; }
		public RelayCommand RemoveDataCommand { get; set; }
		public RelayCommand ChangeConfigurationCommand { get; set; }
		public RelayCommand ShowRpdConfigurator { get; set; }
		public RelayCommand ShowDefaultColorSettings { get; set; }
		public RelayCommand TestCommand { get; set; }
		public RelayCommand ShowHelpWindow { get; set; }
		public RelayCommand ShowAboutProgramWindow { get; set; }
		public RelayCommand UncheckAllSignalsCommand { get; set; }
		public RelayCommand RemoveAllRpdSignalsFromChart { get; set; }
		public RelayCommand RemoveAllPsnSignalsFromChart { get; set; }
		public RelayCommand RemoveAllPsnPowerOnSignalsFromChart { get; set; }

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="container">Используется только для того, чтобы зарегистрировать репозиторий после его инициализации </param>
		/// <param name="loader">Загрузчик.</param>
		/// <param name="applicationSettings">Настройки приложения</param>
		/// <param name="selectionMasksStorage"></param>
		/// <param name="messenger"></param>
		/// <param name="repositoryViewModelFactory"></param>
		/// <param name="sciChartViewModelFactory"> </param>
		/// <param name="colorsStorage"></param>
		public MainViewModel(IUnityContainer container,
				ILoader loader,
				IApplicationSettings applicationSettings,
				ISelectionMasksStorage selectionMasksStorage,
				IMessenger messenger,
				IRepositoryViewModelFactory repositoryViewModelFactory,
				ISciChartViewModelFactory sciChartViewModelFactory,
				IColorsStorage colorsStorage) {
			SelectionMasksStorage = selectionMasksStorage;

			_container = container;
			_loader = loader;
			_applicationSettings = applicationSettings;
			_messenger = messenger;
			_repositoryViewModelFactory = repositoryViewModelFactory;
			_colorsStorage = colorsStorage;

			_mainChart = sciChartViewModelFactory.Create();

			IsRepositoryLoaded = false;

			if (File.Exists(_applicationSettings.SelectionMasksFullFilePath))
				SelectionMasksStorage.Load(_applicationSettings.SelectionMasksFullFilePath);

			InitializeCommands();
			_collapseTreeCmd = new RelayCommand(CollapseTree, () => true);
			_collapsePowerOnTreeCmd = new RelayCommand(CollapsePowerOnTree, () => true);

			_messenger.Register<IsTrendOnPlotChangedMessage>(this, IsTrendOnPlotChanged);
		}

		private void CollapseTree() {
			foreach (var loc in Repository.Locomotives) {
				foreach (var sec in loc.Sections) {
					foreach (var plvm in sec.PsnLogs) {
						plvm.IsTreeViewExpanded = false;
						foreach (var pmvm in plvm.PsnMeters) {
							pmvm.IsTreeViewExpanded = false;
							
						}
					}
				}
			}
		}

		private void CollapsePowerOnTree() {
			foreach (var loc in Repository.Locomotives) {
				foreach (var sec in loc.Sections) {
					foreach (var plvm in sec.PsnPowerOnLogs) {
						plvm.IsTreeViewExpanded = false;
						foreach (var pmvm in plvm.PsnMeters) {
							pmvm.IsTreeViewExpanded = false;
						}
					}
				}
			}
		}


		private void InitializeCommands() {
			ShowAddDataCommand = new RelayCommand(ShowAddDataExecute);
			ShowSettings = new RelayCommand(ShowSettingsExecute);
			Close = new RelayCommand(CloseExecute);
			Initialize = new RelayCommand(InitializeExecute);
			TestCommand = new RelayCommand(TestCommandExecute);
			ShowRpdConfigurator = new RelayCommand(ShowRpdConfiguratorExecute);
			ShowDefaultColorSettings = new RelayCommand(ShowDefaultColorSettingsExecute);
			ShowHelpWindow = new RelayCommand(ShowHelpWindowExecute);
			ShowAboutProgramWindow = new RelayCommand(ShowAboutProgramWindowExecute);
			RemoveAllRpdSignalsFromChart = new RelayCommand(RemoveAllRpdSignalsFromChartExecute);
			RemoveAllPsnSignalsFromChart = new RelayCommand(RemoveAllPsnSignalsFromChartExecute);
			SaveSelectionMaskCommand = new RelayCommand(SaveSelectionMaskCommandExecute);
			SetContextMenuTrendsContainerCommand = new RelayCommand<ITrendsContainer>(SetContextMenuTrendsContainerExecute);
			LoadSelectionMaskCommand = new RelayCommand<ISelectionMask>(LoadSelectionMaskCommandExecute);
			LoadAllTrendsCommand = new RelayCommand(LoadAllTrendsCommandExecute);
			RemoveAllPsnPowerOnSignalsFromChart = new RelayCommand(RemoveAllPsnPowerOnSignalsFromChartExecute);
			ExportDataCommand = new RelayCommand(ExportDataCommandExecute);
			RemoveDataCommand = new RelayCommand(RemoveDataCommandExecute);
			ChangeConfigurationCommand = new RelayCommand(ChangeConfigurationCommandExecute);
			SetContextMenuPsnLogCommand = new RelayCommand<IPsnLogViewModel>(SetContextMenuPsnLogCommandExecute);

			ShowIntegrityErrorsStatistics = new RelayCommand(ShowIntegrityErrorsStatisticsExecute);
			RenamePsnLogCmd = new RelayCommand(RenamePsnLogExecute);

			SeriesColorChangedEventCommand = new RelayCommand<SeriesColorChangedEventArgs>(SeriesColorChangedEventCommandExecute);
			UncheckAllSignalsCommand = new RelayCommand(UncheckAllSignalsExecute);
		}


		public void SaveState() {
			var state = new WorkspacePdo();

			foreach (var seriesAdditionalData in MainChart.AnalogSeriesAdditionalData) {
				var series = (SeriesAdditionalData)seriesAdditionalData;
				state.MainChartAnalogSeries.Add(new TrendPdo { Uid = series.TrendViewModel.Uid });
			}

			foreach (var seriesAdditionalData in MainChart.DiscreteSeriesAdditionalData) {
				var series = (SeriesAdditionalData)seriesAdditionalData;
				state.MainChartDiscreteSeries.Add(new TrendPdo { Uid = series.TrendViewModel.Uid });
			}

			var outputJson = JsonConvert.SerializeObject(state, Formatting.Indented);
			File.WriteAllText(_applicationSettings.WorkspacesFullFilePath, outputJson);
		}

		public void LoadState() {
			if (!File.Exists(_applicationSettings.WorkspacesFullFilePath))
				return;

			var json = File.ReadAllText(_applicationSettings.WorkspacesFullFilePath);
			var state = JsonConvert.DeserializeObject<WorkspacePdo>(json);

			foreach (var series in state.MainChartAnalogSeries) {
				var trend = Repository.FindTrend(series.Uid);
				if (trend != null) {
					trend.IsOnPlot = true;
					//AddTrendToPlot(MainChart, trend);
				}
			}

			_messenger.Send(new ViewModelToViewMessage(), ViewModelToViewMessage.LoadWorkspaceState);
		}


		private void InitializeExecute() {
			if (!_applicationSettings.IsRepositoryPathSetted || _applicationSettings.IsRepositoryPathSetted && !Directory.Exists(_applicationSettings.RepositoryPath)) {
				// Отображаем окно настроек, где юзеру предлагается указать путь к хранилищу.
				_messenger.Send(new MessageWithCallback(LoadRepository), Views.Views.Settings);
			}
			else {
				LoadRepository();
			}
		}


		/// <summary>
		/// Выставлен/снят чекбокс с канала в хранилище.
		/// </summary>
		/// <param name="message"></param>
		private void IsTrendOnPlotChanged(IsTrendOnPlotChangedMessage message) {
			AddTrendToPlot(MainChart, message.Trend);
			/*
			switch (message.Trend.TrendChartType)
			{
					case TrendChartType.Psn:
							AddTrendToPlot(MainChart, message.Trend);
							break;
							/*
					case TrendChartType.PsnFault:
							AddTrendToPlot(PsnFualtsChart, message.Trend);
							break;
					case TrendChartType.Rpd: 
							AddTrendToPlot(RpdChart, message.Trend);
							break;                  
			}
			*/
		}

		private void AddTrendToPlot(ISciChartViewModel sciChartViewModel, ITrendViewModel trend) {
			if (trend.IsOnPlot) {
				IsTrendLoading = true;
				trend.IsTrendLoading = true;

				sciChartViewModel.AddTrend(trend,
						result => {
							IsTrendLoading = false;
							trend.IsTrendLoading = false;
						});
			}
			else {
				sciChartViewModel.RemoveTrend(trend);
			}
		}


		private void TestCommandExecute() {
			SaveState();
			_messenger.Send(new ViewModelToViewMessage(), ViewModelToViewMessage.SaveWorkspaceState);
		}


		private void SeriesColorChangedEventCommandExecute(SeriesColorChangedEventArgs obj) {
			MainChart.SeriesColorChanged(obj.Series, obj.Color);
		}


		private void LoadRepository() {
			Logger.Trace("before load repository");
			_repository = _loader.GetLocalDirectoryRepository(_applicationSettings.RepositoryPath);
			_repository.Open(OnRepositoryOpenComplete, OnProgressChange);
		}


		private void OnProgressChange(OnProgressChangeEventArgs onProgressChangeEventArgs) {
			;
		}


		private void OnRepositoryOpenComplete(OnCompleteEventArgs args) {
			Logger.Trace("OnRepositoryOpenComplete");

			if (args.ResultCode != OnCompleteEventArgs.CompleteResult.Ok) {
				// Сообщаем об ошибке
				_messenger.Send(
						new DialogMessage(this, args.Message, res => { }) {
							Button = MessageBoxButton.OK,
							Icon = MessageBoxImage.Error,
							Caption = "Ошибка инициализации хранилища"
						}, AppMessages.ErrorDialogMessage);
				return;
			}

			Repository = _repositoryViewModelFactory.Create(_repository);
			IsRepositoryLoaded = true;

			_container.RegisterInstance(_repository);

			DefualtColorsViewModel.GenerateDefaultColorsIfNeed(_applicationSettings, _loader, _colorsStorage);

			//LoadState();
		}


		private void SaveSelectionMask(string containerName, SelectionMask mask) {
			if (!SelectionMasksStorage.SelectionMasks.ContainsKey(containerName))
				SelectionMasksStorage.SelectionMasks[containerName] = new SelectionMasksCollection();

			if (!SelectionMasksStorage.SelectionMasks[containerName].Contains(mask)) {
				SelectionMasksStorage.SelectionMasks[containerName].Items.Add(mask);
				SelectionMasksStorage.Save(_applicationSettings.SelectionMasksFullFilePath);
			}
		}


		private void ShowAddDataExecute() {
			_messenger.Send(new ViewMessage(ViewAction.Show), Views.Views.AddData);
		}


		private void ShowSettingsExecute() {
			_messenger.Send(new ViewMessage(ViewAction.ShowModal), Views.Views.Settings);
		}


		private void LoadAllTrendsCommandExecute() {
			foreach (var trendViewModel in ContextMenuTrendsContainer.Trends) {
				trendViewModel.IsOnPlot = true;
			}
		}


		private void SetContextMenuTrendsContainerExecute(ITrendsContainer meterViewModel) {
			ContextMenuTrendsContainer = meterViewModel;
		}


		private void ShowIntegrityErrorsStatisticsExecute() {
			_messenger.Send(new ViewMessage(ViewAction.Show), Views.Views.LogIntegrityStatWindow);
			_messenger.Send(new SetViewModelParametersMessage<IPsnLogViewModel>(ContextMenuPsnLog));
		}


		private void RenamePsnLogExecute() {
			_messenger.Send(new SetViewModelParametersMessage<IPsnLogViewModel>(ContextMenuPsnLog));
			_contextMenuPsnLog.IsInEditLabelMode = true;
		}


		private void SetContextMenuPsnLogCommandExecute(IPsnLogViewModel psnLog) {
			ContextMenuPsnLog = psnLog;
		}


		private void SaveSelectionMaskCommandExecute() {
			var mask = new SelectionMask();

			int i = 0, savedCount = 0;

			foreach (var trendViewModel in ContextMenuTrendsContainer.Trends) {
				if (trendViewModel.IsOnPlot) {
					mask.Items.Add(new SelectionItem {
						Number = i,
						Name = trendViewModel.Name,
					});
					savedCount++;
				}

				i++;
			}

			if (savedCount == 0)
				return;

			SaveSelectionMask(ContextMenuTrendsContainer.Name, mask);
			RaisePropertyChanged(() => SelectionMasksStorage);
			RaisePropertyChanged(() => SelectionMasksStorage.SelectionMasks);
		}


		private void LoadSelectionMaskCommandExecute(ISelectionMask selectionMask) {
			int i = 0;
			foreach (var trendViewModel in ContextMenuTrendsContainer.Trends) {
				trendViewModel.IsOnPlot = selectionMask.ContainsItemNumber(i);
				i++;
			}
		}


		private void CloseExecute() {
			_messenger.Send(new AppMessages(), AppMessages.CloseApplication);
		}


		private void ExportDataCommandExecute() {
			// значит что команда вызывается в первый раз, т.е. когда кнопка нажата, а нужно когда уже отжата.
			if (IsExportDataMode)
				return;

			var dataToExport = new ExportLogsParametersBase();
			FillLogsParametersAndUncheckThem(dataToExport);
			if (dataToExport.PsnLogs.Count == 0 && dataToExport.RpdLogs.Count == 0)
				return;

			var msg = new DialogMessage<SaveFileDialog>(this,
					new SaveFileDialog {
						AddExtension = true,
						InitialDirectory = _applicationSettings.LastExportFolder,
						CheckPathExists = true,
						AutoUpgradeEnabled = true,
						DefaultExt = "*.rpd",
						OverwritePrompt = true,
						Title = "Экспорт",
						Filter = Resources.ExportRpdData_Dialog_Filter
					},
					resultCallback => {
						if (resultCallback.DialogResult == DialogResult.OK) {
							dataToExport.FileName = resultCallback.Dialog.FileName;
							_applicationSettings.LastExportFolder = Path.GetDirectoryName(dataToExport.FileName);
							ShowExportProgress(dataToExport);
						}
					});

			_messenger.Send(msg);
		}


		private void RemoveDataCommandExecute() {
			// значит что команда вызывается в первый раз, т.е. когда кнопка нажата, а нужно когда уже отжата.
			if (IsRemoveDataMode)
				return;

			var dataToRemove = new LogsParameters();
			FillLogsParametersAndUncheckThem(dataToRemove);
			if (dataToRemove.PsnLogs.Count == 0 && dataToRemove.RpdLogs.Count == 0)
				return;

			var msg = new MessageBoxMessage("Удалить выбранные логи?", "РПД", MessageBoxButton.YesNo,
					MessageBoxImage.Question,
					resultCallback => {
						if (resultCallback == MessageBoxResult.Yes) {
							_messenger.Send(new ViewMessage(ViewAction.Show), Views.Views.ExportProgress);
							_messenger.Send(new SetViewModelParametersMessage<LogsParameters>(dataToRemove));
						}
					});

			_messenger.Send(msg);
		}


		private void ChangeConfigurationCommandExecute() {
			// значит что команда вызывается в первый раз, т.е. когда кнопка нажата, а нужно когда уже отжата.
			if (IsChangeConfigurationMode)
				return;

			var logs = new LogsParameters();
			FillLogsParametersAndUncheckThem(logs);
			if (logs.PsnLogs.Count == 0 && logs.RpdLogs.Count == 0)
				return;

			_messenger.Send(new ViewMessage(ViewAction.Show), Views.Views.ChangePsnConfiguration);
			_messenger.Send(new SetViewModelParametersMessage<LogsParameters>(logs));
		}


		private void ShowExportProgress(ExportLogsParametersBase dataToExportLogs) {
			_messenger.Send(new ViewMessage(ViewAction.Show), Views.Views.ExportProgress);
			_messenger.Send(new SetViewModelParametersMessage<ExportLogsParametersBase>(dataToExportLogs));
		}


		private void UncheckAllSignalsExecute() {
			foreach (var loco in Repository.Locomotives) {
				foreach (var sec in loco.Sections) {
					foreach (var log in sec.PsnLogs) {
						if (log.IsChecked)
							log.IsChecked = false;
					}

					foreach (var log in sec.PsnPowerOnLogs) {
						if (log.IsChecked)
							log.IsChecked = false;
					}

					foreach (var log in sec.Faults) {
						if (log.IsChecked)
							log.IsChecked = false;
					}
				}
			}
		}


		private void FillLogsParametersAndUncheckThem(LogsParameters parametersBase) {
			parametersBase.Repository = _repository;

			foreach (var loco in Repository.Locomotives) {
				foreach (var sec in loco.Sections) {
					foreach (var log in sec.PsnLogs) {
						if (log.IsChecked) {
							log.IsChecked = false;
							parametersBase.PsnLogs.Add(log.PsnLog);
						}
					}

					foreach (var log in sec.PsnPowerOnLogs) {
						if (log.IsChecked) {
							log.IsChecked = false;
							parametersBase.PsnLogs.Add(log.PsnLog);
						}
					}

					foreach (var log in sec.Faults) {
						if (log.IsChecked) {
							log.IsChecked = false;
							parametersBase.RpdLogs.Add(log.Fault);
						}
					}
				}
			}
		}


		private void ShowRpdConfiguratorExecute() {
			var deviceConfig = _loader.CreateDeviceConfiguration();

			deviceConfig.Read(App.AppPath + "\\" + _applicationSettings.DefaultDeviceConfigurationDir,
					(arg) => {
						// TODO: fix RPD configurator
						// Сборки RPD.Configurator и RPD.Presentation зависят друг от друга. 
						//var configurator = new RpdConfigurator(deviceConfig);
						//configurator.Show();

					});
		}


		private void ShowDefaultColorSettingsExecute() {
			_messenger.Send(new ViewMessage(ViewAction.Show), Views.Views.DefaultColorSettings);
		}


		private void ShowHelpWindowExecute() {
			_messenger.Send(new ViewMessage(ViewAction.Show), Views.Views.HelpWindow);
		}


		private void ShowAboutProgramWindowExecute() {
			_messenger.Send(new ViewMessage(ViewAction.Show), Views.Views.AboutProgramWindow);
		}


		private void RemoveAllRpdSignalsFromChartExecute() {
			foreach (var loc in Repository.Locomotives) {
				foreach (var sec in loc.Sections) {
					foreach (var fault in sec.Faults) {
						foreach (var meter in fault.RpdMeters) {
							foreach (var channel in meter.Channels) {
								channel.IsOnPlot = false;
							}
						}
					}
				}
			}
		}


		private void RemoveAllPsnSignalsFromChartExecute() {
			foreach (var loc in Repository.Locomotives) {
				foreach (var sec in loc.Sections) {
					foreach (var psn in sec.PsnLogs) {
						foreach (var meter in psn.PsnMeters) {
							foreach (var channel in meter.PsnChannels) {
								channel.IsOnPlot = false;
							}
						}
					}
				}
			}
		}


		private void RemoveAllPsnPowerOnSignalsFromChartExecute() {
			foreach (var loc in Repository.Locomotives) {
				foreach (var sec in loc.Sections) {
					foreach (var psn in sec.PsnPowerOnLogs) {
						foreach (var meter in psn.PsnMeters) {
							foreach (var channel in meter.PsnChannels) {
								channel.IsOnPlot = false;
							}
						}
					}
				}
			}
		}


		public IPsnLogViewModel ContextMenuPsnLog {
			get => _contextMenuPsnLog;
			set { Set(() => ContextMenuPsnLog, ref _contextMenuPsnLog, value); }
		}

		public ITrendsContainer ContextMenuTrendsContainer {
			get => _contextMenuTrendsContainer;
			set { Set(() => ContextMenuTrendsContainer, ref _contextMenuTrendsContainer, value); }
		}

		public bool IsPopupOpen {
			get => _isPopupOpen;
			set { Set(() => IsPopupOpen, ref _isPopupOpen, value); }
		}

		public ISelectionMasksStorage SelectionMasksStorage { get; set; }

		public bool IsTrendLoading {
			get => _isTrendLoading;

			set {
				Set(() => IsTrendLoading, ref _isTrendLoading, value);
				RaisePropertyChanged(() => IsTrendNotLoading);
			}
		}


		public bool IsRepositoryLoaded {
			get => _isRepositoryLoaded;
			set {
				Logger.Debug("IsRepositoryLoaded={0}", value);
				Set(() => IsRepositoryLoaded, ref _isRepositoryLoaded, value);
			}
		}


		public bool IsTrendNotLoading => !IsTrendLoading;

		public bool IsExportDataMode {
			get => _isExportDataMode;
			set {
				Set(() => IsExportDataMode, ref _isExportDataMode, value);
				IsDataSelectMode = value;
				IsRemoveDataModeAvailable = !_isExportDataMode;
				IsChangeConfigurationModeAvailable = !_isExportDataMode;
			}
		}

		public bool IsRemoveDataMode {
			get => _isRemoveDataMode;
			set {
				Set(() => IsRemoveDataMode, ref _isRemoveDataMode, value);
				IsDataSelectMode = value;
				IsExportDataModeAvailable = !_isRemoveDataMode;
				IsChangeConfigurationModeAvailable = !_isRemoveDataMode;
			}
		}

		public bool IsChangeConfigurationMode {
			get => _isChangeConfigurationMode;
			set {
				Set(() => IsChangeConfigurationMode, ref _isChangeConfigurationMode, value);
				IsDataSelectMode = value;
				IsExportDataModeAvailable = !_isChangeConfigurationMode;
				IsRemoveDataModeAvailable = !_isChangeConfigurationMode;
			}
		}

		public bool IsExportDataModeAvailable {
			get => _isExportDataModeAvailable;
			set { Set(() => IsExportDataModeAvailable, ref _isExportDataModeAvailable, value); }
		}

		public bool IsRemoveDataModeAvailable {
			get => _isRemoveDataModeAvailable;
			set { Set(() => IsRemoveDataModeAvailable, ref _isRemoveDataModeAvailable, value); }
		}

		public bool IsChangeConfigurationModeAvailable {
			get => _isChangeConfigurationModeAvailable;
			set { Set(() => IsChangeConfigurationModeAvailable, ref _isChangeConfigurationModeAvailable, value); }
		}

		public bool IsDataSelectMode {
			get => _isDataSelectMode;
			set { Set(() => IsDataSelectMode, ref _isDataSelectMode, value); }
		}

		public bool IsDebugMode {
			get => _applicationSettings.IsDebugMode;
			set { }
		}


		/// <summary>
		/// Модель представления хранилища.
		/// </summary>
		public IRepositoryViewModel Repository {
			get => _repositoryViewModel;
			set { Set(() => Repository, ref _repositoryViewModel, value); }
		}

		public ISciChartViewModel MainChart {
			get => _mainChart;
			set { Set(() => MainChart, ref _mainChart, value); }
		}

		public object Test {
			get => _test;
			set { Set(() => Test, ref _test, value); }
		}

		public ICommand CollapseTreeCmd => _collapseTreeCmd;
		public ICommand CollapsePowerOnTreeCmd => _collapsePowerOnTreeCmd;
	}
}