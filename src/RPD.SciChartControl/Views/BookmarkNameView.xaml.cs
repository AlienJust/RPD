using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Messaging;
using RPD.SciChartControl.Messages;
using RPD.SciChartControl.ViewModel;

namespace RPD.SciChartControl.Views
{
    /// <summary>
    /// Interaction logic for BookmarkNameView.xaml
    /// </summary>
    internal partial class BookmarkNameView : Window
    {
        private readonly IMessenger _messenger;

        public BookmarkNameView(IMessenger messenger)
        {
            _messenger = messenger;
            InitializeComponent();

            _messenger.Register<ViewMessageWithCallback<ChartBookmarkViewModel>>(this,
                    WindowMessages.BookmarkNameWindow,
                msg =>
                {
                    if (msg.Action == ViewAction.Close)
                        Close();
                });
        }

        private void BookmarkNameTextBox_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CloseButton.Command.Execute(null);
            }
        }
    }
}
