

namespace RPD.Configurator.Classes
{

    public class ConditionStringHelper 
    {
        public ConditionStringHelper()
        {

        }

        /// <summary>
        /// Генерирует сроку вида this.ConditionString
        /// </summary>
        static public string Generate(bool useDumpCondition, bool useValueAbs, bool useHighLimit,
            bool useLowLimit, int highLimit, int lowLimit)
        {
            if (!useDumpCondition)
                return "";

            string val = useValueAbs ? "|Значение|" : "Значение";

            string left = useHighLimit ? val + " > " + highLimit.ToString():"";

            string right = useLowLimit ? val + " < " + lowLimit.ToString():"";

            string result = "";
            if (useHighLimit && useLowLimit)
                result = left + " или " + right;
            else
                result = left + right;

            return result;
        }
    }
}