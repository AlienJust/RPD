using RPD.DAL;
using RPD.Presentation.Utils;
using RPD.Configurator.Classes;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RPD.Configurator.Messages;

namespace RPD.Configurator.ViewModels
{
    public class ConnectionPointsViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the ConnectionPointsViewModel class.
        /// </summary>
        public ConnectionPointsViewModel(ConnectionPointsDescription connectionPointDescription)
        {
            ConnectionPointsDesc = connectionPointDescription;

            IsEditMode = false;

            initializeCommands();

            LoadExecute();
        }

        private void initializeCommands()
        {
            AddGroup = new RelayCommand(AddGroupExecute);
            DeleteGroup = new RelayCommand(DeleteGroupExecute);
            AddPoint = new RelayCommand(AddPointExecute);
            DeletePoint = new RelayCommand(DeletePointExecute);
            Save = new RelayCommand(SaveExecute);
            Load = new RelayCommand(LoadExecute);
            DupPoint = new RelayCommand(DupPointExecute);
            SetRpdChannel = new RelayCommand<object>(SetRpdChannelExecute);
            Apply = new RelayCommand(ApplyExecute);
            Cancel = new RelayCommand(CancelExecute);
        }

        #region Presentation Members

        private EditRpdChannelViewModel _rpdChannel;
        public EditRpdChannelViewModel RpdChannel
        {
            get { return _rpdChannel; }
            set
            {
                _rpdChannel = value;

                if (_rpdChannel.DumpCondition.ConnectionPointIndex != 0)                
                    setSelectedConnectionPoint();                 
            }
        }

        private void setSelectedConnectionPoint()
        {
            bool asigned = false;

            foreach (var group in ConnectionPointsDesc.Groups)
                foreach (var point in group.Points)
                {
                    if (point.Condition.ConnectionPointIndex == 
                                _rpdChannel.DumpCondition.ConnectionPointIndex)
                    {
                        SelectedConnectionPoint = point;
                        SelectedConnectionPointGroup = group;
                        asigned = true;
                    }
                }

            if (asigned == false)
            {
                SelectedConnectionPoint = null;
                SelectedConnectionPointGroup = null;
            }
        }

        public const string IsEditModePropertyName = "IsEditMode";
        private bool _isEditMode = false;


        public bool IsEditMode
        {
            get
            {
                return _isEditMode;
            }

            set
            {
                if (_isEditMode == value)
                {
                    return;
                }

                _isEditMode = value;
                RaisePropertyChanged(IsEditModePropertyName);
            }
        }

        public const string ConnectionPointsDescPropertyName = "ConnectionPointsDesc";
        private ConnectionPointsDescription _connectionPointsDesc = new ConnectionPointsDescription();

        /// <summary>
        /// Описание точек подключения.
        /// </summary>
        public ConnectionPointsDescription ConnectionPointsDesc
        {
            get
            {
                return _connectionPointsDesc;
            }

            set
            {
                if (_connectionPointsDesc == value)
                {
                    return;
                }

                _connectionPointsDesc = value;
                RaisePropertyChanged(ConnectionPointsDescPropertyName);
            }
        }


        public const string SelectedConnectionPointGroupPropertyName = "SelectedConnectionPointGroup";
        private ConnectionPointsGroup _selectedConnectionPointGroup = null;

        /// <summary>
        /// Выбранная группа в ConnectionPointsDesc.
        /// </summary>
        public ConnectionPointsGroup SelectedConnectionPointGroup
        {
            get
            {
                return _selectedConnectionPointGroup;
            }

            set
            {
                if (_selectedConnectionPointGroup == value)
                {
                    return;
                }

                _selectedConnectionPointGroup = value;
                RaisePropertyChanged(SelectedConnectionPointGroupPropertyName);
            }
        }


        public const string SelectedConnectionPointPropertyName = "SelectedConnectionPoint";
        private ConnectionPoint _selectedConnectionPoint = null;

        /*
        public const string SelectedConnectionPointIndexPropertyName = "SelectedConnectionPointIndex";
        private int _selectedConnectionPointIndex = 0;

        
        /// <summary>
        /// Gets the SelectedConnectionPointIndex property.
        /// </summary>
        public int SelectedConnectionPointIndex
        {
            get
            {
                return _selectedConnectionPointIndex;
            }

            set
            {
                if (_selectedConnectionPointIndex == value)
                {
                    return;
                }

                bool asigned = false;

                foreach (var group in ConnectionPointsDesc.Groups)
                    foreach (var point in group.Points)
                    {
                        if (point.Condition.ConnectionPointIndex == value)
                        {
                            SelectedConnectionPoint = point;
                            SelectedConnectionPointGroup = group;
                            asigned = true;
                        }
                    }

                if (asigned == false)
                {
                    SelectedConnectionPoint = null;
                    SelectedConnectionPointGroup = null;
                }

                _selectedConnectionPointIndex = value;
                RaisePropertyChanged(SelectedConnectionPointIndexPropertyName);
            }
        }

        */
        /// <summary>
        /// Выбранная точка подключения в группе SelectedConnectionPointGroup.
        /// </summary>
        public ConnectionPoint SelectedConnectionPoint
        {
            get
            {
                return _selectedConnectionPoint;
            }

            set
            {
                if (_selectedConnectionPoint == value)
                {
                    return;
                }

                _selectedConnectionPoint = value;

                /*
                if (_selectedConnectionPoint != null)
                    _selectedConnectionPointIndex = _selectedConnectionPoint.Condition.ConnectionPointIndex;
                */
                if (_selectedConnectionPoint != null)
                    DumpCondition = new DumpConditionViewModel(_selectedConnectionPoint.Condition);
                    //DumpCondition.DumpCondition = _selectedConnectionPoint.Condition;

                RaisePropertyChanged(SelectedConnectionPointPropertyName);
            }
        }


