using System.Windows;
using System.Windows.Forms;
using Dnv.Utils.Messages;
using Dnv.Utils.Settings;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using RPD.Presentation.Messages;
using RPD.Presentation.Utils;
using RPD.Presentation.Utils.Classes;
using RPD.Presentation.Utils.Messages;
using RPD.Presentation.ViewModels;

namespace RPD.Presentation.Views
{
    /// <summary>
    /// Окно настроек приложения.
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private WindowStateSaver _state;

        public SettingsWindow()
        {
            InitializeComponent();

            _state = new WindowStateSaver(this, ApplicationSettingsBase.LocalApplicationDataPath +
                "\\RPD.Presentation.SettingsWindowState.xml");

            Messenger.Default.Register<DialogMessage<FolderBrowserDialog>>(this, Views.Settings, 
                (msg) =>
                {
                    msg.Result.DialogResult = msg.Dialog.ShowDialog();
                    msg.ProcessResult();
                });

            // Закрыть.
            Messenger.Default.Register<ViewMessage>(this, Views.Settings, 
                (msg) =>
                {
                    if (msg.Action == ViewAction.Close)
                    {
                        Close();
                    }
                });

            Closing += (e, s) =>
            {
                var vm = (SettingsViewModel)DataContext;
                if (!vm.CanClose())
                {
                    s.Cancel = true;
                    return;
                }

                Messenger.Default.Unregister(this);
                _state.Dispose();

                var cleanup = DataContext as ICleanup;
                if (cleanup != null)
                    cleanup.Cleanup();
            };
        }
    }
}
