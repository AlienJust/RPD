using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using AlienJust.Support.ModelViewViewModel;
using RPD.DAL;

namespace SampleApp {
	internal class PsnLogViewModel : ViewModelBase {
		private readonly IPsnLog _model;
		private readonly DependedCommand _cmdStartModifyName;
		private readonly DependedCommand _cmdSetName;

		private bool _isInEditMode;
		private string _newName;

		private readonly ObservableCollection<PsnDeviceViewModel> _psnDevices;
		public ICommand CmdGetPagesStatistic { get; }

		public PsnLogViewModel(IPsnLog model) {
			_model = model;

			_isInEditMode = false;
			_newName = _model.Name;
			_psnDevices = new ObservableCollection<PsnDeviceViewModel>();
			foreach (var meter in _model.Meters) {
				_psnDevices.Add(new PsnDeviceViewModel(meter));
			}

			_cmdStartModifyName = new DependedCommand(() => {
				IsInEditMode = true;
			}, () => !IsInEditMode);
			_cmdStartModifyName.AddDependOnProp(this, "IsInEditMode");


			_cmdSetName = new DependedCommand(() => {
				IsInEditMode = false;
				Name = _newName;
			}, () => IsInEditMode);
			_cmdSetName.AddDependOnProp(this, "IsInEditMode");

			CmdGetPagesStatistic = new RelayCommand(() => {
				_model.GetStatisticAsync((ex, text) => {
					if (ex != null)
						MessageSystem.ShowMessage(ex.ToString());
					else {
						MessageSystem.ShowMessage(text.Aggregate(string.Empty, (current, line) => current + (line + Environment.NewLine)));
					}
				});
			});

			model.IsSomethingWrongWithLogChanged += (sender, args) => RaisePropertyChanged(() => LogHealth);
		}

		public string Name {
			get => _model.Name;
			private set {
				if (_model.Name != value) {
					_model.Update(_model.PsnConfiguration, value);
					RaisePropertyChanged(() => Name);
				}
			}
		}

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

		public string NewName {
			get => _newName;
			set {
				if (_newName != value) {
					_newName = value;
					RaisePropertyChanged(() => NewName);
				}
			}
		}

		public ObservableCollection<PsnDeviceViewModel> PsnDevices => _psnDevices;

		

		public string LogHealth {
			get {

				switch (_model.LogIntegrity) {
					case PsnLogIntegrity.Ok:
						return "Лог в полном порядке";
					case PsnLogIntegrity.PagesFlowError:
						return "Есть ошибки потока страниц (см сводку статистики)";
					case PsnLogIntegrity.Unknown:
						return "Неизвестно";
					default:
						return "Неизвестное значение инфомрации";
				}
			}
		}
	}
}