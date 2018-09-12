using System;
using RPD.DAL;
using RPD.Presentation.Contracts.ViewModels;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;

namespace RPD.Presentation.ViewModels.DalViewModels {
	/// <summary>
	/// Generates Messages: IsTrendOnPlotChangedMessage&lt;PsnChannelViewModel&gt;
	/// </summary>
	sealed class PsnChannelViewModel : TrendViewModelBase<PsnChannelViewModel>, IPsnChannelViewModel {
		readonly IPsnChannel _psnChannel;

		public PsnChannelViewModel(IPsnChannel psnChannel, TrendChartType trendChartType) {
			_psnChannel = psnChannel;
			TrendChartType = trendChartType;
		}


		#region Presentation Members

		public override string Name => _psnChannel.Name;

		#region Реализация ITrendViewModel

		public override Guid Uid => _psnChannel.UnicId;

		public override Guid ConfigGuid => _psnChannel.ConfigurationId;

		public override TrendChartType TrendChartType { get; set; }

		public override string Title => _psnChannel.OwnerPsnMeter.Name + ", " + _psnChannel.Name + " (" + TrendData.Trend.Count + ")";

		public override ILazyTrendData TrendData => _psnChannel;

		public override bool IsEnabled {
			get => _psnChannel.IsEnabled;
			set {; }
		}

		#endregion // Реализация TrendViewModelBase


		#endregion // Presentation Members
	}
}