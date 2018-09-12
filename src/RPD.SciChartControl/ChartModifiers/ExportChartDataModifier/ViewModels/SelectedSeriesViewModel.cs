using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using Abt.Controls.SciChart.Visuals.RenderableSeries;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RPD.SciChartControl.ChartModifiers.ExportChartDataModifier.Messages;
using RPD.SciChartControl.Messages;

namespace RPD.SciChartControl.ChartModifiers.ExportChartDataModifier.ViewModels
{
    /// <summary>
    /// Модель представления окна выбора серий для экспорта.
    /// </summary>
    internal class SelectedSeriesViewModel : ViewModelBase
    {
        private readonly IMessenger _messenger;

        public SelectedSeriesViewModel(IMessenger messenger, IEnumerable<IRenderableSeries> renderableSeries)
        {
            _messenger = messenger;

            Series = new ObservableCollection<SeriesViewModel>();

            foreach (var series in renderableSeries)
                Series.Add(new SeriesViewModel(series){IsChecked = true});

            OkCommand = new RelayCommand(OkCommandExecute);

            ViewUnloadCommand = new RelayCommand(() => _messenger.Send(new CloseWindowMessage()));
        }

        public ObservableCollection<SeriesViewModel> Series { get; private set; }

        public RelayCommand OkCommand { get; private set; }

        public RelayCommand ViewUnloadCommand { get; private set; }


        private void OkCommandExecute()
        {
            var msg = new SelectedSeriesMessage(Series.
                Where(x => x.IsChecked).
                Select(x => x.RenderableSeries).
                ToList());

            _messenger.Send(msg);
            _messenger.Send(new CloseWindowMessage());
        }
    }
}