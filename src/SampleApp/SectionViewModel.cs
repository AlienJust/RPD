using System.Collections.ObjectModel;
using System.Windows.Input;
using AlienJust.Support.ModelViewViewModel;
using RPD.DAL;

namespace SampleApp {
	internal class SectionViewModel : ViewModelBase {
		private readonly ISection _model;

		private readonly DependedCommand _cmdStartModifyName;
		private readonly DependedCommand _cmdSetName;
		private readonly ObservableCollection<PsnLogViewModel> _psnLogs;

		private bool _isInEditMode;
		private string _newName;

		public SectionViewModel(ISection model) {
			_model = model;

			_isInEditMode = false;
			_newName = _model.Name;

			_psnLogs = new ObservableCollection<PsnLogViewModel>();
			foreach (var psnLog in _model.Psns) {
				_psnLogs.Add(new PsnLogViewModel(psnLog));
			}

			_cmdStartModifyName = new DependedCommand(() =>
			{
				IsInEditMode = true;
			}, () => !IsInEditMode);
			_cmdStartModifyName.AddDependOnProp(this, "IsInEditMode");


			_cmdSetName = new DependedCommand(() =>
			{
				IsInEditMode = false;
				Name = _newName;
			}, () => IsInEditMode);
			_cmdSetName.AddDependOnProp(this, "IsInEditMode");
		}

		public string Name
		{
			get => _model.Name;
			set
			{
				if (_model.Name != value)
				{
					_model.SetName(value);
					RaisePropertyChanged(() => Name);
				}
			}
		}

		public bool IsInEditMode
		{
			get => _isInEditMode;
			set
			{
				if (_isInEditMode != value)
				{
					_isInEditMode = value;
					RaisePropertyChanged(() => IsInEditMode);
					RaisePropertyChanged(() => IsNotInEditMode);
				}
			}
		}

		public bool IsNotInEditMode => !_isInEditMode;

		public ICommand CmdStartModifyName => _cmdStartModifyName;

		public ICommand CmdSetName => _cmdSetName;

		public string NewName
		{
			get => _newName;
			set
			{
				if (_newName != value)
				{
					_newName = value;
					RaisePropertyChanged(() => NewName);
				}
			}
		}

		public ObservableCollection<PsnLogViewModel> PsnLogs => _psnLogs;
	}
}