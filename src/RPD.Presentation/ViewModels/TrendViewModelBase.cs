using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using RPD.DAL;
using RPD.Presentation.Contracts.ViewModels;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;
using RPD.Presentation.Messages;
using System.Collections.ObjectModel;
using Color = System.Windows.Media.Color;

namespace RPD.Presentation.ViewModels
{    
    abstract class TrendViewModelBase<TChild> : ViewModelBase, ITrendViewModel where TChild : ITrendViewModel
    {
        #region Private Properties Fields

        private readonly ObservableCollection<IFaultViewModel> _faults = new ObservableCollection<IFaultViewModel>();
        private bool _isOnPlot = false;
        private bool _isTrendLoading = false;

        #endregion


        #region Public Properties

        public abstract ILazyTrendData TrendData { get; }

        public abstract TrendChartType TrendChartType { get; set; }

        public abstract string Name { get; }

        public abstract string Title { get; }

        public Color Color { get; set; }

        public abstract bool IsEnabled { get; set; }

        public abstract Guid Uid { get; }

        public abstract Guid ConfigGuid { get; }

        public ObservableCollection<IFaultViewModel> Faults
        {
            get { return _faults; }
        }

        /// <summary>
        /// Находится ли тренд на графике. 
        /// При установке этого свойства генерируется сообщение IsTrendOnPlotChangedMessage<TChild>.
        /// </summary>
        public bool IsOnPlot
        {
            get { return _isOnPlot; }

            set
            {
                // проверка нужна чтобы IsTrendOnPlotChangedMessage не посылалось дважды.
                if (_isOnPlot == value)
                    return;

                _isOnPlot = value;
                RaisePropertyChanged(() => IsOnPlot);

                Messenger.Default.Send(new IsTrendOnPlotChangedMessage(this));
            }
        }
     
        /// <summary>
        /// Индикация того, что тренд находится в процессе загрузки.
        /// </summary>
        public bool IsTrendLoading
        {
            get { return _isTrendLoading; }
            set { Set(() => IsTrendLoading, ref _isTrendLoading, value); }
        }

        #endregion
    }    
}