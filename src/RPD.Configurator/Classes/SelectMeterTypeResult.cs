using RPD.DAL;

namespace RPD.Configurator.Classes
{
    /// <summary>
    /// Результат выбора пользователем типа измеритель.
    /// </summary>
    public class SelectMeterTypeResult
    {
        /// <summary>
        /// True если пользватель выбрал измеритель, False если нет.
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// Тип измерителя.
        /// </summary>
        public RpdMeterType MeterType { get; set; }
    }
}
