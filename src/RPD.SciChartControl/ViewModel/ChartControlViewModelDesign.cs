using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.ChartModifiers;
using Abt.Controls.SciChart.Model.DataSeries;
using Abt.Controls.SciChart.Numerics;
using Abt.Controls.SciChart.Visuals.Axes;
using Abt.Controls.SciChart.Visuals.RenderableSeries;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RPD.SciChartControl.Converters;
using RPD.SciChartControl.Events;

namespace RPD.SciChartControl.ViewModel
{
    class ChartControlViewModelDesign : ViewModelBase, IChartControlViewModel
    {
        private ObservableCollection<ISeriesAdditionalData> _analogSeriesMetadata;
        private ObservableCollection<ISeriesAdditionalData> _discreteSeriesMetadata;

        public ChartControlViewModelDesign()
        {
            //
            IsAnnotationsVisible = true;
            IsToZoomXAxisOnly = false;
            IsAntialiasingEnabled = true;
            StrokeThickness = 1;
            SelectedResamplingMode = ResamplingMode.MinMax;
            IsMarkersVisible = false;

            //
            InitializeCommands();

            //
            DiscreteSeries = new ObservableCollection<IChartSeriesViewModel>();
            AnalogSeries = new ObservableCollection<IChartSeriesViewModel>();

            DiscreteSeriesAdditionalData = new ObservableCollection<ISeriesAdditionalData>();
            AnalogSeriesAdditionalData = new ObservableCollection<ISeriesAdditionalData>();

            RpdAnalogSeriesViewModels = new ObservableCollection<IRpdChartSeriesViewModel>();
            RpdDiscreteSeriesViewModels = new ObservableCollection<IDiscreteChartSeriesViewModel>();
            

            YAxes = new AxisCollection()
            {
                new NumericAxis()
                {
                    Id = ChartControlViewModel.MainYAxisId,
                    IsPrimaryAxis = true,
                    AutoRange = AutoRange.Once,
                    AxisAlignment = AxisAlignment.Left,
                }
            };

            ColorsBrush = new ObservableCollection<SolidColorBrush>();
            AllThemes = new string[] { };
            StrokeThicknesses = new int[] { };
            SharedXVisibleRange = new DateRange(DateTime.Now, DateTime.Now + TimeSpan.FromMinutes(12));
            AutoRange = new AutoRange();
            BindingTest = new object();
            ChartModifiers = new ObservableCollection<IChartModifier>();
            Bookmarks = new ObservableCollection<ChartBookmarkViewModel>();

            GenerateSampleData();
        }

        private void InitializeCommands()
        {
            ChartCloseCommand = new RelayCommand<IRpdChartSeriesViewModel>(o => { });
            SeriesColorCommand = new RelayCommand<SetSeriesColorCommandParameter>(o => { });
            ShowHelpWindowCommand = new RelayCommand(() => { });
            ShowSelectColorWindowCommand = new RelayCommand<IRpdChartSeriesViewModel>(o => { });
            MoveToOwnYAxis = new RelayCommand<IRpdChartSeriesViewModel>(o => { });
            MoveToMainYAxis = new RelayCommand<IRpdChartSeriesViewModel>(o => { });
            ApplyMathFunCommand = new RelayCommand(TestCommandExecute);
            TestCommand = new RelayCommand(TestCommandExecute);
            AddBookmark = new RelayCommand(TestCommandExecute);
            GoToNextBookmark = new RelayCommand(TestCommandExecute);
            GoToPreviousBookmark = new RelayCommand(TestCommandExecute);
            DeleteCurrentBookmark = new RelayCommand(TestCommandExecute);
            DeleteAllBookmarks = new RelayCommand(TestCommandExecute);
            GoToBookmark = new RelayCommand<ChartBookmarkViewModel>(model => { });
            MoveRight = new RelayCommand(TestCommandExecute);
            MoveLeft = new RelayCommand(TestCommandExecute);
            MoveUp = new RelayCommand(TestCommandExecute);
            MoveDown = new RelayCommand(TestCommandExecute);
            CombineYAxesZero = new RelayCommand(TestCommandExecute);
            MoveSignalToYAxisCommand = new RelayCommand<MoveSignalToYAxisCommandParameter>(MoveSignalToYAxisCommandExecute);
        }

        private void MoveSignalToYAxisCommandExecute(MoveSignalToYAxisCommandParameter obj)
        {
            throw new NotImplementedException();
        }


        private void TestCommandExecute()
        {
            throw new NotImplementedException();
        }

        private void GenerateSampleData()
        {
//
            var startDateTime = DateTime.Now;

            AnalogSeries.Add(GenerateSeries(startDateTime, 1, "bobo1"));
            RpdAnalogSeriesViewModels.Add(new RpdChartSeriesViewModel(AnalogSeries[0]));

            DiscreteSeries.Add(GenerateSeries(startDateTime, -1, "bobo2"));
            DiscreteSeries.Add(GenerateSeries(startDateTime, 0.2, "bobo3"));
            DiscreteSeries.Add(GenerateSeries(startDateTime, 0.5, "bobo4"));
            DiscreteSeries.Add(GenerateSeries(startDateTime, 0.7, "bobo5"));
            DiscreteSeries.Add(GenerateSeries(startDateTime, 1.2, "bobo6"));
            DiscreteSeries.Add(GenerateSeries(startDateTime, 1.5, "bobo7"));

            foreach (var chartSeriesViewModel in DiscreteSeries)
            {
                RpdDiscreteSeriesViewModels.Add(new DiscreteChartSeriesViewModel(chartSeriesViewModel));
            }
        }

