using System;
using RPD.DAL;
using RPD.Presentation.Contracts.ViewModels;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;

namespace RPD.Presentation.ViewModels.DalViewModels.DesignTime
{
    class RpdChannelDesignTime : TrendViewModelBase<RpdChannelViewModel>, IRpdChannelViewModel
    {
        private readonly string _name;

	    public RpdChannelDesignTime(string name)
        {
            _name = name;
            IsEnabled = true;
        }

        #region Overrides of TrendViewModelBase<RpdChannelViewModel>



        public override string Title { get; }


	    public override Guid Uid => throw new NotImplementedException();

	    public override Guid ConfigGuid => Guid.Empty;

	    public override bool IsEnabled { get; set; }

        public override ILazyTrendData TrendData => null;

	    public override TrendChartType TrendChartType { get; set; }

        public override string Name => _name;

	    #endregion

        #region Implementation of IRpdChannelViewModel

        public IRpdMeterViewModel Meter { get; set; }

        #endregion
    }
}
