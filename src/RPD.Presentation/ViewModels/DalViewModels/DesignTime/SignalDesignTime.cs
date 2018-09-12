using System;
using RPD.DAL;
using RPD.Presentation.Contracts.ViewModels;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;

namespace RPD.Presentation.ViewModels.DalViewModels.DesignTime {
	class SignalDesignTime : TrendViewModelBase<SignalViewModel>, ISignalViewModel {
		public SignalDesignTime(string name) {
			Name = name;
			IsEnabled = true;
		}

		#region Overrides of TrendViewModelBase<SignalViewModel>

		public override string Title { get; }


		public override Guid Uid => throw new NotImplementedException();

		public override Guid ConfigGuid => Guid.Empty;

		public override ILazyTrendData TrendData => null;

		public override TrendChartType TrendChartType { get; set; }


		public override bool IsEnabled { get; set; }

		public override string Name { get; }

		#endregion

		#region Implementation of ISignalViewModel

		public string MathOperation { get; set; }

		#endregion
	}
}
