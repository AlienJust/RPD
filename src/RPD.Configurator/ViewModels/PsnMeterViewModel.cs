using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using RPD.DAL;

namespace RPD.Configurator.ViewModels
{
    /// <summary>
    /// Модель представления измерителя магистрали ПСН.
    /// </summary>
    public class PsnMeterViewModel : ViewModelBase
    {
        IPsnMeter psnMeter;

        public PsnMeterViewModel(IPsnMeter psnMeter)
        {
            this.psnMeter = psnMeter;

            fillChannels(psnMeter);
        }

        private void fillChannels(IPsnMeter psnMeter)
        {
            foreach (var channel in psnMeter.Channels)
                Channels.Add(new PsnChannelViewModel(channel, this));
        }


        #region Presentation Members

        public const string ChannelsPropertyName = "Channels";
        private ObservableCollection<PsnChannelViewModel> _channels = new ObservableCollection<PsnChannelViewModel>();

        public ObservableCollection<PsnChannelViewModel> Channels
        {
            get
            {
                return _channels;
            }

            set
            {
                if (_channels == value)
                    return;

                _channels = value;
                RaisePropertyChanged(ChannelsPropertyName);
            }
        }


        public string Name
        {
            get { return psnMeter.Name; }
        }

        #endregion

    }
}