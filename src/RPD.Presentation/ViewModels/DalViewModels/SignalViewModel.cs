using System;
using RPD.DAL;
using RPD.Presentation.Contracts.ViewModels;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;

namespace RPD.Presentation.ViewModels.DalViewModels {
	/// <summary>
	/// Модель представления сигнала.
	/// </summary>
	class SignalViewModel : TrendViewModelBase<SignalViewModel>, ISignalViewModel {
		readonly ISignal _signal;

		public SignalViewModel(ISignal signal) {
			_signal = signal;
			TrendChartType = TrendChartType.Signal;
		}

		public string MathOperation { get; set; }

		#region Реализация TrendViewModelBase

		/// <summary>
		/// Название сигнала.
		/// </summary>
		public override string Name => _signal.Name;


		public override Guid Uid => throw new NotImplementedException();

		public override Guid ConfigGuid => Guid.Empty;

		public override ILazyTrendData TrendData => _signal;

		public override TrendChartType TrendChartType { get; set; }

		public override string Title => _signal.OwnerFault.Name + ", " + _signal.Name;

		public override bool IsEnabled {
			get => true;
			set {; }
		}

		#endregion // Реализация TrendViewModelBase
	}
}
