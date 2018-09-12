using System.Collections.ObjectModel;
using RPD.DAL;
using GalaSoft.MvvmLight;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;

namespace RPD.Presentation.ViewModels.DalViewModels {
	/// <summary>
	/// Модель представления измерителя.
	/// </summary>
	class RpdMeterViewModel : ViewModelBase, IRpdMeterViewModel {
		readonly IRpdMeter _meter;

		public RpdMeterViewModel(IRpdMeter meter, FaultViewModel fault) {
			Fault = fault;
			_meter = meter;
			FillChannels();
		}

		void FillChannels() {
			for (int i = 0; i < _meter.Channels.Count; ++i)
				Channels.Add(new RpdChannelViewModel(_meter.Channels[i], this));
		}

		#region Presentation Members

		public IFaultViewModel Fault { get; set; }

		public string Name => _meter.Name;


		/// <summary>
		/// Список каналов на измерителе.
		/// </summary>
		public ObservableCollection<IRpdChannelViewModel> Channels { get; } = new ObservableCollection<IRpdChannelViewModel>();

		#endregion //Presentation Members
	}
}
