using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RPD.Presentation.Messages;

namespace RPD.Presentation.ViewModels.AddDataViewModels {
	internal interface ISelectDataSourceViewModel {
		bool IsSimpleMode { get; }

		/// <summary>
		/// Считать данные из флеш.
		/// </summary>
		RelayCommand ReadFromFlashCommand { get; set; }

		/// <summary>
		/// Считать данные из образа.
		/// </summary>
		RelayCommand ReadFromImageCommand { get; set; }

		/// <summary>
		/// Считать данные из папки.
		/// </summary>
		RelayCommand ReadFromFolderCommand { get; set; }

		RelayCommand ReadFromFtpServerCommand { get; set; }
	}

	/// <summary>
	/// Модель представления окна выбора источника, из которого будут считываться аварии.
	/// </summary>
	internal class SelectDataSourceViewModel : ViewModelBase, ISelectDataSourceViewModel {
		private readonly Action<AddDataWindowMessages> _navigateAction;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="navigateAction">Вызывается когда пользователь нажимает одну из кнопок перехода на другой вид.</param>
		/// <param name="isSimpleMode">В этом режиме не отображает кнопка импорта данных. </param>
		public SelectDataSourceViewModel(Action<AddDataWindowMessages> navigateAction, bool isSimpleMode) {
			_navigateAction = navigateAction;
			IsSimpleMode = isSimpleMode;

			ReadFromFlashCommand = new RelayCommand(ReadFromFlashExecute, () => true);
			ReadFromImageCommand = new RelayCommand(ReadFromImageExecute, () => true);
			ReadFromFolderCommand = new RelayCommand(ReadFromFolderExecute, () => true);
			ReadFromFtpServerCommand = new RelayCommand(ReadFromFtpServerExecute, () => true);
		}

		#region Commands

		public bool IsSimpleMode { get; }

		/// <summary>
		/// Считать данные из флеш.
		/// </summary>
		public RelayCommand ReadFromFlashCommand { get; set; }

		void ReadFromFlashExecute() {
			_navigateAction(AddDataWindowMessages.SelectUsbDevicePage);
		}

		/// <summary>
		/// Считать данные из образа.
		/// </summary>
		public RelayCommand ReadFromImageCommand { get; set; }

		void ReadFromImageExecute() {
			_navigateAction(AddDataWindowMessages.SelectZippedRepositoryPage);
		}

		/// <summary>
		/// Считать данные из папки.
		/// </summary>
		public RelayCommand ReadFromFolderCommand { get; set; }

		void ReadFromFolderExecute() {
			_navigateAction(AddDataWindowMessages.SelectFolderPage);
		}

		public RelayCommand ReadFromFtpServerCommand { get; set; }

		void ReadFromFtpServerExecute() {
			_navigateAction(AddDataWindowMessages.SelectFtpServerPage);
		}

		#endregion //Commands
	}
}