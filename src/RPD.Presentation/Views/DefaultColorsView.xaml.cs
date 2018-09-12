using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using Dnv.Utils.Messages;
using Dnv.Utils.Settings;
using GalaSoft.MvvmLight.Messaging;
using RPD.Presentation.Messages;
using RPD.Presentation.Utils;
using RPD.Presentation.Utils.Classes;
using RPD.Presentation.Utils.Messages;

namespace RPD.Presentation.Views
{
    /// <summary>
    /// Interaction logic for DefaultColorsView.xaml
    /// </summary>
    public partial class DefaultColorsView : Window
    {
        private readonly IMessenger _messenger;

        WindowStateSaver _state;

        public DefaultColorsView(IMessenger messenger)
        {
            _messenger = messenger;
            InitializeComponent();

            _state = new WindowStateSaver(this, ApplicationSettingsBase.LocalApplicationDataPath + "\\RPD.Presentation.DefaultColorsWindowState.xml");

            _messenger.Register<DialogMessage<ColorDialog>>(this,
                msg =>
                {
                    msg.Result.DialogResult = msg.Dialog.ShowDialog();
                    msg.ProcessResult();
                });

            _messenger.Register<ViewMessage>(this, Views.DefaultColorSettings,
                (msg) =>
                {
                    if (msg.Action == ViewAction.Close)
                        Close();
                });

            Closing += OnClosing;
        }

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            _state.Dispose();
            _messenger.Unregister(this);
        }
    }
}
