using Abt.Controls.SciChart;

namespace RPD.SciChartControl
{
    /// <summary>
    /// Описание точки серии.
    /// </summary>
    public struct PointMetadata
    {
        /// <summary>
        /// Позиция точки в исходных данных.
        /// </summary>
        public int DataPosition;

        /// <summary>
        /// Текст команды, которой принадлежит значение точки
        /// </summary>
        public string CmdData;

        /// <summary>
        /// Валидность значения. Пока не используется.
        /// </summary>
        public bool IsValid;
    }

    /// <summary>
    /// Используется для упаковки дополнительных данных, которые могут быть связаны с серией. 
    /// Клиент должен реализовать интерфейс и может хранить в нём ссылки на свои 
    /// внутренние объекты, которые связаны с этой серией.
    /// </summary>
    public interface ISeriesAdditionalData
    {
        /// <summary>
        /// Модель представления серии на графике.
        /// </summary>
        IChartSeriesViewModel ChartSeries { get; set; }

        /// <summary>
        /// Возвращает данные точки. Используется для отображения позиции точки в исходных данных на графике.
        /// </summary>
        /// <param name="pointIndex">Номер точки</param>
        PointMetadata GetPointMetadata(int pointIndex);
    }
} 