using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using RPD.DAL;
using RPD.Presentation.Contracts;
using RPD.Presentation.Contracts.ViewModels;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;

namespace RPD.Presentation.ViewModels.DalViewModels.DesignTime {
	class PsnMeterDesignTime : ViewModelBase, ITrendsContainer, IPsnMeterViewModel {
		public PsnMeterDesignTime() {
			Name = "ТП ХЗ";

			PsnChannels = new ObservableCollection<IPsnChannelViewModel>();
			/*
			PsnChannels.Add(new PsnChannelDesignTime("Канал 1"));
			PsnChannels.Add(new PsnChannelDesignTime("Канал 2"));
			PsnChannels.Add(new PsnChannelDesignTime("Канал 3"));
			PsnChannels.Add(new PsnChannelDesignTime("Канал 4"));
			PsnChannels.Add(new PsnChannelDesignTime("Канал 5"));
			PsnChannels.Add(new PsnChannelDesignTime("Канал 6"));
			*/
		}

		#region Implementation of ITrendsContainer

		public IPsnMeter Model => throw new System.NotImplementedException();

		public string Name { get; }

		public string AddressString { get; private set; }
		public ObservableCollection<IPsnChannelViewModel> PsnChannels { get; }

		public IEnumerable<ITrendViewModel> Trends => PsnChannels;

		#endregion

		public bool IsTreeViewExpanded { get; set; }
	}
}
