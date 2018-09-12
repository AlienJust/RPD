using System;
using System.Globalization;
using System.Windows.Data;
using Abt.Controls.SciChart;

namespace RPD.SciChartControl.Converters
{
    [ValueConversion(typeof(bool),typeof(XyDirection))]
    class BoolToXDirectionConverter: IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (bool) value;
            return val ? XyDirection.XDirection : XyDirection.XYDirection;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var direction = (XyDirection) value;
            return direction == XyDirection.XDirection;
        }

        #endregion
    }
}
