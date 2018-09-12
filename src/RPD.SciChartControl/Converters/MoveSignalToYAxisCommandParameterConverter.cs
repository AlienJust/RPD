using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Abt.Controls.SciChart.Visuals.Axes;
using RPD.SciChartControl.ViewModel;

namespace RPD.SciChartControl.Converters
{
    internal class MoveSignalToYAxisCommandParameterConverter: IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Count() != 2)
                throw new ArgumentException("MoveSignalToYAxisCommandParameterConverter must have 2 parameters");

            var result = new MoveSignalToYAxisCommandParameter();

            foreach (var value in values)
            {
                if (value is IAxis)
                    result.Axis = (IAxis)value;
                else if (value is IRpdChartSeriesViewModel)
                    result.SeriesVm = (IRpdChartSeriesViewModel) value;
            }

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}