using System.Collections.ObjectModel;
using Abt.Controls.SciChart;
using RPD.SciChartControl.Events;

namespace RPD.SciChartControl.ViewModel
{
    /// <summary>
    /// Свойства модели представления связанные со свойствами представления ChartControl. 
    /// Автоматически обновляются при изменении аналогичных св-в в представлении. Например, когда клиент данного 
    /// UserConrol присваивает новое значение ChartControl.AnalogSeries (которое DependencyProperty).
    /// </summary>
    internal interface IChartControlViewModelRelatedProperties {
        /// <summary>
        /// Модели представления аналоговых графиков. Является ссылкой на ChartControl.AnalogSeries.
        /// Вид биндится к этому св-ву.
        /// </summary>
        ObservableCollection<IChartSeriesViewModel> AnalogSeries { get; set; }

        /// <summary>
        /// Коллекция дополнительных данных связанных с серией. Данные должны добавлять вручную 
        /// одновременно с добавлением данных в AnalogSeries.
        /// </summary>
        ObservableCollection<ISeriesAdditionalData> AnalogSeriesAdditionalData { get; set; }

        /// <summary>
        /// Модели представления дискретных графиков. Является ссылкой на ChartControl.DiscreteSeries.
        /// Вид биндится к этому св-ву.
        /// </summary>
        ObservableCollection<IChartSeriesViewModel> DiscreteSeries { get; set; }

        /// <summary>
        /// Коллекция дополнительных данных связанных с серией. Данные должны добавлять вручную 
        /// одновременно с добавлением данных в DiscreteSeries.
        /// </summary>
        ObservableCollection<ISeriesAdditionalData> DiscreteSeriesAdditionalData { get; set; }

        /// <summary>
        /// Возникает при смене цвета серии на графике.
        /// </summary>
        event SeriesColorChangedEventHandler OnSeriesColorChanged;
    }
}