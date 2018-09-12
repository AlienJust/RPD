using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.ChartModifiers;
using Abt.Controls.SciChart.Visuals.Axes;
using GalaSoft.MvvmLight.Command;
using RPD.SciChartControl.Converters;

namespace RPD.SciChartControl.ViewModel
{
    /// <summary>
    /// Модель представления чарта.
    /// </summary>
    internal interface IChartControlViewModel : IChartControlViewModelRelatedProperties, IChartSettings {
        /// <summary>
        /// Коллекция аналоговых графиков. Используется для хранения дополнительных данных связанных с графиком. 
        /// В эту коллекцию автоматически добавляются новые элементы при появлении новых объектов в 
        /// IChartControlViewModelRelatedProperties.AnalogSeries.
        /// </summary>
        ObservableCollection<IRpdChartSeriesViewModel> RpdAnalogSeriesViewModels { get; }

        /// <summary>
        /// Коллекция аналоговых графиков. Используется для хранения дополнительных данных связанных с графиком. 
        /// В эту коллекцию автоматически добавляются новые элементы при появлении новых объектов в 
        /// IChartControlViewModelRelatedProperties.DiscreteSeries.
        /// </summary>
        ObservableCollection<IDiscreteChartSeriesViewModel> RpdDiscreteSeriesViewModels { get; }

        /// <summary>
        /// Коллекция осей ординат. Должна содержать главную ось с Id == mainYAxis.
        /// </summary>
        AxisCollection YAxes { get; }

        bool IsMultipleYAxes { get; set; }

        bool IsAnalogSeriesItemsCountZero { get; }
        bool IsDiscreteSeriesItemsCountZero { get; }

        bool CombineYAxesZeroes { get; set; }

        ObservableCollection<SolidColorBrush> ColorsBrush { get; set; }

        IEnumerable<string> AllThemes { get; }

        IEnumerable<int> StrokeThicknesses { get; }

        /// <summary>
        /// Видимая область графика по оси X
        /// </summary>
        DateRange SharedXVisibleRange { get; set; }


        AutoRange AutoRange { get; set; }

        RelayCommand ShowHelpWindowCommand { get; set; }

        RelayCommand<IRpdChartSeriesViewModel> ChartCloseCommand { get; set; }

        RelayCommand<SetSeriesColorCommandParameter> SeriesColorCommand { get; set; }
        RelayCommand<IRpdChartSeriesViewModel> ShowSelectColorWindowCommand { get; set; }

        RelayCommand<IRpdChartSeriesViewModel> MoveToOwnYAxis { get; set; }

        RelayCommand<IRpdChartSeriesViewModel> MoveToMainYAxis { get; set; }

        RelayCommand ApplyMathFunCommand { get; set; }

        RelayCommand TestCommand { get; set; }

        RelayCommand AddBookmark { get; set; }
        RelayCommand GoToNextBookmark { get; set; }
        RelayCommand GoToPreviousBookmark { get; set; }
        RelayCommand DeleteCurrentBookmark { get; set; }
        RelayCommand DeleteAllBookmarks { get; set; }
        RelayCommand<ChartBookmarkViewModel> GoToBookmark { get; set; }

        RelayCommand MoveRight { get; set; }
        RelayCommand MoveLeft { get; set; }
        RelayCommand MoveUp { get; set; }
        RelayCommand MoveDown { get; set; }

        RelayCommand<MoveSignalToYAxisCommandParameter> MoveSignalToYAxisCommand { get; set; }

        ObservableCollection<IChartModifier> ChartModifiers { get; }

        ObservableCollection<ChartBookmarkViewModel> Bookmarks { get; set; }

        //ObservableCollection<AxisViewModel>  YAxes { get; set; }

        ChartDataObject SeriesData { get; set; }

        object BindingTest { get; set; }
        IRpdChartSeriesViewModel FindRpdChartSeriesBySeriesInfo(SeriesInfo seriesInfo);
        void SaveState(string fileName);
        void LoadState(string fileName);
    }
}