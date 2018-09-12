using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RPD.Configurator.Messages;
using RPD.DAL;
using RPD.Configurator.Classes;
using System.ComponentModel;

namespace RPD.Configurator.ViewModels
{
    /// <summary>
    /// Модель представления условия регистрации аварии на канале измерителя.
    /// </summary>
    public class DumpConditionViewModel : ViewModelBase
    {       
        public DumpConditionViewModel(IDumpCondition dumpCondition)
        {
            this.ConnectionPointIndex = dumpCondition.ConnectionPointIndex;
            this.ControlValue = dumpCondition.ControlValue;
            this.HighLimit = dumpCondition.HighLimit;
            this.IsUsed = dumpCondition.IsUsed;
            this.LowLimit = dumpCondition.LowLimit;
            this.UseControlValue = dumpCondition.UseControlValue;
            this.UseHighLimit = dumpCondition.UseHighLimit;
            this.UseLowLimit = dumpCondition.UseLowLimit;
            this.UseValueAbs = dumpCondition.UseValueAbs;
        }
        
        public DumpConditionViewModel(DumpConditionViewModel dumpCondition)
        {
            this.ConnectionPointIndex = dumpCondition.ConnectionPointIndex;
            this.ControlValue = dumpCondition.ControlValue;
            this.HighLimit = dumpCondition.HighLimit;
            this.IsUsed = dumpCondition.IsUsed;
            this.LowLimit = dumpCondition.LowLimit;
            this.UseControlValue = dumpCondition.UseControlValue;
            this.UseHighLimit = dumpCondition.UseHighLimit;
            this.UseLowLimit = dumpCondition.UseLowLimit;
            this.UseValueAbs = dumpCondition.UseValueAbs; 
        }
        
        /*
        /// <summary>
        /// Условие регистрации.
        /// </summary>
        public IDumpCondition DumpCondition
        { 
            get {return _dumpCondition;}
            set
            {
                _dumpCondition = value;
                // обновляем все биндинги
                
                RaisePropertyChanged(IsUsedPropertyName);
                RaisePropertyChanged(HighLimitPropertyName);
                RaisePropertyChanged(LowLimitPropertyName);
                RaisePropertyChanged(UseHighLimitPropertyName);
                RaisePropertyChanged(UseLowLimitPropertyName);
                RaisePropertyChanged(UseValueAbsPropertyName);
                RaisePropertyChanged(ConditionStringPropertyName);
                RaisePropertyChanged(UseControlValuePropertyName);
                RaisePropertyChanged(ControlValuePropertyName);
                RaisePropertyChanged(ControlValueStringPropertyName);
                RaisePropertyChanged(ConnectionPointIndexPropertyName);
                
            }
        }
        */

        #region Presentation Members

        public const string IsUsedPropertyName = "IsUsed";
        bool _isUsed = false;

        public bool IsUsed
        {
            get
            {
                return _isUsed;
            }

            set
            {
                if (_isUsed == value)
                {
                    return;
                }

                _isUsed = value;
                
                RaisePropertyChanged(IsUsedPropertyName);
                RaisePropertyChanged(ConditionStringPropertyName);
            }
        }


        public const string HighLimitPropertyName = "HighLimit";
        int _highLimit = 0;

        public int HighLimit
        {
            get
            {
                return _highLimit;
            }

            set
            {
                if (_highLimit == value)
                {
                    return;
                }

                _highLimit = value;

                RaisePropertyChanged(HighLimitPropertyName);
                RaisePropertyChanged(ConditionStringPropertyName);
            }
        }


        public const string LowLimitPropertyName = "LowLimit";
        int _lowLimit = 0;
        public int LowLimit
        {
            get
            {
                return _lowLimit;
            }

            set
            {
                if (_lowLimit == value)
                {
                    return;
                }

                _lowLimit = value;
                RaisePropertyChanged(LowLimitPropertyName);
                RaisePropertyChanged(ConditionStringPropertyName);
            }
        }


        public const string UseHighLimitPropertyName = "UseHighLimit";
        bool _useHighLimit = false;

        public bool UseHighLimit
        {
            get
            {
                return _useHighLimit;
            }

            set
            {
                if (_useHighLimit == value)
                {
                    return;
                }

                _useHighLimit = value;
                RaisePropertyChanged(UseHighLimitPropertyName);
                RaisePropertyChanged(ConditionStringPropertyName);
            }
        }


        public const string UseLowLimitPropertyName = "UseLowLimit";
        bool _useLowLimit = false;

        public bool UseLowLimit
        {
            get
            {
                return _useLowLimit;
            }

            set
            {
                if (_useLowLimit == value)
                {
                    return;
                }

                _useLowLimit = value;

                RaisePropertyChanged(UseLowLimitPropertyName);
                RaisePropertyChanged(ConditionStringPropertyName);
            }
        }


        public const string UseValueAbsPropertyName = "UseValueAbs";
        bool _useValueAbs = false;

        public bool UseValueAbs
        {
            get
            {
                return _useValueAbs;
            }

            set
            {
                if (_useValueAbs == value)
                {
                    return;
                }

                _useValueAbs = value;
                RaisePropertyChanged(UseValueAbsPropertyName);
                RaisePropertyChanged(ConditionStringPropertyName);
            }
        }


        public const string ConditionStringPropertyName = "ConditionString";

        public string ConditionString
        {
            get
            {
                return ConditionStringHelper.Generate(IsUsed, UseValueAbs, UseHighLimit, 
                    UseLowLimit, HighLimit, LowLimit);
            }
        }


        public const string UseControlValuePropertyName = "UseControlValue";
        bool _useControlValue = false;

        public bool UseControlValue 
        {
            get
            {
                return _useControlValue;
            }

            set
            {
                if (_useControlValue  == value)
                {
                    return;
                }

                _useControlValue  = value;
                RaisePropertyChanged(UseControlValuePropertyName);
                RaisePropertyChanged(ControlValueStringPropertyName);
            }
        }


        public const string ControlValuePropertyName = "ControlValue";
        int _controlValue;

        public int ControlValue
        {
            get
            {
                return _controlValue;
            }

            set
            {
                if (_controlValue == value)
                {
                    return;
                }

                _controlValue = value;
                RaisePropertyChanged(ControlValuePropertyName);
                RaisePropertyChanged(ControlValueStringPropertyName);
            }
        }


        public const string ControlValueStringPropertyName = "ControlValueString";

        public string ControlValueString
        {
            get
            {
                return ControlValue.ToString();
            }
        }


        public const string ConnectionPointIndexPropertyName = "ConnectionPointIndex";
        int _connectionPointIndex = 0;

        public int ConnectionPointIndex
        {
            get
            {
                return _connectionPointIndex;
            }

            set
            {
                if (_connectionPointIndex == value)
                {
                    return;
                }
                _connectionPointIndex = value;
                RaisePropertyChanged(ConnectionPointIndexPropertyName);
            }
        }

        #endregion // Presentation Members


        public void CopyTo(IDumpCondition dumpCondition)
        {
            dumpCondition.ConnectionPointIndex = ConnectionPointIndex;
            dumpCondition.ControlValue = ControlValue;
            dumpCondition.HighLimit = HighLimit;
            dumpCondition.IsUsed = IsUsed;
            dumpCondition.LowLimit = LowLimit;
            dumpCondition.UseControlValue = UseControlValue;
            dumpCondition.UseHighLimit = UseHighLimit;
            dumpCondition.UseLowLimit = UseLowLimit;
            dumpCondition.UseValueAbs = UseValueAbs;
        }

    }
}