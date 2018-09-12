using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using RPD.DAL;

namespace RPD.Configurator.ViewModels
{
    /// <summary>
    /// Модель представления измерителя.
    /// </summary>
    public class RpdMeterViewModel : ViewModelBase
    {
        public IRpdMeter Meter { get; set; }

        IRpdConfigurator rpdConfigurator;
        ConnectionPointsViewModel connectionPointsViewModel;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="meter">Измеритель.</param>
        public RpdMeterViewModel(IRpdMeter meter, IRpdConfigurator rpdConfigurator, ConnectionPointsViewModel connectionPointsViewModel)
        {
            this.Meter = meter;
            this.rpdConfigurator = rpdConfigurator;
            this.connectionPointsViewModel = connectionPointsViewModel;

            fillChannels();
        }

        void fillChannels()
        {
            foreach (var chan in Meter.Channels)
                Channels.Add(new RpdChannelViewModel(chan, this, rpdConfigurator, connectionPointsViewModel));
        }


        #region Presentation Members


        public const string NamePropertyName = "Name";

        public string Name
        {
            get
            {
                return Meter.Name;
            }

            set
            {
                if (Meter.Name == value)
                {
                    return;
                }

                Meter.Name = value;
                RaisePropertyChanged(NamePropertyName);
            }
        }

        public const string AddressPropertyName = "Address";

        /// <summary>
        /// Адрес измерителя.
        /// </summary>
        public int Address
        {
            get
            {
                return Meter.Address;
            }

            set
            {
                if (Meter.Address == value)
                {
                    return;
                }

                Meter.Address = value;
                RaisePropertyChanged(AddressPropertyName);
            }
        }


        public const string ChannelsPropertyName = "Channels";
        private ObservableCollection<RpdChannelViewModel> _channels = new ObservableCollection<RpdChannelViewModel>();

        /// <summary>
        /// Каналы.
        /// </summary>
        public ObservableCollection<RpdChannelViewModel> Channels
        {
            get
            {
                return _channels;
            }

            set
            {
                if (_channels == value)
                {
                    return;
                }

                _channels = value;
                RaisePropertyChanged(ChannelsPropertyName);
            }
        }


        /// <summary>
        /// тип измерителя.
        /// </summary>
        public RpdMeterType MeterType
        {
            get
            {
                return Meter.Type;
            }
        }

        #endregion // Presentation Members
    }
}