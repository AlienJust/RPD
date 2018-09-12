using Dnv.Utils.Collections;
using Dnv.Utils.Messages;
using System.Collections.ObjectModel;
using System.IO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RPD.DAL;
using RPD.Configurator.Classes;
using System.Windows.Input;
using System.Text;
using RPD.Configurator.Messages;
using System.Windows.Forms;
using RPD.Presentation.Utils.Classes;
using System.Windows.Data;
using System.ComponentModel;
using System.Windows;

namespace RPD.Configurator.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        ObservableCollectionsLinker<IRpdMeter, RpdMeterViewModel> _rpdMetersLinker;

        IDeviceConfiguration deviceConfiguration;
        IRpdConfigurator rpdConfigurator;

        public MainViewModel(IDeviceConfiguration deviceConfiguration, IRpdConfigurator rpdConfigurator)
        {
            this.deviceConfiguration = deviceConfiguration;
            this.rpdConfigurator = rpdConfigurator;

            ProgressVisible = false;

            _connectionPointsViewModel = new ConnectionPointsViewModel(new ConnectionPointsDescription());

            initializeCommands();

            reinitializeDeviceConfigurationViewModel();
        }

        /// <summary>
        /// Реинициализирует всю модель. Можно использовать для полного 
        /// пересброса всех параметров (например после загрузки модели из файла).
        /// </summary>
        private void reinitializeDeviceConfigurationViewModel()
        {
            fillRpdMeters(deviceConfiguration, rpdConfigurator);

            linkRpdMetersModelAndViewModel(deviceConfiguration, rpdConfigurator);

            fillPsnMeters(deviceConfiguration);

            fillPsnFaultSigns();

            updateDeviceConfigPropertyBindings();
        }

        private void updateDeviceConfigPropertyBindings()
        {
            RaisePropertyChanged(AddressPropertyName);
            RaisePropertyChanged(CommentPropertyName);
            RaisePropertyChanged(LocomotiveNamePropertyName);
            RaisePropertyChanged(LogPsnPropertyName);
            RaisePropertyChanged(NetAddressPropertyName);
            RaisePropertyChanged(ResetFaultsDumpPropertyName);
            RaisePropertyChanged(SaveByteIntervalPropertyName);
            RaisePropertyChanged(SectionNumberPropertyName);
            RaisePropertyChanged(UseHammingForNandPropertyName);
            RaisePropertyChanged(FirmwareVersionPropertyName);
            RaisePropertyChanged(MemoryStatusPropertyName);
            RaisePropertyChanged(FaultsLogCountPropertyName);
            RaisePropertyChanged(BadBlocksPropertyName);
            //RaisePropertyChanged(PsnVersionPropertyName);
        }

        private void fillRpdMeters(IDeviceConfiguration deviceConfiguration, IRpdConfigurator rpdConfigurator)
        {
            RpdMeters = new ObservableCollection<RpdMeterViewModel>();

            foreach (var meter in deviceConfiguration.RpdMeters)
                RpdMeters.Add(new RpdMeterViewModel(meter, rpdConfigurator, ConnectionPoints));
        }

        private void linkRpdMetersModelAndViewModel(IDeviceConfiguration deviceConfiguration, IRpdConfigurator rpdConfigurator)
        {
            // Связываем коллекцию моделей представления измерителей с коллекцией измерителей из модели данных.
            _rpdMetersLinker = new ObservableCollectionsLinker<IRpdMeter, RpdMeterViewModel>
                (deviceConfiguration.RpdMeters, RpdMeters,
                (meter) =>
                {
                    return new RpdMeterViewModel(meter, rpdConfigurator, ConnectionPoints);
                });
        }

        private void fillPsnFaultSigns()
        {
            PsnFaultSigns = new ObservableCollection<PsnChannelViewModel>();

            selectPsnMetersThatCanBeFaultSigns();

            groupPsnFaultSignsByMeterName();
        }

        private void selectPsnMetersThatCanBeFaultSigns()
        {
            foreach (var psnMeter in PsnMeters)
                foreach (var psnChannel in psnMeter.Channels)
                    if (psnChannel.CanBeFaultSign)
                        PsnFaultSigns.Add(psnChannel);
        }

        private void groupPsnFaultSignsByMeterName()
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(PsnFaultSigns);
            view.GroupDescriptions.Add(new PropertyGroupDescription("MeterName"));
        }

        private void fillPsnMeters(IDeviceConfiguration deviceConfiguration)
        {
            PsnMeters = new ObservableCollection<PsnMeterViewModel>();

            
            /* TODO: разобраться куда делись PsnMeters
            foreach (var psnMeter in deviceConfiguration.PsnMeters)
                PsnMeters.Add(new PsnMeterViewModel(psnMeter));
            */
        }

        void initializeCommands()
        {
            AddMeter = new RelayCommand(AddMeterExecute);

            DeleteMeter = new RelayCommand(DeleteMeterExecute,
                () =>
                {
                    return CurrentMeter != null;
                });

            SaveToFile = new RelayCommand(SaveToFileExecute, () => { return !ProgressVisible; });

            LoadFromFile = new RelayCommand(LoadFromFileExecute, () => { return !ProgressVisible; });

            SaveToDevice = new RelayCommand(SaveToDeviceExecute, () => { return !ProgressVisible; });

            LoadFromDevice = new RelayCommand(LoadFromDeviceExecute, () => { return !ProgressVisible; });

            ClearAllArchives = new RelayCommand(ClearAllArchivesExecute, () => { return !ProgressVisible; });

            MeterConfigExt = new RelayCommand(MeterConfigExtExecute, () => { return false; });

            ShowEditRpdChannel = new RelayCommand(ShowEditRpdChannelExecute);

            WriteFirmware = new RelayCommand(WriteFirmwareExecute, () => { return !ProgressVisible; });
        }


        #region Presentation Members


        public const string CurrentMeterPropertyName = "CurrentMeter";
        private RpdMeterViewModel _currentMeter = null;

        public RpdMeterViewModel CurrentMeter
        {
            get
            {
                return _currentMeter;
            }

            set
            {
                if (_currentMeter == value)
                {
                    return;
                }

                _currentMeter = value;
                RaisePropertyChanged(CurrentMeterPropertyName);

                // Обновляем биндинги команд во View.
                CommandManager.InvalidateRequerySuggested();
            }
        }


        public const string CurrentRpdChannelPropertyName = "CurrentRpdChannel";
        private RpdChannelViewModel _currentRpdChannel = null;

        /// <summary>
        /// Выбранный канал в списке каналов CurrentMeter.
        /// </summary>
        public RpdChannelViewModel CurrentRpdChannel
        {
            get
            {
                return _currentRpdChannel;
            }

            set
            {
                if (_currentRpdChannel == value)
                {
                    return;
                }

                _currentRpdChannel = value;
                RaisePropertyChanged(CurrentRpdChannelPropertyName);
            }
        }


        public const string ProgressVisiblePropertyName = "ProgressVisible";
        private bool _progressVisible = true;

        /// <summary>
        /// Признак того что конфигурация находится в процессе загрузки.
        /// </summary>
        public bool ProgressVisible
        {
            get
            {
                return _progressVisible;
            }

            set
            {
                if (_progressVisible == value)
                {
                    return;
                }

                _progressVisible = value;
                RaisePropertyChanged(ProgressVisiblePropertyName);
                CommandManager.InvalidateRequerySuggested();
            }
        }


        public const string ProgressStringPropertyName = "ProgressString";
        private string _progressString = "";

        /// <summary>
        /// Строка прогресса. Отображается в случае долгой операции (загрузка, чтение и т.п.).
        /// </summary>
        public string ProgressString
        {
            get
            {
                return _progressString;
            }

            set
            {
                if (_progressString == value)
                {
                    return;
                }

                _progressString = value;
                RaisePropertyChanged(ProgressStringPropertyName);
            }
        }


        public const string SelectedDrivePropertyName = "SelectedDrive";
        private DriveInfo _selectedDrive = null;

        public DriveInfo SelectedDrive
        {
            get
            {
                return _selectedDrive;
            }

            set
            {
                if (_selectedDrive == value)
                {
                    return;
                }

                _selectedDrive = value;
                RaisePropertyChanged(SelectedDrivePropertyName);
            }
        }


        private UsbRemovableDrives _removableDrives = new UsbRemovableDrives();

        /// <summary>
        /// Список подключаемых накопителей (флешек).
        /// </summary>
        public UsbRemovableDrives RemovableDrives
        {
            get
            {
                return _removableDrives;
            }
        }


        public const string RpdMetersPropertyName = "RpdMeters";
        private ObservableCollection<RpdMeterViewModel> _rpdMeters = null;

        /// <summary>
        /// IDeviceConfiguration.Meters view model.
        /// </summary>
        public ObservableCollection<RpdMeterViewModel> RpdMeters
        {
            get
            {
                return _rpdMeters;
            }

            set
            {
                if (_rpdMeters == value)
                    return;

                _rpdMeters = value;
                RaisePropertyChanged(RpdMetersPropertyName);
            }
        }


        public const string PsnMetersPropertyName = "PsnMeters";
        private ObservableCollection<PsnMeterViewModel> _psnMeters = null;

        /// <summary>
        /// IDeviceConfiguration.PsnMeters view model.
        /// </summary>
        public ObservableCollection<PsnMeterViewModel> PsnMeters
        {
            get
            {
                return _psnMeters;
            }

            set
            {
                if (_psnMeters == value)
                {
                    return;
                }
                _psnMeters = value;
                RaisePropertyChanged(PsnMetersPropertyName);
            }
        }


        public const string PsnFaultSignsPropertyName = "PsnFaultSigns";
        private ObservableCollection<PsnChannelViewModel> _psnFaultSigns = null;

        /// <summary>
        /// Список каналов ПСН со всех измерителей ПСН, которые могут быть признаками аварий.
        /// </summary>
        public ObservableCollection<PsnChannelViewModel> PsnFaultSigns
        {
            get { return _psnFaultSigns; }

            set
            {
                if (_psnFaultSigns == value) 
                    return;

                _psnFaultSigns = value;
                RaisePropertyChanged(PsnFaultSignsPropertyName);
            }
        }


        public const string AddressPropertyName = "Address";

        /// <summary>
        /// IDeviceConfiguration.Address.
        /// </summary>
        public int Address
        {
            get
            {
                return deviceConfiguration.Address;
            }

            set
            {
                if (deviceConfiguration.Address == value)
                {
                    return;
                }

                deviceConfiguration.Address = value;
                RaisePropertyChanged(AddressPropertyName);
            }
        }


        public const string CommentPropertyName = "Comment";

        /// <summary>
        /// IDeviceConfiguration.Comment.
        /// </summary>
        public string Comment
        {
            get
            {
                return deviceConfiguration.Comment;
            }

            set
            {
                if (deviceConfiguration.Comment == value)
                {
                    return;
                }

                deviceConfiguration.Comment = value;
                RaisePropertyChanged(CommentPropertyName);
            }
        }


        public const string LocomotiveNamePropertyName = "LocomotiveName";

        /// <summary>
        /// IDeviceConfiguration.LocomotiveName.
        /// </summary>
        public string LocomotiveName
        {
            get
            {
                return deviceConfiguration.LocomotiveName;
            }

            set
            {
                if (deviceConfiguration.LocomotiveName == value)
                {
                    return;
                }

                deviceConfiguration.LocomotiveName = value;
                RaisePropertyChanged(LocomotiveNamePropertyName);
            }
        }


        public const string LogPsnPropertyName = "LogPsn";

        /// <summary>
        /// IDeviceConfiguration.LogPsn.
        /// </summary>
        public bool LogPsn
        {
            get
            {
                return deviceConfiguration.LogPsn;
            }

            set
            {
                if (deviceConfiguration.LogPsn == value)
                {
                    return;
                }

                deviceConfiguration.LogPsn = value;
                RaisePropertyChanged(LogPsnPropertyName);
            }
        }


        public const string NetAddressPropertyName = "NetAddress";

        /// <summary>
        /// IDeviceConfiguration.NetAddress.
        /// </summary>
        public int NetAddress
        {
            get  { return deviceConfiguration.NetAddress; }

            set
            {
                if (deviceConfiguration.NetAddress == value)
                    return;

                deviceConfiguration.NetAddress = value;
                RaisePropertyChanged(NetAddressPropertyName);
            }
        }


        public const string ResetFaultsDumpPropertyName = "ResetFaultsDump";

        /// <summary>
        /// IDeviceConfiguration.ResetFaultsDump.
        /// </summary>
        public bool ResetFaultsDump
        {
            get { return deviceConfiguration.ResetFaultsDump; }

            set
            {
                if (deviceConfiguration.ResetFaultsDump == value)
                    return;

                deviceConfiguration.ResetFaultsDump = value;
                RaisePropertyChanged(ResetFaultsDumpPropertyName);
            }
        }


        public const string SaveByteIntervalPropertyName = "SaveByteInterval";

        /// <summary>
        /// IDeviceConfiguration.SaveByteIntervalPropertyName.
        /// </summary>
        public bool SaveByteInterval
        {
            get {return deviceConfiguration.SaveByteInterval;}

            set
            {
                if (deviceConfiguration.SaveByteInterval == value)
                    return;

                deviceConfiguration.SaveByteInterval = value;
                RaisePropertyChanged(SaveByteIntervalPropertyName);
            }
        }


        public const string SectionNumberPropertyName = "SectionNumber";

        /// <summary>
        /// IDeviceConfiguration.SectionNumber.
        /// </summary>
        public int SectionNumber
        {
            get { return deviceConfiguration.SectionNumber - 1; }

            set
            {
                if (deviceConfiguration.SectionNumber == value)
                    return;

                deviceConfiguration.SectionNumber = value + 1;
                RaisePropertyChanged(SectionNumberPropertyName);
            }
        }


        public const string UseHammingForNandPropertyName = "UseHammingForNand";

        /// <summary>
        /// IDeviceConfiguration.UseHammingForNand.
        /// </summary>
        public bool UseHammingForNand
        {
            get
            {
                return deviceConfiguration.UseHammingForNand;
            }

            set
            {
                if (deviceConfiguration.UseHammingForNand == value)
                {
                    return;
                }

                deviceConfiguration.UseHammingForNand = value;
                RaisePropertyChanged(UseHammingForNandPropertyName);
            }
        }


        public const string FirmwareVersionPropertyName = "FirmwareVersion";
        /// <summary>
        /// Версия прошивки.
        /// </summary>
        public string FirmwareVersion
        {
            get
            {
                if (deviceConfiguration.Diagnostic != null)
                    return deviceConfiguration.Diagnostic.FirmwareVersion.ToString();
                return "";
            }
        }


        public const string MemoryStatusPropertyName = "MemoryStatus";
        
        /// <summary>
        /// Статус работы с памятью.
        /// </summary>
        public string MemoryStatus
        {
            get
            {
                string status = "";
                if (deviceConfiguration.Diagnostic.ErrorDumpTableCRC)
                    status += "ошибка CRC16 таблицы дампов аварий; ";
                if (deviceConfiguration.Diagnostic.ErrorRpdMetersTableCRC)
                    status += "ошибка КС в таблице измерителей; ";
                if (deviceConfiguration.Diagnostic.LostFRAM)
                    status += "нет связи с  FRAM; ";
                if (deviceConfiguration.Diagnostic.LostFRAM)
                    status += "нет связи с NAND; ";

                return status;
            }
        }


        public const string FaultsLogCountPropertyName = "FaultsLogCount";

        /// <summary>
        /// Количество дампов аварий.
        /// </summary>
        public string FaultsLogCount
        {
            get { return deviceConfiguration.Diagnostic.FaultLogsCount.ToString(); }
        }


        private string badBlocks = "";
        private bool badBlocksGenerated = false;

        public const string BadBlocksPropertyName = "BadBlocks";

        /// <summary>
        /// Сбойные блоки памяти.
        /// </summary>
        public string BadBlocks
        {
            get
            {
                if (deviceConfiguration.Diagnostic.BadBlocks.Count == 0)
                    return "отсутствуют";
                else
                {
                    if (!badBlocksGenerated)
                        return generateBadBlocksString();
                    else
                        return badBlocks;
                }

            }
        }

        /// <summary>
        /// Генерирунт сроку с перечислением всх сбойных блоков.
        /// </summary>
        private string generateBadBlocksString()
        {
            var str = new StringBuilder();

            for (int i = 0; i < deviceConfiguration.Diagnostic.BadBlocks.Count; i++)
            {
                if (i > 0)
                    str.Append(", ");

                str.Append(deviceConfiguration.Diagnostic.BadBlocks[i].ToString());
            }

            badBlocks = str.ToString();
            badBlocksGenerated = true;

            return badBlocks;
        }


        public const string StatusStringPropertyName = "StatusString";
        private string _statusString = "";

        /// <summary>
        /// Строка статуса.
        /// </summary>
        public string StatusString
        {
            get
            {
                return _statusString;
            }

            set
            {
                if (_statusString == value)
                {
                    return;
                }

                _statusString = value;
                RaisePropertyChanged(StatusStringPropertyName);
            }
        }


        public const string ConnectionPointsPropertyName = "ConnectionPoints";
        private ConnectionPointsViewModel _connectionPointsViewModel;

        /// <summary>
        /// Gets the ConnectionPoints property.
        /// </summary>
        public ConnectionPointsViewModel ConnectionPoints
        {
            get
            {
                return _connectionPointsViewModel;
            }

            set
            {
                if (_connectionPointsViewModel == value)
                {
                    return;
                }

                _connectionPointsViewModel = value;
                RaisePropertyChanged(ConnectionPointsPropertyName);
            }
        }


        /*
        public const string PsnVersionPropertyName = "PsnVersion";

        public PsnProtocolVersion PsnVersion
        {
            get { return deviceConfiguration.PsnVersion;}

            set
            {
                if (deviceConfiguration.PsnVersion == value)
                    return;

                deviceConfiguration.PsnVersion = value;

                fillPsnFaultSigns();

                RaisePropertyChanged(PsnVersionPropertyName);
            }
        }
        */


        #endregion // Presentation Members


        #region Commands

        public RelayCommand WriteFirmware { get; set; }

        void WriteFirmwareExecute()
        {
            if (!checkIfDriveSelected("Выберите устройство для записи ПО."))
                return;

            var msg = new DialogMessage<CommonDialog>(this, new OpenFileDialog(),
                (result) =>
                {
                    if (result.DialogResult == DialogResult.OK)
                    {
                        ProgressVisible = true;
                        StatusString = "Запись ПО РПД. Пожалуйста, подождите..";
                        deviceConfiguration.WriteFirmware((result.Dialog as OpenFileDialog).FileName, SelectedDrive.Name,
                            (complete) =>
                            {                                
                                ProgressVisible = false;
                                if (complete.ResultCode == EventArgs.OnCompleteEventArgs.CompleteResult.Error)
                                    StatusString = "При записи ПО РПД возникла ошибка.";                              
                                else
                                    StatusString = "Запись ПО РПД успешно.";   
                            });
                    }
                });

            Messenger.Default.Send<DialogMessage<CommonDialog>>(msg, AppMessages.ShowDialog);
        }


        public RelayCommand AddMeter { get; set; }

        void AddMeterExecute()
        {
            rpdConfigurator.ShowSelectMeterTypeDialog(onSelectMeterTypeComplete);
        }


        public RelayCommand DeleteMeter { get; set; }

        void DeleteMeterExecute()
        {
            if (CurrentMeter != null)
            {
                deviceConfiguration.RpdMeters.Remove(CurrentMeter.Meter);
                RpdMeters.Remove(CurrentMeter);
            }
        }


        public RelayCommand SaveToFile { get; set; }

        void SaveToFileExecute()
        {
            if (!canBeSaved())
                return;

            // отображаем диалог выбора папки.
            var msg = new DialogMessage<FolderBrowserDialog>(this, new FolderBrowserDialog(),
                (result) =>
                {
                    if (result.DialogResult == DialogResult.OK)
                    {
                        deviceConfiguration.Write(result.Dialog.SelectedPath,
                            (completeRes) =>
                            {
                                if (completeRes.ResultCode == EventArgs.OnCompleteEventArgs.CompleteResult.Ok)
                                    StatusString = "Конфигурация успешно сохранена в папку " + result.Dialog.SelectedPath;
                                else
                                    StatusString = completeRes.Message;
                            });
                    }
                });

            // посылаем сообщение в главную форму
            Messenger.Default.Send<DialogMessage<FolderBrowserDialog>>(msg, AppMessages.ShowSaveToFileDialog);
        }


        public RelayCommand LoadFromFile { get; set; }

        void LoadFromFileExecute()
        {
            if (!promtBeforeLoad())
                return;

            // отображаем диалог выбора папки.
            var msg = new DialogMessage<FolderBrowserDialog>(this, new FolderBrowserDialog(),
                (result) =>
                {
                    if (result.DialogResult == DialogResult.OK)
                    {
                        ProgressVisible = true;
                        StatusString = "Чтение конфигурации из файла. Пожалуйста, подождите...";
                        deviceConfiguration.Read(result.Dialog.SelectedPath,
                            (completeRes) =>
                            {
                                ProgressVisible = false;
                                if (completeRes.ResultCode == EventArgs.OnCompleteEventArgs.CompleteResult.Ok)
                                {
                                    StatusString = "Конфигурация успешно загружена.";
                                    reinitializeDeviceConfigurationViewModel();
                                }
                                else
                                    StatusString = completeRes.Message;
                            });
                    }
                });

            Messenger.Default.Send<DialogMessage<FolderBrowserDialog>>(msg, AppMessages.ShowLoadFromFileDialog);

        }


        public RelayCommand SaveToDevice { get; set; }

        void SaveToDeviceExecute()
        {
            if (!canBeSaved())
                return;

            if (!checkIfDriveSelected("Выберите устройство для записи конфигурации."))
                return;

            if (showContinueSaveToFileDialog() == MessageBoxResult.Yes)
            {
                ProgressVisible = true;
                ProgressString = "Запись конфигурации в устройство. Пожалуйста, подождите...";

                deviceConfiguration.Write(SelectedDrive.Name,
                    (result) =>
                    {
                        ProgressVisible = false;
                        if (result.ResultCode == EventArgs.OnCompleteEventArgs.CompleteResult.Ok)
                            StatusString = "Конфигурация успешно записана";
                        else
                            StatusString = result.Message;
                    });
            }

        }

        private bool canBeSaved()
        {
            if (RpdMeters.Count == 0)
                return true;

            // Проверяем, что все адреса идут последовательно и непрерывно.
            int prev = RpdMeters[0].Address;
            for (int i = 1; i < RpdMeters.Count; i++ )
            {
                if (prev != RpdMeters[i].Address - 1)
                {
                    rpdConfigurator.ShowMessageBox("Адреса измерителей должны идти последовательно и непрерывно в порядке возрастания. Измеритель \"" + 
                        RpdMeters[i].Name + "\"", 
                        "Ошибка", 
                        MessageBoxImage.Exclamation, 
                        MessageBoxButton.OK);

                    return false;
                }

                prev = RpdMeters[i].Address;
            }

            return true;
        }

        private MessageBoxResult showContinueSaveToFileDialog()
        {
            return rpdConfigurator.ShowMessageBox("Это действие приведёт к утере всей имеющейся информации на диске " + 
                SelectedDrive.Name + ". Если диск не является флеш накопителем РПД, то на диске будет утеряна файловая система. Продолжить?",
                "Внимание",
                MessageBoxImage.Exclamation,
                MessageBoxButton.YesNo);
        }


        private bool checkIfDriveSelected(string ifNotSelectedMessage)
        {
            if (SelectedDrive == null)
            {
                rpdConfigurator.ShowMessageBox("Выберите устройство для записи конфигурации.",
                    "",
                    MessageBoxImage.Exclamation,
                    MessageBoxButton.OK);

                return false;  
            }

            return true;
        }


        public RelayCommand LoadFromDevice { get; set; }

        void LoadFromDeviceExecute()
        {
            if (!promtBeforeLoad())
                return;

            if (!checkIfDriveSelected("Выберите устройство для считывания конфигурации."))
                return;

            ProgressVisible = true;
            ProgressString = "Загрузка конфигурации из устройства. Пожалуйста, подождите...";

            deviceConfiguration.Read(SelectedDrive.Name, (result) =>
            {
                ProgressVisible = false;

                reinitializeDeviceConfigurationViewModel();

                if (result.ResultCode == EventArgs.OnCompleteEventArgs.CompleteResult.Ok)
                    StatusString = "Конфигурация успешно загружена";
                else
                    StatusString = result.Message;
            });
        }

        private bool promtBeforeLoad()
        {
            return rpdConfigurator.ShowMessageBox("Все несохранённые данные будут утеряны. Продолжить считывание конфигурации?",
                                "Внимание", MessageBoxImage.Question, MessageBoxButton.YesNo) == MessageBoxResult.Yes;
        }

        public RelayCommand ShowEditRpdChannel { get; set; }

        void ShowEditRpdChannelExecute()
        {
            if (CurrentRpdChannel != null)
            {
                if (!CurrentRpdChannel.IsService)
                    CurrentRpdChannel.ShowEditChannel.Execute(this);
            }
        }


        public RelayCommand MeterConfigExt { get; set; }

        void MeterConfigExtExecute()
        {
            ;
        }

        public RelayCommand ClearAllArchives { get; set; }

        void ClearAllArchivesExecute()
        {
            if (!checkIfDriveSelected("Выберите устройство для стирания всех архивов."))
                return;

            ProgressVisible = true;
            deviceConfiguration.ClearArchivesAsync(SelectedDrive.Name,
                (result) =>                
                {
                    ProgressVisible = false;
                    if (result.ResultCode == EventArgs.OnCompleteEventArgs.CompleteResult.Ok)
                        StatusString = "Архивы успешно удалены";
                    else
                        StatusString = result.Message;
                });
        }

        #endregion // Commands


        /// <summary>
        /// Вызывается по резульатут отображения диалога выбора типа счетчика.
        /// </summary>
        private void onSelectMeterTypeComplete(SelectMeterTypeResult result)
        {
            if (result.Result)
            {
                // добавляем измеритель в список.
                IRpdMeter meter = deviceConfiguration.CreateMeter();
                meter.Type = result.MeterType;

                // при добавлении измерителя в этот список, будет автоматически 
                // создана модель представления этого измеритя блягодаря классу ObservaleCollectionsLinker    
                deviceConfiguration.RpdMeters.Add(meter);
            }
        }
    }
}