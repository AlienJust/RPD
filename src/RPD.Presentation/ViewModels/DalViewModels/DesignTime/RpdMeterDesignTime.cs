using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;

namespace RPD.Presentation.ViewModels.DalViewModels.DesignTime
{
    class RpdMeterDesignTime : ViewModelBase, IRpdMeterViewModel
    {
        public RpdMeterDesignTime(string name)
        {
            Name = name;
            Channels = new ObservableCollection<IRpdChannelViewModel>();
        }

        #region Implementation of IRpdMeterViewModel

        public IFaultViewModel Fault { get; set; }
        public string Name { get; }
        public ObservableCollection<IRpdChannelViewModel> Channels { get; }

        #endregion
    }
}
