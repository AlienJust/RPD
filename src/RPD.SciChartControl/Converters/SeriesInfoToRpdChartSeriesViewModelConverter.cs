using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Abt.Controls.SciChart;
using RPD.SciChartControl.ViewModel;

namespace RPD.SciChartControl.Converters
{
    class SeriesInfoToRpdChartSeriesViewModelConverter: IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var ex = "You must provide two values. First of type SeriesVm and second of type ChartControlViewModel";
            if (values.Count() != 2)
                throw new ArgumentException(ex);

            if (!(values[0] is SeriesInfo) || !(values[1] is ChartControlViewModel))
                throw new ArgumentException(ex);

            return ((ChartControlViewModel)values[1]).FindRpdChartSeriesBySeriesInfo((SeriesInfo)values[0]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
