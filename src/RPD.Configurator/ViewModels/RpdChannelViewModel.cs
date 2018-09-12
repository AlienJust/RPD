using GalaSoft.MvvmLight;
using RPD.DAL;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RPD.Configurator.Messages;
using RPD.Configurator.Classes;

namespace RPD.Configurator.ViewModels
{
    /// <summary>
    /// Модель представления канала.
    /// </summary>
    public class RpdChannelViewModel : ViewModelBase
    {        
        IRpdConfigurator rpdConfigurator;
        RpdMeterViewModel rpdMeter;

        /// <summary>
        /// Модель представления канала.
        /// </summary>
        public RpdChannelViewModel(IRpdChannel channel, RpdMeterViewModel rpdMeter, 
            IRpdConfigurator rpdConfigurator, ConnectionPointsViewModel connectionPointsViewModel)
        {
            this.RpdChannel = channel;
            this.rpdConfigurator = rpdConfigurator;
            this.rpdMeter = rpdMeter;
            this.ConnectionPoints = connectionPointsViewModel;
            this.DumpCondition = new DumpConditionViewModel(channel.DumpCondition);

            initializeCommands();
        }

        private void initializeCommands()
        {
            ShowEditChannel = new RelayCommand<object>(ShowEditChannelExecute);
        }

        public RpdMeterViewModel RpdMeter { get { return rpdMeter; } }


        public IRpdChannel RpdChannel { get; set; }


        #region Presentation members


        public ConnectionPointsViewModel ConnectionPoints { get; set; }

        /// <summary>
        /// IRpdChannel.Number
        /// </summary>
        public int Number
        {
            get
            {
                return RpdChannel.Number;
            }
        }


        public const string NamePropertyName = "Name";

        /// <summary>
        /// IRpdChannel.Name
        /// </summary>
        public string Name
        {
            get
            {
                return RpdChannel.Name;
            }

            set
            {
                if (RpdChannel.Name == value)
                {
                    return;
                }

                RpdChannel.Name = value;

                RaisePropertyChanged(NamePropertyName);
            }
        }


        public const string IsEnabledPropertyName = "IsEnabled";

        /// <summary>
        /// IRpdChannel.IsEnabled
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return RpdChannel.IsEnabled;
            }

            set
            {
                if (RpdChannel.IsEnabled == value)
                {
                    return;
                }

                RpdChannel.IsEnabled = value;
                RaisePropertyChanged(IsEnabledPropertyName);
            }
        }


        /// <summary>
        /// IRpdChannel.IsService
        /// </summary>
        public bool IsService
        {
            get { return RpdChannel.IsService; }
        }


        /// <summary>
        /// Можно ли редактировать параметры канала.
        /// </summary>
        public bool IsEditable
        {
            get { return !RpdChannel.IsService; }
        }


        public const string DumpConditionPropertyName = "DumpCondition";

        /// <summary>
        /// Модель представления условия регистрации аварии.
        /// </summary>
        public DumpConditionViewModel DumpCondition { get; set; }


        #endregion // Presentation members


        #region Commands

        /// <summary>
        /// Показать окно редактирования канала.
        /// </summary>
        public RelayCommand<object> ShowEditChannel { get; set; }

        void ShowEditChannelExecute(object prm)
        {
            rpdConfigurator.ShowEditChannelWindow(this);
        }

        #endregion // Commands
    }
}