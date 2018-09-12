using System.Collections.ObjectModel;

namespace RPD.Presentation.Contracts.ViewModels.DalViewModels
{
    public interface IRpdMeterViewModel
    {
        IFaultViewModel Fault { get; set; }
        string Name { get; }
        ObservableCollection<IRpdChannelViewModel> Channels { get; }
    }
}