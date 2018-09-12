using GalaSoft.MvvmLight;
using RPD.Presentation.Contracts.ViewModels;

namespace RPD.Presentation.ViewModels.DalViewModels
{
	/// <summary>
	/// Base class for tree view item, that allows to control "IsExpanded" property of items
	/// </summary>
	internal class TreeItemViewModelBase : ViewModelBase, ITreeViewItemViewModel {
		private bool _isNodeExpanded;


		/// <inheritdoc />
		public bool IsTreeViewExpanded {
			get => _isNodeExpanded;
			set {
				if (_isNodeExpanded != value) {
					_isNodeExpanded = value;
					RaisePropertyChanged(() => IsTreeViewExpanded);
				}
			}
		}
	}
}