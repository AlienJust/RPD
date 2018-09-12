using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RPD.Configurator.Messages;

namespace RPD.Configurator.ViewModels
{
    /// <summary>
    /// Модель представления окна редактирования канала измеринтеля.
    /// </summary>
    public class EditRpdChannelViewModel : ViewModelBase
    {        
        IRpdConfigurator rpdConfigurator;

        public EditRpdChannelViewModel(RpdChannelViewModel rpdChannel, IRpdConfigurator rpdConfigurator)
        {
            this.rpdConfigurator = rpdConfigurator;
            this.RpdChannel = rpdChannel;

            Name = rpdChannel.Name;
            DumpCondition = new DumpConditionViewModel(rpdChannel.DumpCondition);

            initializeCommands();

            if (DumpCondition.ConnectionPointIndex > 0 && DumpCondition.ConnectionPointIndex <= 48)
                IsPsnPoint = true;
        }

        private void initializeCommands()
        {
            Ok = new RelayCommand(OkExecute);
            Cancel = new RelayCommand(CancelExecute);
            ShowConnectionPoint = new RelayCommand(ShowConnectionPointExecute);
        }       


        #region Presentation Members


        public const string IsPsnPointPropertyName = "IsPsnPoint";
        private bool _isPsnPoint = false;


        /// <summary>
        /// Признак того, что канал является точкой из стандартной таблицы каналов системы диагностики ПСН.
        /// Что в свою очередь означает, что (DumpCondition.ConnectionPointIndex > 0 и DumpCondition.ConnectionPointIndex &lt& = 48).
        /// </summary>
        public bool IsPsnPoint
        {
            get
            {
                return _isPsnPoint;
            }

            set
            {
                if (_isPsnPoint == value)
                {
                    return;
                }

                _isPsnPoint = value;
                RaisePropertyChanged(IsPsnPointPropertyName);
            }
        }

        public const string IsFreePointPropertyName = "IsFreePoint";

        /// <summary>
        /// Произвольная точка.
        /// </summary>
        public bool IsFreePoint
        {
            get
            {
                return !IsPsnPoint;
            }

            set
            {
                IsPsnPoint = !value;
                RaisePropertyChanged(IsFreePointPropertyName);
            }
        }


        public RpdChannelViewModel RpdChannel { get; set; }

        public DumpConditionViewModel DumpCondition { get; set; }

        public string WindowTitle
        {
            get { return "Точка подключения Канала №" + RpdChannel.Number.ToString() + ", " + 
                RpdChannel.RpdMeter.Name; }
        }


        public const string NamePropertyName = "Name";
        private string _name;

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                if (_name == value)
                {
                    return;
                }

                _name = value;
                RaisePropertyChanged(NamePropertyName);
            }
        }

        #endregion // Presentation Members


        #region Commands

        public RelayCommand Ok { get; set; }

        void OkExecute()
        {
            if (IsFreePoint)
                RpdChannel.DumpCondition.ConnectionPointIndex = 0;
            else
                RpdChannel.DumpCondition.ConnectionPointIndex = DumpCondition.ConnectionPointIndex;

            RpdChannel.DumpCondition.ControlValue = DumpCondition.ControlValue;
            RpdChannel.DumpCondition.HighLimit = DumpCondition.HighLimit;
            RpdChannel.DumpCondition.IsUsed = DumpCondition.IsUsed;
            RpdChannel.DumpCondition.LowLimit = DumpCondition.LowLimit;
            RpdChannel.DumpCondition.UseControlValue = DumpCondition.UseControlValue;
            RpdChannel.DumpCondition.UseHighLimit = DumpCondition.UseHighLimit;
            RpdChannel.DumpCondition.UseLowLimit = DumpCondition.UseLowLimit;
            RpdChannel.DumpCondition.UseValueAbs = DumpCondition.UseValueAbs;

            RpdChannel.DumpCondition.CopyTo(RpdChannel.RpdChannel.DumpCondition);

            RpdChannel.Name = Name;

            Messenger.Default.Send<AppMessages>(new AppMessages(), AppMessages.CloseEditRpdChannelWindow);
        }


        public RelayCommand Cancel { get; set; }

        void CancelExecute()
        {
            Messenger.Default.Send<AppMessages>(new AppMessages(), AppMessages.CloseEditRpdChannelWindow);
        }


        public RelayCommand ShowConnectionPoint { get; set; }

        void ShowConnectionPointExecute()
        {
            RpdChannel.ConnectionPoints.RpdChannel = this;
            rpdConfigurator.ShowConnectionPointDialog(this);
        }

        #endregion // Commands

    }
}