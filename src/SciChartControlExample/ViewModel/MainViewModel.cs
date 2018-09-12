using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.Model.DataSeries;
using Abt.Controls.SciChart.Visuals.Annotations;
using Abt.Controls.SciChart.Visuals.RenderableSeries;
using GalaSoft.MvvmLight;

namespace SciChartControlExample.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private const int MaxPoints = 10000;
        private readonly ObservableCollection<IChartSeriesViewModel> _chartSeries = new ObservableCollection<IChartSeriesViewModel>();
        private readonly ObservableCollection<IChartSeriesViewModel> _discreteSeries = new ObservableCollection<IChartSeriesViewModel>();
        private readonly ObservableCollection<IAnnotation> _annotations = new ObservableCollection<IAnnotation>();

        public MainViewModel()
        {
            var startDateTime = DateTime.Now;

            ChartSeries.Add(GenerateSeries(startDateTime, 1, "analog bobo 1"));
            ChartSeries.Add(GenerateSeries(startDateTime, 1, "analog bobo 2"));

            DiscreteSeries.Add(GenerateDiscreteSeries(startDateTime, 1, "discrete bobo 1"));
            DiscreteSeries.Add(GenerateDiscreteSeries(startDateTime, -1, "discrete bobo3"));
            DiscreteSeries.Add(GenerateDiscreteSeries(startDateTime, 1.5, "discrete bobo4"));
            DiscreteSeries.Add(GenerateDiscreteSeries(startDateTime, -0.5, "discrete bobo5"));
            DiscreteSeries.Add(GenerateDiscreteSeries(startDateTime, -0.3, "discrete bobo6"));
            DiscreteSeries.Add(GenerateDiscreteSeries(startDateTime, 0.3, "discrete bobo7"));

            Annotations.Add(new VerticalLineAnnotation()
                                {
                                    X1 = startDateTime + TimeSpan.FromMilliseconds(60), 
                                    Stroke = new SolidColorBrush(Colors.Aqua)
                                });

        }

        private static ChartSeriesViewModel GenerateSeries(DateTime startDateTime, double koeff, string seriesName)
        {
            var dataSeries = new XyDataSeries<DateTime, double> {SeriesName = seriesName};
            
            var dt2 = startDateTime;
            for (int i = 0; i < MaxPoints; i++)
            {
                dataSeries.Append(dt2, Math.Sin(2 * Math.PI * i / 1000 + koeff));
                dt2 += TimeSpan.FromMilliseconds(20);
            }

            var series = new FastLineRenderableSeries {DataSeries = dataSeries};
            series.SeriesColor = Colors.Green;
            return new ChartSeriesViewModel(dataSeries, series);
        }

        private static ChartSeriesViewModel GenerateDiscreteSeries(DateTime startDateTime, double koeff, string seriesName)
        {
            var dataSeries = new XyDataSeries<DateTime, double> { SeriesName = seriesName };
            var dt2 = startDateTime;
            var r = new Random();

            for (int i = 0; i < MaxPoints; i++)
            {
                var val = Math.Sin(2*Math.PI*i/1000 + koeff);
                dataSeries.Append(dt2, Math.Round(val>0?val:0));
                dt2 += TimeSpan.FromMilliseconds(20);
            }

            var series = new FastLineRenderableSeries { DataSeries = dataSeries };
            return new ChartSeriesViewModel(dataSeries, series);
        }

        public ObservableCollection<IChartSeriesViewModel> ChartSeries
        {
            get
            {
                return _chartSeries;
            }
        }

        public ObservableCollection<IChartSeriesViewModel> DiscreteSeries
        {
            get
            {
                return _discreteSeries;
            }
        }

        public ObservableCollection<IAnnotation> Annotations
        {
            get { return _annotations; }
        }
    }
}