using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPD.DalRelease.Configuration.System;

namespace RPD.DAL
{
    internal class RpdChannellDumpCondition: IDumpCondition
    {
        /// <summary>
        /// № точки подключения (индекс в массиве правил (1-47))
        /// </summary>
        public int ConnectionPointIndex { get; set; }

        /// <summary>
        /// Верхний предел, при превышении которого будет дамп
        /// </summary>
        public int HighLimit { get; set; }

        /// <summary>
        /// Флаг, указывающий используется ли верхний предел в условии
        /// </summary>
        public bool UseHighLimit { get; set; }

        /// <summary>
        /// Нижний предел, если значение уйдет ниже него - будет дамп
        /// </summary>
        public int LowLimit { get; set; }

        /// <summary>
        /// Флаг, указывающий используется ли нижний предел в условии
        /// </summary>
        public bool UseLowLimit { get; set; }

        /// <summary>
        /// Флаг использования значения по модулю при сравнении с условием
        /// </summary>
        public bool UseValueAbs { get; set; }

        /// <summary>
        /// Исользовать логическое И (то есть фактически услови дампа сработает, если значение не выйдет из диапазона, а войдет в него)
        /// </summary>
        public bool UseLogicalAnd { get; set; }

        /// <summary>
        /// Контролируемое значение
        /// </summary>
        public int ControlValue { get; set; }
        
        /// <summary>
        /// Флаг ипользования условия дампа для данного канала
        /// </summary>
        public bool UseControlValue { get; set; }

        /// <summary>
        /// Используется ли условие
        /// </summary>
        public bool IsUsed
        {
            get
            {
                return this.isUsed;
            }
            set
            {
                if (!value) this.ConnectionPointIndex = 0;
                this.isUsed = value;
            }
        }
        private bool isUsed;
        public RpdChannellDumpCondition()
        {
            IsUsed = false;
            ConnectionPointIndex = 0;
            HighLimit = 0;
            LowLimit = 0;
            UseHighLimit = false;
            UseLowLimit = false;
            UseValueAbs = false;
            UseLogicalAnd = false;
            ControlValue = 0;
            UseControlValue = false;
        }
        public RpdChannellDumpCondition(int connectionPointIndex, int hiLimit, bool useHiLimit, int lowLimit, bool useLowLimit, bool useValueAbs, bool useLogicalAnd, int controlValue, bool useControlValue, bool isUsed)
        {
            this.ConnectionPointIndex = connectionPointIndex;

            this.HighLimit = hiLimit;
            this.UseHighLimit = useHiLimit;

            this.LowLimit = lowLimit;
            this.UseLowLimit = useLowLimit;

            this.UseValueAbs = useValueAbs;
            this.UseLogicalAnd = useLogicalAnd;

            this.ControlValue = controlValue;
            this.UseControlValue = useControlValue;
            this.IsUsed = isUsed;
        }
        public bool IsEqual(RpdChannellDumpCondition anotherCondition)
        {
            bool result = true;//изначально считаем, что условия совпадают

            //затем проверяем все необходимые параметры объектов, если хотя бы один не совпадает - значит объекты не совпадают
            if (this.HighLimit != anotherCondition.HighLimit) result = false;
            else if (this.UseHighLimit != anotherCondition.UseHighLimit) result = false;

            else if (this.LowLimit != anotherCondition.LowLimit) result = false;
            else if (this.UseLowLimit != anotherCondition.UseLowLimit) result = false;

            else if (this.UseValueAbs != anotherCondition.UseValueAbs) result = false;
            else if (this.UseLogicalAnd != anotherCondition.UseLogicalAnd) result = false;

            else if (this.ControlValue != anotherCondition.ControlValue) result = false;
            else if (this.UseControlValue != anotherCondition.UseControlValue) result = false;
            
            return result;
        }

        public RpdDumpRule AsRpdDumpRule()
        {
            RpdDumpRule result = new RpdDumpRule();
            result.Type = 0;
            if (this.UseValueAbs) result.Type += 0x01;
            if (this.UseHighLimit) result.Type += 0x02;
            if (this.UseLowLimit) result.Type += 0x04;
            if (this.UseControlValue) result.Type += 0x08; //пока что условие контроля только одно - взведен 3 бит, остальные, что страше, = 0

            result.MaxValue = (short)this.HighLimit;
            result.MinValue = (short)this.LowLimit;
            result.ControlValue = (short)this.ControlValue;

            //UseLogicalAnd - пока не кодируется в RpdDumpRule
            return result;
        }

        /// <summary>
        /// Копирует значения членов класса из другого объекта с интерфейсом IDumpCondition
        /// </summary>
        /// <param name="source">Условие-источник для копирования</param>
        public void CopyFrom(IDumpCondition source)
        {
            this.IsUsed = source.IsUsed;

            this.ConnectionPointIndex = source.ConnectionPointIndex;

            this.ControlValue = source.ControlValue;
            this.HighLimit = source.HighLimit;
            this.LowLimit = source.LowLimit;

            this.UseControlValue = source.UseControlValue;

            this.UseHighLimit = source.UseHighLimit;
            this.UseLogicalAnd = source.UseLogicalAnd;
            this.UseLowLimit = source.UseLowLimit;
            this.UseValueAbs = source.UseValueAbs;
        }

        public override string ToString()
        {
            string result = "IsUsed=" + this.IsUsed.ToString() + Environment.NewLine;
            result += "ConnectionPointIndex=" + this.ConnectionPointIndex.ToString() + Environment.NewLine;
            result += "ControlValue=" + this.ControlValue.ToString() + Environment.NewLine;
            result += "HighLimit=" + this.HighLimit.ToString() + Environment.NewLine;
            result += "LowLimit=" + this.LowLimit.ToString() + Environment.NewLine;
            result += "UseControlValue=" + this.UseControlValue.ToString() + Environment.NewLine;
            result += "UseHighLimit=" + this.UseHighLimit.ToString() + Environment.NewLine;
            result += "UseLowLimit=" + this.UseLowLimit.ToString() + Environment.NewLine;
            result += "UseValueAbs=" + this.UseValueAbs.ToString() + Environment.NewLine;

            return result;
        }
    }
}
