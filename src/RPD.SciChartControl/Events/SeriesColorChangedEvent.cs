using System;
using System.Windows;
using System.Windows.Media;
using Abt.Controls.SciChart;

namespace RPD.SciChartControl.Events
{
    /// <summary>
    /// 
    /// </summary>
    public class SeriesColorChangedEventArgs : RoutedEventArgs
    {
        public SeriesColorChangedEventArgs(IChartSeriesViewModel series, Color color)
        {
            Series = series;
            Color = color;
        }

        /// <summary>
        /// Серия у котой сменился цвет.
        /// </summary>
        public IChartSeriesViewModel Series { get; private set; }
    
        /// <summary>
        /// Новый цвет.
        /// </summary>
        public System.Windows.Media.Color Color{ get; private set; }
    }

    /// <summary>
    /// Представляет метод, который должен обрабатывать событие смены цвета серии на графике.
    /// </summary>
    internal delegate void SeriesColorChangedEventHandler(SeriesColorChangedEventArgs e);
}