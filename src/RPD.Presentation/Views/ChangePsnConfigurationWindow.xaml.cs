using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for ChangePsnConfigurationWindow.xaml
    /// </summary>
    public partial class ChangePsnConfigurationWindow : Window
    {
        private readonly IMessenger _messenger;
        private WindowStateSaver _state;

        public ChangePsnConfigurationWindow(IMessenger messenger)
        {
            _messenger = messenger;

            InitializeComponent();
            _state = new WindowStateSaver(this, ApplicationSettingsBase.LocalApplicationDataPath + "\\RPD.Presentation.ChangePsnConfigurationWindowState.xml");

            ((IMessageable)DataContext).StartMessaging(_messenger);

            _messenger.Register<ViewMessage>(this, Views.ChangePsnConfiguration,
                (msg) =>
                {
                    if (msg.Action == ViewAction.Close)
                    {
                        Close();
                    }
                });

            Closing += OnClosing;
        }

        private void OnClosing(object p, CancelEventArgs e)
        {
            _messenger.Unregister(this);

            var cleanup = DataContext as ICleanup;
            if (cleanup != null)
                cleanup.Cleanup();

            _state.Dispose();
        }
    }
}