        private static ChartSeriesViewModel GenerateSeries(DateTime startDateTime, double koeff, string seriesName)
        {
            var dataSeries = new XyDataSeries<DateTime, double> { SeriesName = seriesName };
            var dt2 = startDateTime;
            for (int i = 0; i < 10000; i++)
            {
                dataSeries.Append(dt2, Math.Sin(2 * Math.PI * i / 1000 + koeff));
                dt2 += TimeSpan.FromMilliseconds(20);
            }

            var series = new FastLineRenderableSeries { DataSeries = dataSeries };
            return new Abt.Controls.SciChart.ChartSeriesViewModel(dataSeries, series);
        }

        #region Implementation of IChartControlViewModel

        public ObservableCollection<IChartSeriesViewModel> AnalogSeries { get;  set; }

        public ObservableCollection<ISeriesAdditionalData> AnalogSeriesAdditionalData { get; set; }

        public ObservableCollection<IChartSeriesViewModel> DiscreteSeries { get; set; }

        public ObservableCollection<ISeriesAdditionalData> DiscreteSeriesAdditionalData { get; set; }
        public event SeriesColorChangedEventHandler OnSeriesColorChanged;

        public ObservableCollection<IRpdChartSeriesViewModel> RpdAnalogSeriesViewModels { get; set; }

        public ObservableCollection<IDiscreteChartSeriesViewModel> RpdDiscreteSeriesViewModels { get; set; }

        public AxisCollection YAxes { get; set; }
        public bool IsMultipleYAxes { get; set; }
        public bool IsAnalogSeriesItemsCountZero { get; set; }
        public bool IsDiscreteSeriesItemsCountZero { get; set; }
        public bool CombineYAxesZeroes { get; set; }

        public ObservableCollection<SolidColorBrush> ColorsBrush { get; set; }

        public bool ZoomExtentsY { get; set; }
        public bool IsAnnotationsVisible { get; set; }
        public bool IsPanEnabled { get; set; }
        public bool IsZoomEnabled { get; set; }
        public bool IsToZoomXAxisOnly { get; set; }

        public string SelectedTheme { get; set; }

        public bool IsAntialiasingEnabled { get; set; }

        public bool IsMarkersVisible { get; set; }

        public int StrokeThickness { get; set; }

        public ResamplingMode SelectedResamplingMode { get; set; }

        public AxisDragModes AxisDragMode { get; set; }

        public IEnumerable<string> AllThemes { get; private set; }
        public IEnumerable<int> StrokeThicknesses { get; private set; }

        public DateRange SharedXVisibleRange { get; set; }
        
        public AutoRange AutoRange { get; set; }

        public RelayCommand ShowHelpWindowCommand { get; set; }

        public RelayCommand<IRpdChartSeriesViewModel> ChartCloseCommand { get; set; }

        public RelayCommand<SetSeriesColorCommandParameter> SeriesColorCommand { get; set; }

        public RelayCommand<IRpdChartSeriesViewModel> ShowSelectColorWindowCommand { get; set; }

        public RelayCommand<IRpdChartSeriesViewModel> MoveToOwnYAxis { get; set; }

        public RelayCommand<IRpdChartSeriesViewModel> MoveToMainYAxis { get; set; }
        public RelayCommand ApplyMathFunCommand { get; set; }
        public RelayCommand TestCommand { get; set; }
        public RelayCommand AddBookmark { get; set; }
        public RelayCommand<ChartBookmarkViewModel> GoToBookmark { get; set; }

        public RelayCommand MoveRight { get; set; }
        public RelayCommand MoveLeft { get; set; }
        public RelayCommand MoveUp { get; set; }
        public RelayCommand MoveDown { get; set; }
        public RelayCommand<MoveSignalToYAxisCommandParameter> MoveSignalToYAxisCommand { get; set; }
        public RelayCommand CombineYAxesZero { get; set; }


        public RelayCommand GoToNextBookmark { get; set; }
        public RelayCommand GoToPreviousBookmark { get; set; }
        public RelayCommand DeleteCurrentBookmark { get; set; }
        public RelayCommand DeleteAllBookmarks { get; set; }

        public ObservableCollection<IChartModifier> ChartModifiers { get; private set; }
        public ObservableCollection<ChartBookmarkViewModel> Bookmarks { get; set; }

        public ChartDataObject SeriesData { get; set; }

        public object BindingTest { get; set; }
        public IRpdChartSeriesViewModel FindRpdChartSeriesBySeriesInfo(SeriesInfo seriesInfo)
        {
            return null;
        }

        public void SaveState(string fileName)
        {
            throw new NotImplementedException();
        }

        public void LoadState(string fileName)
        {
            throw new NotImplementedException();
        }

        public void SaveSettings(string fileName)
        {
            ;
        }

        public void LoadSetings(string fileName)
        {
        }

        #endregion
    }
}
