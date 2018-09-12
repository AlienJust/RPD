using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Abt.Controls.SciChart;
using Dnv.Utils.Binding;
using RPD.SciChartControl.ViewModel;

namespace RPD.SciChartControl.Converters
{
    internal class ChartControlVmToDataPositionConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var vm = (IChartControlViewModel) ((BindingProxy) parameter).Data;
            var seriesInfo = (SeriesInfo) value;
            if (seriesInfo == null)
                return null;

            if (vm == null)
                throw new Exception("1");

            if (vm.AnalogSeriesAdditionalData == null)
                throw  new Exception("2");

            var metadata = vm.AnalogSeriesAdditionalData.
                SingleOrDefault(x => x.ChartSeries.RenderSeries == seriesInfo.RenderableSeries);

            if (metadata == null)
                metadata = vm.DiscreteSeriesAdditionalData.
                    SingleOrDefault(x => x.ChartSeries.RenderSeries == seriesInfo.RenderableSeries);

            return metadata.GetPointMetadata(seriesInfo.DataSeriesIndex).DataPosition;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
    internal class ChartControlVmToCommandDataConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var vm = (IChartControlViewModel) ((BindingProxy) parameter).Data;
            var seriesInfo = (SeriesInfo) value;
            if (seriesInfo == null)
                return null;

            if (vm == null)
                throw new Exception("1");

            if (vm.AnalogSeriesAdditionalData == null)
                throw  new Exception("2");

            var metadata = vm.AnalogSeriesAdditionalData.
                SingleOrDefault(x => x.ChartSeries.RenderSeries == seriesInfo.RenderableSeries);

            if (metadata == null)
                metadata = vm.DiscreteSeriesAdditionalData.
                    SingleOrDefault(x => x.ChartSeries.RenderSeries == seriesInfo.RenderableSeries);

            return metadata.GetPointMetadata(seriesInfo.DataSeriesIndex).CmdData;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}