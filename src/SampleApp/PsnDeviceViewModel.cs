using System.Collections.ObjectModel;
using AlienJust.Support.ModelViewViewModel;
using RPD.DAL;

namespace SampleApp
{
	class PsnDeviceViewModel : ViewModelBase
	{
		private readonly IPsnMeter _model;
		private readonly ObservableCollection<PsnSignalViewModel> _psnSignals;
		public PsnDeviceViewModel(IPsnMeter model) {
			_model = model;

			_psnSignals = new ObservableCollection<PsnSignalViewModel>();
			foreach (var psnChannel in _model.Channels) {
				_psnSignals.Add(new PsnSignalViewModel(psnChannel));
			}
		}

		public string Name => _model.Name;

		public ObservableCollection<PsnSignalViewModel> PsnSignals => _psnSignals;
	}
}
