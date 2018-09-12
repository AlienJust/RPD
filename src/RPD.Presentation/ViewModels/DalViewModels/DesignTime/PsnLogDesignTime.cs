using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using RPD.DAL;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;
using RPD.Presentation.Settings;

namespace RPD.Presentation.ViewModels.DalViewModels.DesignTime {
	class PsnLogDesignTime : ViewModelBase, IPsnLogViewModel {
		public PsnLogDesignTime() {
			var end = DateTime.Now;
			var begin = end - TimeSpan.FromMinutes(20);

			Name = begin.ToShortDateString() + " " +
						 begin.ToString(Formats.TimeFormat) + " - " +
						 end.ToShortDateString() + " " +
						 end.ToString(Formats.TimeFormat);

			PsnMeters = new ObservableCollection<IPsnMeterViewModel>();
			//PsnMeterConfigs.Add(new PsnMeterDesignTime());
		}

		#region Implementation of IPsnLogViewModel

		public IPsnLog PsnLog { get; set; }
		public bool IsChecked { get; set; }
		public bool IsSavedToRepository { get; set; }
		public bool IsNew { get; set; }
		public bool IsLastDeviceLog { get; }
		public string Name { get; }
		public string Label { get; set; }
		public ObservableCollection<IPsnMeterViewModel> PsnMeters { get; }
		public bool HasIntegrityErrors { get; set; }
		public string Hint { get; }
		public bool IsInEditLabelMode { get; set; }
		#endregion

		public bool IsTreeViewExpanded { get; set; }
	}
}
