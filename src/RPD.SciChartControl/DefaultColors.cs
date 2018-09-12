using System.Collections.ObjectModel;
using System.Windows.Media;

namespace RPD.SciChartControl
{
    /// <summary>
    /// Цвета используемые по-умолчанию.
    /// </summary>
    public static class DefaultColors
    {
        static DefaultColors()
        {
            ColorBrushes = new ObservableCollection<SolidColorBrush>();
            ColorBrushes.Add(new SolidColorBrush(Colors.Green));
            ColorBrushes.Add(new SolidColorBrush(Colors.White));
            ColorBrushes.Add(new SolidColorBrush(Colors.Red));
            ColorBrushes.Add(new SolidColorBrush(Colors.Purple));
            ColorBrushes.Add(new SolidColorBrush(Colors.Peru));
            ColorBrushes.Add(new SolidColorBrush(Colors.SaddleBrown));
            ColorBrushes.Add(new SolidColorBrush(Colors.RoyalBlue));
            ColorBrushes.Add(new SolidColorBrush(Colors.Blue));
            ColorBrushes.Add(new SolidColorBrush(Colors.CadetBlue));
            ColorBrushes.Add(new SolidColorBrush(Colors.Cyan));
            ColorBrushes.Add(new SolidColorBrush(Colors.DarkGray));
            ColorBrushes.Add(new SolidColorBrush(Colors.DarkOrange));
            ColorBrushes.Add(new SolidColorBrush(Colors.Gold));
            ColorBrushes.Add(new SolidColorBrush(Colors.Magenta));
            ColorBrushes.Add(new SolidColorBrush(Colors.Olive));
            ColorBrushes.Add(new SolidColorBrush(Colors.SlateGray));
            ColorBrushes.Add(new SolidColorBrush(Colors.SpringGreen));
            ColorBrushes.Add(new SolidColorBrush(Colors.SkyBlue));
            ColorBrushes.Add(new SolidColorBrush(Colors.Black));
            ColorBrushes.Add(new SolidColorBrush(Colors.Snow));
            ColorBrushes.Add(new SolidColorBrush(Colors.Thistle));
            ColorBrushes.Add(new SolidColorBrush(Colors.BlueViolet));
            ColorBrushes.Add(new SolidColorBrush(Colors.Chocolate));
            ColorBrushes.Add(new SolidColorBrush(Colors.Aquamarine));
            ColorBrushes.Add(new SolidColorBrush(Colors.Azure));
            ColorBrushes.Add(new SolidColorBrush(Colors.Coral));
            ColorBrushes.Add(new SolidColorBrush(Colors.DarkBlue));
            ColorBrushes.Add(new SolidColorBrush(Colors.DarkKhaki));
            ColorBrushes.Add(new SolidColorBrush(Colors.YellowGreen));
        }

        /// <summary>
        /// Кисти цветов используемые по-умолчанию для отображения серий на графике.
        /// </summary>
        static public ObservableCollection<SolidColorBrush> ColorBrushes { get; private set; }
    }
}