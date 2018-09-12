using System.Collections.ObjectModel;
using System.Windows.Media;
using GalaSoft.MvvmLight.Command;
using RPD.Presentation.Contracts.ViewModels;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;
using RPD.Presentation.ViewModels.DalViewModels.DesignTime;
using PsnConfigurationDesignTime = RPD.Presentation.ViewModels.DalViewModels.DesignTime.PsnConfigurationDesignTime;

namespace RPD.Presentation.ViewModels
{
    internal class DeafultColorsDesignTime : IDeafultColorsViewModel
    {
        public DeafultColorsDesignTime()
        {
            ColorBrushes = new ObservableCollection<SolidColorBrush>();
            ColorBrushes.Add(new SolidColorBrush(Colors.Green));
            ColorBrushes.Add(new SolidColorBrush(Colors.White));
            ColorBrushes.Add(new SolidColorBrush(Colors.Red));
            ColorBrushes.Add(new SolidColorBrush(Colors.Purple));
            ColorBrushes.Add(new SolidColorBrush(Colors.Peru));
            ColorBrushes.Add(new SolidColorBrush(Colors.SaddleBrown));
            ColorBrushes.Add(new SolidColorBrush(Colors.RoyalBlue));
            ColorBrushes.Add(new SolidColorBrush(Colors.Blue));
            ColorBrushes.Add(new SolidColorBrush(Colors.CadetBlue));
            ColorBrushes.Add(new SolidColorBrush(Colors.Cyan));
            ColorBrushes.Add(new SolidColorBrush(Colors.DarkGray));
            ColorBrushes.Add(new SolidColorBrush(Colors.DarkOrange));
            ColorBrushes.Add(new SolidColorBrush(Colors.Gold));
            ColorBrushes.Add(new SolidColorBrush(Colors.Magenta));
            ColorBrushes.Add(new SolidColorBrush(Colors.Olive));
            ColorBrushes.Add(new SolidColorBrush(Colors.SlateGray));

            Configurations = new ObservableCollection<IPsnConfigurationViewModel>
            {
                new PsnConfigurationDesignTime() {Name = "Трамвай 1"},
                new PsnConfigurationDesignTime() {Name = "Трамвай 2"},
                new PsnConfigurationDesignTime() {Name = "Трамвай 3"},
                new PsnConfigurationDesignTime() {Name = "Трамвай 4"}
            };
            
            SelectedConfiguration = Configurations[0];
            SelectedConfiguration.PsnMeterConfigs.Add(new PsnMeterConfigDesignTime { Name = "Измеритель 1"});
            SelectedConfiguration.PsnMeterConfigs.Add(new PsnMeterConfigDesignTime { Name = "Измеритель 2" });
            SelectedConfiguration.PsnMeterConfigs.Add(new PsnMeterConfigDesignTime { Name = "Измеритель 3" });
            SelectedConfiguration.PsnMeterConfigs.Add(new PsnMeterConfigDesignTime { Name = "Измеритель 4" });

            SelectedPsnMeterConfig = SelectedConfiguration.PsnMeterConfigs[0];
            SelectedPsnMeterConfig.PsnChannelConfigs.Add(new PsnChannelConfigDesignTime("Канал 1") { Color = Colors.Red });
            SelectedPsnMeterConfig.PsnChannelConfigs.Add(new PsnChannelConfigDesignTime("Канал 2") { Color = Colors.Purple });
            SelectedPsnMeterConfig.PsnChannelConfigs.Add(new PsnChannelConfigDesignTime("Канал 2") { Color = Colors.Peru });
            SelectedPsnMeterConfig.PsnChannelConfigs.Add(new PsnChannelConfigDesignTime("Канал 2") { Color = Colors.Plum });
            SelectedPsnMeterConfig.PsnChannelConfigs.Add(new PsnChannelConfigDesignTime("Канал 2") { Color = Colors.RoyalBlue });

            SelectedChannelConfig = SelectedPsnMeterConfig.PsnChannelConfigs[0];

            AddSelectedColorToPalette = new RelayCommand(() => { });
            Close = new RelayCommand(() => { });
            ShowSelectColorDialog = new RelayCommand(() => { });
        }

        public ObservableCollection<IPsnConfigurationViewModel> Configurations { get; }
        public IPsnConfigurationViewModel SelectedConfiguration { get; set; }
        public IPsnMeterConfigViewModel SelectedPsnMeterConfig { get; set; }
        public IPsnChannelConfigViewModel SelectedChannelConfig { get; set; }
        public Color SelectedColor { get; set; }
        public ObservableCollection<SolidColorBrush> ColorBrushes { get; set; }
        public RelayCommand Close { get; set; }
        public RelayCommand ShowSelectColorDialog { get; set; }
        public RelayCommand AddSelectedColorToPalette { get; set; }
    }
}