        public const string DumpConditionPropertyName = "DumpCondition";
        private DumpConditionViewModel _dumpCondition;

        /// <summary>
        /// Условие регистрации для this.SelectedConnectionPoint или для последней 
        /// выбранной точки, если SelectedConnectionPoint == null. Это сво-во не может быть null.
        /// </summary>
        public DumpConditionViewModel DumpCondition
        {
            get
            {
                return _dumpCondition;
            }

            set
            {
                if (_dumpCondition == value)
                {
                    return;
                }

                _dumpCondition = value;
                RaisePropertyChanged(DumpConditionPropertyName);
            }
        }

        #endregion // Presentation Members


        #region Commands

        public RelayCommand Apply { get; set; }

        void ApplyExecute()
        {
            RpdChannel.Name = SelectedConnectionPoint.Name;
            RpdChannel.DumpCondition.ConnectionPointIndex = SelectedConnectionPoint.Condition.ConnectionPointIndex;
            RpdChannel.DumpCondition.ControlValue = SelectedConnectionPoint.Condition.ControlValue;
            RpdChannel.DumpCondition.HighLimit = SelectedConnectionPoint.Condition.HighLimit;
            RpdChannel.DumpCondition.IsUsed = SelectedConnectionPoint.Condition.IsUsed;
            RpdChannel.DumpCondition.LowLimit = SelectedConnectionPoint.Condition.LowLimit;
            RpdChannel.DumpCondition.UseControlValue = SelectedConnectionPoint.Condition.UseControlValue;
            RpdChannel.DumpCondition.UseHighLimit = SelectedConnectionPoint.Condition.UseHighLimit;
            RpdChannel.DumpCondition.UseLowLimit = SelectedConnectionPoint.Condition.UseLowLimit;
            RpdChannel.DumpCondition.UseValueAbs = SelectedConnectionPoint.Condition.UseValueAbs;

            Messenger.Default.Send<AppMessages>(new AppMessages(), AppMessages.CloseDefaultConnectionPointsWindow);
        }

        public RelayCommand Cancel { get; set; }

        void CancelExecute()
        {
            Messenger.Default.Send<AppMessages>(new AppMessages(),AppMessages.CloseDefaultConnectionPointsWindow);
        }

        public RelayCommand AddGroup { get; set; }

        public void AddGroupExecute()
        {
            ConnectionPointsDesc.Groups.Add(new ConnectionPointsGroup("Измеритель"));
        }


        public RelayCommand DeleteGroup { get; set; }

        public void DeleteGroupExecute()
        {
            if (SelectedConnectionPointGroup != null)
            {
                ConnectionPointsDesc.Groups.Remove(SelectedConnectionPointGroup);
                SelectedConnectionPointGroup = null;
            }
        }


        public RelayCommand AddPoint { get; set; }

        public void AddPointExecute()
        {
            if (SelectedConnectionPointGroup != null)
            {
                var cp = new ConnectionPoint("Канал");
                cp.Condition.ConnectionPointIndex = generateNextConnectionPointIndex();
                SelectedConnectionPointGroup.Points.Add(cp);
            }
        }


        public RelayCommand DeletePoint { get; set; }

        public void DeletePointExecute()
        {
            if (SelectedConnectionPoint != null)
            {
                SelectedConnectionPointGroup.Points.Remove(SelectedConnectionPoint);
                SelectedConnectionPoint = null;
            }
        }

        public RelayCommand DupPoint { get; set; }

        public void DupPointExecute()
        {
            if (SelectedConnectionPoint != null)
            {
                var cp = new ConnectionPoint(SelectedConnectionPoint.Name);
                cp.Condition.CopyFrom(SelectedConnectionPoint.Condition);
                cp.Condition.ConnectionPointIndex = generateNextConnectionPointIndex();
                SelectedConnectionPointGroup.Points.Add(cp);
            }
        }


        public RelayCommand Save { get; set; }

        public void SaveExecute()
        {
            ConnectionPointsDesc.Save(System.IO.Path.GetDirectoryName(System.Diagnostics.Process.
                    GetCurrentProcess().MainModule.FileName) + "\\DefaultConnectionPoints.xml");
        }


        public RelayCommand Load { get; set; }

        public void LoadExecute()
        {
            ConnectionPointsDesc.Load(System.IO.Path.GetDirectoryName(System.Diagnostics.Process.
                    GetCurrentProcess().MainModule.FileName) + "\\DefaultConnectionPoints.xml");
        }


        public RelayCommand<object> SetRpdChannel { get; set; }

        public void SetRpdChannelExecute(object rpdChannelViewModel)
        {
//            RpdChannel = rpdChannelViewModel;
        }
        
        #endregion // Commands

        private int generateNextConnectionPointIndex()
        {
            return ConnectionPointsDesc.Groups[ConnectionPointsDesc.Groups.Count-1]
                                .Points[ConnectionPointsDesc.Groups[ConnectionPointsDesc.Groups.Count-1].Points.Count-1]
                                .Condition.ConnectionPointIndex + 1;
        }
    }
}