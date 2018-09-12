using System.Collections.Generic;
using NLog;
using RPD.DAL;
using System.Collections.ObjectModel;
using RPD.Presentation.Contracts;
using RPD.Presentation.Contracts.ViewModels;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;

namespace RPD.Presentation.ViewModels.DalViewModels {
	/// <summary>
	/// Измеритель ПСН. Интерфейс ITrendsContainer частично дублирует IPsnMeterViewModel  
	/// </summary>
	class PsnMeterViewModel : TreeItemViewModelBase, ITrendsContainer, IPsnMeterViewModel {
		private static Logger logger = LogManager.GetCurrentClassLogger();

		public PsnMeterViewModel(IPsnMeter psnMeter, TrendChartType trendChartType) {
			_psnMeter = psnMeter;
			FillPsnChannels(psnMeter, trendChartType);
		}

		private void FillPsnChannels(IPsnMeter psnMeter, TrendChartType trendChartType) {
			foreach (var channel in psnMeter.Channels) {
				var c = new PsnChannelViewModel(channel, trendChartType);
				PsnChannels.Add(c);
				//logger.Debug(c.Uid);
			}
		}

		public IPsnMeter Model => _psnMeter;

		#region Presentation Members

		public string Name => _psnMeter.Name;

		public ObservableCollection<IPsnChannelViewModel> PsnChannels => _psnChannels;

		public IEnumerable<ITrendViewModel> Trends => _psnChannels;

		#endregion // Presentation Members


		#region Private Fields

		private readonly IPsnMeter _psnMeter;
		private readonly ObservableCollection<IPsnChannelViewModel> _psnChannels = new ObservableCollection<IPsnChannelViewModel>();

		#endregion // Private Fields
	}
}