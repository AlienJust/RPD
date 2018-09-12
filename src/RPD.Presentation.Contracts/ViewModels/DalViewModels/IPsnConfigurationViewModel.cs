using System.Collections.ObjectModel;
using RPD.DAL;

namespace RPD.Presentation.Contracts.ViewModels.DalViewModels
{
    public interface IPsnConfigurationViewModel
    {
        IPsnConfiguration Model { get; }

        string Version { get; }
        string Name { get; }
        string Description { get; }

        string DescriptionAndVersionString { get; }

        ObservableCollection<IPsnMeterConfigViewModel> PsnMeterConfigs { get; }
    }
}