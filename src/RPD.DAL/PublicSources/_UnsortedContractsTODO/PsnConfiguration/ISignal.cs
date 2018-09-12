using System.Collections.ObjectModel;

namespace RPD.DAL
{
    /// <summary>
    /// Представляет собой аналоговый сигнал для отображения
    /// </summary>
    public interface ISignal : ILazyTrendData//<IDataPoint>
    {
        /// <summary>
        /// Список каналов, с которыми связан сигнал
        /// </summary>
        ObservableCollection<IRpdChannel> Channels { get; set; }
        
        /// <summary>
        /// Мат. формула, которая оперирует значениями сигналов
        /// </summary>
        string MathOperation { get; set; }

        /// <summary>
        /// Имя сигнала
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Авария, которой принадлежит сигнал
        /// </summary>
        IFaultLog OwnerFault { get; }
    }
}
