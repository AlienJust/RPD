using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using RPD.Presentation.Contracts.Model.SelectionMasks;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;
using RPD.SciChartControl.Events;

namespace RPD.Presentation.Contracts.ViewModels {
	public interface IMainViewModel {
		/// <summary>
		/// �������� ������ ���������� ������ � ������.
		/// </summary>
		RelayCommand ShowAddDataCommand { get; set; }

		/// <summary>
		/// �������� ���� ��������.
		/// </summary>
		RelayCommand ShowSettings { get; set; }

		/// <summary>
		/// ��������� ������ ��������� �������� �� ����������. 
		/// </summary>
		RelayCommand SaveSelectionMaskCommand { get; set; }

		/// <summary>
		/// ��������� ������� �� ������� ���������.
		/// </summary>
		RelayCommand<ISelectionMask> LoadSelectionMaskCommand { get; set; }

		/// <summary>
		/// ������������� ���������� ���, ������� ����� �������� ���������� ��� ������������ ���� ����������. 
		/// </summary>
		RelayCommand<ITrendsContainer> SetContextMenuTrendsContainerCommand { get; set; }

		/// <summary>
		/// ��������� ��� ������� �� ����������.
		/// </summary>
		RelayCommand LoadAllTrendsCommand { get; set; }

		/// <summary>
		/// ����� �� ���������.
		/// </summary>
		RelayCommand Close { get; set; }

		RelayCommand ExportDataCommand { get; set; }
		RelayCommand RemoveDataCommand { get; set; }
		RelayCommand ChangeConfigurationCommand { get; set; }

		/// <summary>
		/// ��������� �������������
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
		/// ������ ��������� � ������ ������ ������ ��� ��������� (������ ������ ��������, �������� ��� ����� ������������).
		/// </summary>
		bool IsDataSelectMode { get; set; }

		bool IsDebugMode { get; set; }

		/// <summary>
		/// ������ ������������� ���������.
		/// </summary>
		IRepositoryViewModel Repository { get; set; }

		ISciChartViewModel MainChart { get; set; }
		/*
		ISciChartViewModel PsnFualtsChart { get; set; }
		ISciChartViewModel RpdChart { get; set; }
		*/

		ISelectionMasksStorage SelectionMasksStorage { get; set; }

		/// <summary>
		/// ���������� �� ������� ���� ������� ����������� ����.
		/// </summary>
		ITrendsContainer ContextMenuTrendsContainer { get; set; }

		bool IsPopupOpen { get; set; }

		object Test { get; set; }

		ICommand CollapseTreeCmd { get; }
	}
}