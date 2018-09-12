using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using AlienJust.Support.ModelViewViewModel;
using RPD.DAL;

namespace SampleApp {
	internal class LocomotiveViewModel : ViewModelBase {
		private readonly ILocomotive _model;
		private readonly ObservableCollection<SectionViewModel> _sections;

		private readonly DependedCommand _cmdStartModifyName;
		private readonly DependedCommand _cmdSetName;

		private bool _isInEditMode;
		private string _newName;


		public LocomotiveViewModel(ILocomotive model) {
			_model = model;
			_sections = new ObservableCollection<SectionViewModel>();
			foreach (var section in model.Sections)
			{
				_sections.Add(new SectionViewModel(section));
			}

			_isInEditMode = false;
			_newName = _model.Name;



			_cmdStartModifyName = new DependedCommand(() => {
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

		public string Name {
			get => _model.Name;
			set {
				if (_model.Name != value) {
					_model.SetName(value);
					RaisePropertyChanged(() => Name);
				}
			}
		}

		public ObservableCollection<SectionViewModel> Sections => _sections;

		public bool IsInEditMode {
			get => _isInEditMode;
			set {
				if (_isInEditMode != value) {
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
				if (_newName != value) {
					_newName = value;
					RaisePropertyChanged(() => NewName);
				}
			}
		}
	}
}