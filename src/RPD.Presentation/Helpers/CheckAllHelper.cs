using System.Collections;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RPD.Presentation.Contracts.ViewModels;

namespace RPD.Presentation.Helpers {
	/// <summary>
	/// Помогает реализовать функционал "Выделить всё" и "Снять выделение".
	/// </summary>
	internal class CheckAllHelper : ViewModelBase {
		public CheckAllHelper() {
			SelectAllCommand = new RelayCommand<IEnumerable>(SelectAllCommandExecute);
			UnselectAllCommand = new RelayCommand<IEnumerable>(UnselectAllCommandExecute);
		}

		/// <summary>
		/// Команда "выделяет" все элементы. В качестве параметра передаётся коллекция элементов реализующих интерфейс ICheckableViewModel.
		/// </summary>
		public RelayCommand<IEnumerable> SelectAllCommand { get; set; }

		void SelectAllCommandExecute(IEnumerable collection) {
			foreach (var item in collection) {
				var vm = item as ICheckableViewModel;
				if (vm != null)
					vm.IsChecked = true;
			}
		}

		/// <summary>
		/// Команда "снимает выделение" со всех элементов. В качестве параметра передаётся коллекция элементов реализующих интерфейс ICheckableViewModel.
		/// </summary>
		public RelayCommand<IEnumerable> UnselectAllCommand { get; set; }

		void UnselectAllCommandExecute(IEnumerable collection) {
			foreach (var item in collection) {
				var vm = item as ICheckableViewModel;
				if (vm != null)
					vm.IsChecked = false;
			}
		}
	}
}
