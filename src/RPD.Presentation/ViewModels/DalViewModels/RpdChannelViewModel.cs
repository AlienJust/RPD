using System;
using RPD.DAL;
using RPD.Presentation.Contracts.ViewModels;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;

namespace RPD.Presentation.ViewModels.DalViewModels {
	/// <summary>
	/// Модель представления канала.
	/// Generates Messages: IsTrendOnPlotChangedMessage&lt;RpdChannelViewModel&gt;
	/// </summary>
	class RpdChannelViewModel : TrendViewModelBase<RpdChannelViewModel>, IRpdChannelViewModel {
		readonly IRpdChannel _channel;

		public RpdChannelViewModel(IRpdChannel channel, RpdMeterViewModel meter) {
			_channel = channel;
			TrendChartType = TrendChartType.Rpd;
			Meter = meter;

			Faults.Add(meter.Fault);
		}

		public IRpdMeterViewModel Meter { get; set; }

		/// <summary>
		/// Название канала.
		/// </summary>
		public override string Name => _channel.Name;

		public override Guid Uid => throw new NotImplementedException();

		public override Guid ConfigGuid => Guid.Empty;

		public override TrendChartType TrendChartType { get; set; }

		#region Реализация ITrendViewModel

		public override string Title => _channel.OwnerMeter.OwnerFault.Name + ", " + _channel.OwnerMeter.Name + ", " + _channel.Name;


		public override bool IsEnabled {
			get => _channel.IsEnabled;
			set {; }
		}

		public override ILazyTrendData TrendData => _channel;

		#endregion // Реализация TrendViewModelBase
	}
}
