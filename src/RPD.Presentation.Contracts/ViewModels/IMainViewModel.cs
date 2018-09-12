using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using RPD.Presentation.Contracts.Model.SelectionMasks;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;
using RPD.SciChartControl.Events;

namespace RPD.Presentation.Contracts.ViewModels {
	public interface IMainViewModel {
		/// <summary>
		/// Вызывает диалог добавления данных в проект.
		/// </summary>
		RelayCommand ShowAddDataCommand { get; set; }

		/// <summary>
		/// Показать окно настроек.
		/// </summary>
		RelayCommand ShowSettings { get; set; }

		/// <summary>
		/// Сохранить шаблон выделения сигналов на измерителе. 
		/// </summary>
		RelayCommand SaveSelectionMaskCommand { get; set; }

		/// <summary>
		/// Загрузить сигналы из шаблона выделения.
		/// </summary>
		RelayCommand<ISelectionMask> LoadSelectionMaskCommand { get; set; }

		/// <summary>
		/// Устанавливает измеритель ПСН, который будет являться контекстом для контекстного меню измерителя. 
		/// </summary>
		RelayCommand<ITrendsContainer> SetContextMenuTrendsContainerCommand { get; set; }

		/// <summary>
		/// Загрузить все сигналы на измерителе.
		/// </summary>
		RelayCommand LoadAllTrendsCommand { get; set; }

		/// <summary>
		/// Выйти из программы.
		/// </summary>
		RelayCommand Close { get; set; }

		RelayCommand ExportDataCommand { get; set; }
		RelayCommand RemoveDataCommand { get; set; }
		RelayCommand ChangeConfigurationCommand { get; set; }

		/// <summary>
		/// Начальная инициализация
		/// </summary>
		RelayCommand Initialize { get; set; }

		RelayCommand ShowRpdConfigurator { get; set; }
		RelayCommand TestCommand { get; set; }
		RelayCommand ShowHelpWindow { get; set; }
		RelayCommand ShowDefaultColorSettings { get; set; }
		RelayCommand ShowAboutProgramWindow { get; set; }

		RelayCommand RemoveAllRpdSignalsFromChart { get; set; }
		RelayCommand RemoveAllPsnSignalsFromChart { get; set; }
		RelayCommand RemoveAllPsnPowerOnSignalsFromChart { get; set; }

		RelayCommand UncheckAllSignalsCommand { get; set; }

		RelayCommand ShowIntegrityErrorsStatistics { get; set; }

		RelayCommand<IPsnLogViewModel> SetContextMenuPsnLogCommand { get; set; }

		RelayCommand<SeriesColorChangedEventArgs> SeriesColorChangedEventCommand { get; set; }

		IPsnLogViewModel ContextMenuPsnLog { get; set; }

		bool IsTrendLoading { get; set; }
		bool IsRepositoryLoaded { get; set; }
		bool IsTrendNotLoading { get; }

		bool IsExportDataMode { get; set; }
		bool IsRemoveDataMode { get; set; }
		bool IsChangeConfigurationMode { get; set; }

		bool IsExportDataModeAvailable { get; set; }
		bool IsRemoveDataModeAvailable { get; set; }
		bool IsChangeConfigurationModeAvailable { get; set; }

		/// <summary>
		/// Модель находится в режиме выбора данных для выделения (зажата кнопка экспорта, удаления или смены конфигурации).
		/// </summary>
		bool IsDataSelectMode { get; set; }

		bool IsDebugMode { get; set; }

		/// <summary>
		/// Модель представления хранилища.
		/// </summary>
		IRepositoryViewModel Repository { get; set; }

		ISciChartViewModel MainChart { get; set; }
		/*
		ISciChartViewModel PsnFualtsChart { get; set; }
		ISciChartViewModel RpdChart { get; set; }
		*/

		ISelectionMasksStorage SelectionMasksStorage { get; set; }

		/// <summary>
		/// Измеритель на котором было вызвано контекстное меню.
		/// </summary>
		ITrendsContainer ContextMenuTrendsContainer { get; set; }

		bool IsPopupOpen { get; set; }

		object Test { get; set; }

		ICommand CollapseTreeCmd { get; }
	}
}