using System;
using GalaSoft.MvvmLight.Command;

namespace RPD.Presentation.ViewModels.AddDataViewModels.SelectPathHelperViewModels {
	/// <summary>
	/// Вспомогательный интерфейс для выбора пути к данным.
	/// </summary>
	internal interface ISelectPathHelperViewModel {
		/// <summary>
		/// Текст всплывающей подсказки.
		/// </summary>
		string ToolTipText { get; }

		string Label { get; }

		/// <summary>
		/// Путь который был выбран при предыдущем отображении окна или путь по умолчанию.
		/// </summary>
		string LastPath { get; set; }

		RelayCommand BackCommand { get; }
		RelayCommand NextCommand { get; }

		/// <summary>
		/// Отобразить диалог выбора пути.
		/// </summary>
		/// <param name="browseResult">Действие будет вызвано когда юзер выберет путь.</param>
		void ShowBrowseDialog(Action<string> browseResult);
	}
}
