using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RPD.SciChartControl.Messages;

namespace RPD.SciChartControl.ViewModel
{
    internal class BookmarkNameViewModel: ViewModelBase
    {
        private readonly IMessenger _messenger;
        private readonly Action<ChartBookmarkViewModel> _callback;

        private ChartBookmarkViewModel _bookmark = new ChartBookmarkViewModel();

        public BookmarkNameViewModel(IMessenger messenger, Action<ChartBookmarkViewModel> callback)
        {
            _messenger = messenger;
            _callback = callback;

            Close = new RelayCommand(CloseExecute);
        }

        private void CloseExecute()
        {
            _callback.Invoke(_bookmark);
            _messenger.Send(new ViewMessageWithCallback<ChartBookmarkViewModel>(ViewAction.Close), 
                WindowMessages.BookmarkNameWindow);
        }

        public ChartBookmarkViewModel Bookmark 
        {
            get { return _bookmark; }
            set { Set(() => Bookmark, ref _bookmark, value); }
        }

        public RelayCommand Close { get; set; }
    
    }
}