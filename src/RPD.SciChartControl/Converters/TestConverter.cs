using System;
using System.Globalization;
using System.Windows.Data;

namespace RPD.SciChartControl.Converters
{
    [ValueConversion(typeof(object), typeof(object))]
    internal class TestConverter: IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
