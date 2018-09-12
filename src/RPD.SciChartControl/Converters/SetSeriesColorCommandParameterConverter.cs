using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using Abt.Controls.SciChart;
using RPD.SciChartControl.ViewModel;

namespace RPD.SciChartControl.Converters
{
    class SetSeriesColorCommandParameterConverter: IMultiValueConverter
    {
        #region Implementation of IMultiValueConverter

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Count() != 2)
                throw new ArgumentException("SetSeriesColorCommandParameterConverter: Неверное количесттво параметров. Должно быть 2.");

            var result = new SetSeriesColorCommandParameter();
            foreach (var value in values)
            {
                if (value is IRpdChartSeriesViewModel)
                    result.ViewModel = (IRpdChartSeriesViewModel)value;
                else if (value is Color)
                    result.Color = (Color)value;
                else if (value is SolidColorBrush)
                    result.Color = ((SolidColorBrush) value).Color;
            }

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
