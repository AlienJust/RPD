using System.ComponentModel;
using System.Windows;
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
    /// Окно отображающее прогресс экспорта данных РПД.
    /// </summary>
    public partial class ExportProgressWindow : Window
    {
        bool _canClose = false;

        readonly IMessenger _messenger;
        private readonly bool _isSimpleMode;
        private WindowStateSaver _state;

        public ExportProgressWindow(IMessenger messenger, bool isSimpleMode)
        {
            _messenger = messenger;
            _isSimpleMode = isSimpleMode;
            InitializeComponent();

            _state = new WindowStateSaver(this, ApplicationSettingsBase.LocalApplicationDataPath + "\\RPD.Presentation.ExportProgressWindowState.xml");

            ((IMessageable)DataContext).StartMessaging(_messenger);

            _messenger.Register<ViewMessage>(this, Views.ExportProgress,  
                (msg) =>
                {
                    if (msg.Action == ViewAction.Close)
                    {
                        _canClose = true;
                        Close();
                    }
                });

            _messenger.Register<DialogMessage>(this, Views.ExportProgress,
                (msg) => msg.ProcessCallback(MessageBox.Show(msg.Content, msg.Caption, msg.Button, msg.Icon)));

            Closing += OnClosing;
        }

        private void OnClosing(object p, CancelEventArgs e)
        {
            if (!_canClose)
            {
                e.Cancel = true;
                return;
            }

            _messenger.Unregister(this);

            var cleanup = DataContext as ICleanup;
            if (cleanup != null)
                cleanup.Cleanup();

            _state.Dispose();

            if (_isSimpleMode)
                Application.Current.Shutdown();
        }
    }
}
