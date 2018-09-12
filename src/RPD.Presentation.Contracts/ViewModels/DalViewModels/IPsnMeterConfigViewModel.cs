using System.Collections.ObjectModel;

namespace RPD.Presentation.Contracts.ViewModels.DalViewModels
{
    public interface IPsnMeterConfigViewModel
    {
        string Name { get; }

        ObservableCollection<IPsnChannelConfigViewModel> PsnChannelConfigs { get; }
    }
}