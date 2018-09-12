using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;

namespace RPD.Presentation.Contracts.ViewModels
{
    public interface IChangePsnConfigurationViewModel
    {
        ObservableCollection<IPsnConfigurationViewModel> AvailableConfigurations { get; }
        IPsnConfigurationViewModel SelectedConfiguration { get; set; }

        RelayCommand ApplyCommand { get; }
    }
}