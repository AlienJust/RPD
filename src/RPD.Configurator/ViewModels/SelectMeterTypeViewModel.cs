using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Command;
using RPD.DAL;
using RPD.Configurator.Classes;
using RPD.Configurator.Messages;

namespace RPD.Configurator.ViewModels
{
    /// <summary>
    /// Модель представления диалога выбора типа измерителя.
    /// </summary>
    public class SelectMeterTypeViewModel : ViewModelBase
    {
        Action<SelectMeterTypeResult> onComplete;

//        byte [] leak = new byte[50*1024*1024];

        public SelectMeterTypeViewModel(Action<SelectMeterTypeResult> onComplete)
        {
            this.onComplete = onComplete;

            initializeCommands();

            MeterTypes.Add("УРАН");
            MeterTypes.Add("ИРВИ");
        }

        private void initializeCommands()
        {
            Ok = new RelayCommand(okExecute);
        }


        #region Commands

        public RelayCommand Ok { get; set; }

        void okExecute()
        {
            if (onComplete == null)
                return;

            onComplete(new SelectMeterTypeResult()
            {
                Result = true,
                MeterType = (RpdMeterType)(SelectedIndex + 1)
            });
            
            Messenger.Default.Send<AppMessages>(new AppMessages(), AppMessages.CloseSelectMeterTypeWindow);
        }

        #endregion


        #region Presentation Members



        public const string SelectedIndexPropertyName = "SelectedIndex";
        private int _selectedIndex = 0;

        /// <summary>
        /// Gets the SelectedIndex property.
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }

            set
            {
                if (_selectedIndex == value)
                {
                    return;
                }
                _selectedIndex = value;
                RaisePropertyChanged(SelectedIndexPropertyName);
            }
        }


        private ObservableCollection<string> _meterTypes = new ObservableCollection<string>();

        /// <summary>
        /// Типы счетчиков.
        /// </summary>
        public ObservableCollection<string> MeterTypes
        {
            get
            {
                return _meterTypes;
            }
        }

        #endregion // Presentation Members
    }
}