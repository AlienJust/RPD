using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using AlienJust.Support.Concurrent.Contracts;
using AlienJust.Support.ModelViewViewModel;
using AlienJust.Support.UserInterface.Contracts;
using RPD.DAL;
using RPD.EventArgs;

namespace SampleApp
{
	class MainWindowViewModel : ViewModelBase
	{
		private readonly IThreadNotifier _notifier;
		private readonly IWindowSystem _windows;
		private readonly IRepository _repository;
		private readonly ObservableCollection<LocomotiveViewModel> _locomotives;
		private readonly Loader _loader;
		private string _windowTitle;

		public MainWindowViewModel(IThreadNotifier notifier, IWindowSystem windows) {
			_notifier = notifier;
			_windows = windows;

			_loader = new Loader();
			_repository = _loader.GetLocalDirectoryRepository("C:\\Users\\aj01\\rpd.storage");
			_locomotives = new ObservableCollection<LocomotiveViewModel>();
			/*
			var importRepo = _loader.GetZippedRepository("C:\\Users\\aj01\\Downloads\\Mout.rpd");
			importRepo.Open(cea => {
				Console.WriteLine("Zip repo opened");
			}, pea => { });
			*/
			_repository.Open(ea => _notifier.Notify(() => {
				if (ea.ResultCode == OnCompleteEventArgs.CompleteResult.Ok) {
					foreach (var loc in _repository.Locomotives) {
						_locomotives.Add(new LocomotiveViewModel(loc));
					}
				}
				else {
					_windows.ShowMessageBox(ea.Message, "Error on repo opening");
				}
			}), e=>{Console.WriteLine(e.ProgressPercent.ToString("f2") + "%");});

			WindowTitle = "Max RPD Id = " + _loader.AvailablePsnConfigruations.Max(pc => pc.RpdId) + " TOTAL CONFIGS: " + _loader.AvailablePsnConfigruations.Count();
		}

		public string WindowTitle {
			get => _windowTitle;
			set {
				if (_windowTitle != value) {
					_windowTitle = value;
					RaisePropertyChanged(()=>WindowTitle);
				}
			}
		}
		public ObservableCollection<LocomotiveViewModel> Locomotives => _locomotives;
	}
}
