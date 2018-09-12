using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Threading;
using Abt.Controls.SciChart.ChartModifiers;
using Abt.Controls.SciChart.Visuals.Annotations;
using Abt.Controls.SciChart.Visuals.RenderableSeries;
using GalaSoft.MvvmLight.Messaging;
using RPD.SciChartControl.ChartModifiers.ExportChartDataModifier.Messages;
using RPD.SciChartControl.ChartModifiers.ExportChartDataModifier.ViewModels;
using RPD.SciChartControl.ChartModifiers.ExportChartDataModifier.Views;
using RPD.SciChartControl.Messages;
using Application = System.Windows.Application;
using MouseButtons = Abt.Controls.SciChart.ChartModifiers.MouseButtons;

namespace RPD.SciChartControl.ChartModifiers.ExportChartDataModifier
{
    /// <summary>
    /// Экспортирует данные серий, которые находятся на графике в заданном диапазоне. Диапазон задается мышью.
    /// </summary>
    internal class ExportChartDataModifier : ChartModifierBase
    {
        // Количество линий границ диапазона, помещенных на график.
        private int _dataRangeLinesCount = 0;
        private double _rangeBeginXCoord;
        private double _rangeEndXCoord;
        private bool _isExportMode;
        private const string LineAnnotationNamePrefix = "_xxDataRangeLine";

        private Timer _removeAnnotationsDelayTimer;
        private Dispatcher _uiDispatcher;

        private readonly ISeriesDataExporter _exporter;
        private readonly Messenger _messenger = new Messenger();
        private SelectSeriesWindow _selectSeriesWindow;

        public ExportChartDataModifier()
        {
            _exporter = new SeriesToExcelExporter();
            _messenger.Register<SelectedSeriesMessage>(this, OnSeriesSelected);
        }


        /// <summary>
        /// При установке в true включается режим экспорта данных, при котором первое нажатие левой 
        /// кнопки мыши на графике добавляет вертикальную линию, которая соответсвует левой границе
        /// дипазона, а второе правую границу. После этого данные экспортируются и линию автоматически исчезают.
        /// </summary>
        public bool IsExportMode 
        {
            get { return _isExportMode; }

            set
            {
                if (_isExportMode == value)
                    return;

                if (value == false)
                    _dataRangeLinesCount = 0;

                _isExportMode = value;
                OnPropertyChanged("IsExportMode");
            }
        }

        public override void OnModifierMouseUp(ModifierMouseArgs e)
        {
            if (!IsExportMode || _exporter == null)
                return;

            if (e.MouseButtons != MouseButtons.Left)
                return;

            _uiDispatcher = Application.Current.Dispatcher;

            AddDataRangeBoundaryAnnotation(e.MousePoint.X);

            if (IsFirstClick())
            {
                _rangeBeginXCoord = e.MousePoint.X;
                _dataRangeLinesCount++;
            }
            else
            {
                _rangeEndXCoord = e.MousePoint.X;
                Export();
            }

            base.OnModifierMouseUp(e);
        }

        private bool IsFirstClick()
        {
            return _dataRangeLinesCount == 0;
        }

        private void Export()
        {
            if (ParentSurface.RenderableSeries.Count == 0)
            {
                Cleanup();
                return;
            }

            if (ParentSurface.RenderableSeries.Count == 1)
            {
                ExportData(ParentSurface.RenderableSeries);
                return;
            }

            ShowSelectSeriesWindow();
        }

        private void OnSeriesSelected(SelectedSeriesMessage message)
        {
            if (_selectSeriesWindow == null)
                return;

            ExportData(message.Series);
            _selectSeriesWindow.Close();
            _selectSeriesWindow = null;
        }

        private void OnSelectedSeriesWindowClose(object sender, RoutedEventArgs e)
        {
            //_selectSeriesWindow.Unloaded -= OnSelectedSeriesWindowClose;
            Cleanup();
        }

        private void ShowSelectSeriesWindow()
        {
            _selectSeriesWindow = new SelectSeriesWindow();
            _selectSeriesWindow.Unloaded += OnSelectedSeriesWindowClose;

            var vm = new SelectedSeriesViewModel(_messenger, ParentSurface.RenderableSeries);
            _selectSeriesWindow.DataContext = vm;
            _selectSeriesWindow.Show();
        }

        private void ExportData(IEnumerable<IRenderableSeries> renderableSeries)
        {
            AddSeriesToExport(renderableSeries);

            _exporter.ExportAll();

            Cleanup();
        }

        private void Cleanup()
        {
            IsExportMode = false;
            _dataRangeLinesCount = 0;

            StartRemoveAnnotationsTimer();
        }

        private void AddSeriesToExport(IEnumerable<IRenderableSeries> renderableSeries)
        {
            var axisRangeBegin = GetXAxisDataValueFixed(_rangeBeginXCoord);
            var axisRangeEnd = GetXAxisDataValueFixed(_rangeEndXCoord);

            foreach (var series in renderableSeries)
            {
                var firstValue = series.DataSeries.XValues.Cast<IComparable>().
                    First(x => x.CompareTo(axisRangeBegin) >= 0);

                var last = series.DataSeries.XValues.Cast<IComparable>().
                    Last(x => x.CompareTo(axisRangeEnd) <= 0);

                var firstIndex = series.DataSeries.XValues.IndexOf(firstValue);
                var lastIndex = series.DataSeries.XValues.IndexOf(last);

                _exporter.AddSeries(series.DataSeries, firstIndex, lastIndex);
            }
        }


        private void StartRemoveAnnotationsTimer()
        {
            _removeAnnotationsDelayTimer = new Timer();
            _removeAnnotationsDelayTimer.Interval = 1000;
            _removeAnnotationsDelayTimer.Tick += RemoveAnnotationsDelayTimerOnTick;
            _removeAnnotationsDelayTimer.Start();
        }

        private void RemoveAnnotationsDelayTimerOnTick(object sender, EventArgs eventArgs)
        {
            _removeAnnotationsDelayTimer.Enabled = false;
            _removeAnnotationsDelayTimer.Dispose();
            _uiDispatcher.Invoke(new Action(RemoveDataRangeAnnotations));
        }

        private void AddDataRangeBoundaryAnnotation(double x)
        {
            var anot = new VerticalLineAnnotation()
            {
                CoordinateMode = AnnotationCoordinateMode.Absolute,
                Stroke = new SolidColorBrush(Colors.Red),
                YAxisId = "mainYAxis",
                X1 = GetXAxisDataValueFixed(x),
                IsEditable = false,
                Name = LineAnnotationNamePrefix + _dataRangeLinesCount
            };

            ParentSurface.Annotations.Add(anot);
        }

        
        private IComparable GetXAxisDataValueFixed(double x)
        {
            var axesWidth = ParentSurface.YAxes.Sum(axis => axis.ActualWidth);

            return ParentSurface.XAxis.GetDataValue(x - axesWidth);
        }

        private void RemoveDataRangeAnnotations()
        {
            var lines = ParentSurface.Annotations.Where(x =>
            {
                var m = x as AnnotationBase;
                if (m != null)
                    return m.Name.Contains(LineAnnotationNamePrefix);
                
                return false;
            }).Select(x => x).ToArray();

            foreach (var annotation in lines)
                ParentSurface.Annotations.Remove(annotation);
        }
    }
}