using System.Collections.ObjectModel;
using System.Windows.Media;
using GalaSoft.MvvmLight.Command;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;

namespace RPD.Presentation.Contracts.ViewModels
{
    public interface IDeafultColorsViewModel
    {
        ObservableCollection<IPsnConfigurationViewModel> Configurations { get; }

        IPsnConfigurationViewModel SelectedConfiguration { get; set; }
        IPsnMeterConfigViewModel SelectedPsnMeterConfig { get; set; }
        IPsnChannelConfigViewModel SelectedChannelConfig { get; set; }

        Color SelectedColor { get; set; }

        ObservableCollection<SolidColorBrush> ColorBrushes { get;  }

        RelayCommand Close { get; set; }
        RelayCommand ShowSelectColorDialog { get; set; }
        RelayCommand AddSelectedColorToPalette { get; set; }
    }
}