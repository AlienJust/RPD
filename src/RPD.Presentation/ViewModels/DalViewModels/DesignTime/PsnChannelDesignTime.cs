using System;
using RPD.DAL;
using RPD.Presentation.Contracts.ViewModels;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;

namespace RPD.Presentation.ViewModels.DalViewModels.DesignTime {
	class PsnChannelDesignTime : TrendViewModelBase<PsnChannelViewModel>, IPsnChannelViewModel {
		public PsnChannelDesignTime(string name = "Двигатель выхлоп ПП ТП ХЗ 1") {
			Name = name;
		}

		#region Overrides of TrendViewModelBase<PsnChannelViewModel>

		private const string _title = "Двигатель выхлоп ПП ТП ХЗ 1";

		public override string Title => _title;


		public override Guid Uid => Guid.Empty;

		public override Guid ConfigGuid => Guid.Empty;

		public override bool IsEnabled { get; set; }


		public override ILazyTrendData TrendData { get; }

		public override TrendChartType TrendChartType { get; set; }

		public override string Name { get; }

		#endregion
	}
}
