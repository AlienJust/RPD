using System.ComponentModel;
using System.Windows;
//using Dnv.Utils.Settings;
using Dnv.Utils.Settings;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using RPD.Presentation.Contracts;
using RPD.Presentation.Messages;
using RPD.Presentation.Utils;
using RPD.Presentation.Utils.Classes;
using RPD.Presentation.Utils.Messages;
using RPD.Presentation.ViewModels;

namespace RPD.Presentation.Views
{
    /// <summary>
    /// Отображает прогресс копирования.
    /// </summary>
    public partial class CopyProgressWindow : Window
    {
        bool _canClose;

        private readonly IMessenger _messenger;
        private WindowStateSaver _state;


        public CopyProgressWindow(IMessenger messenger)
        {
            _messenger = messenger;

            InitializeComponent();

            _state = new WindowStateSaver(this, ApplicationSettingsBase.LocalApplicationDataPath + "\\RPD.Presentation.CopyProgressWindowState.xml");

            ((IMessageable)DataContext).StartMessaging(_messenger);

            Loaded += OnLoaded;
            Closing += OnClosing;
        }

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            if (!_canClose)
            {
                cancelEventArgs.Cancel = true;
                return;
            }

            _messenger.Unregister(this);

            var cleanup = DataContext as ICleanup;
            if (cleanup != null)
                cleanup.Cleanup();

            _state.Dispose();
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _messenger.Register<ViewMessage>(this, Views.CopyProggress, 
                (msg) =>
                {
                    if (msg.Action == ViewAction.Close)
                    {
                        _canClose = true;
                        Close();
                    }
                });

            _messenger.Register<DialogMessage>(this, AppMessages.CopyProgressError,
                (msg) => msg.ProcessCallback(MessageBox.Show(msg.Content, msg.Caption, msg.Button, msg.Icon)));
        }
    }
}
