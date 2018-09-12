using System.Windows.Forms;
using System.Windows.Controls;
using Dnv.Utils.Messages;
using GalaSoft.MvvmLight.Messaging;
using RPD.Presentation.Contracts;
using RPD.Presentation.Messages;
using RPD.Presentation.ViewModels;

namespace RPD.Presentation.Views.AddDataView
{
    /// <summary>
    /// Страница выбора директории с данными РПД.
    /// </summary>
    public partial class SelectPathPage : Page, IMessageable
    {
        private IMessenger _messenger;

        public SelectPathPage()
        {
            InitializeComponent();

            Unloaded += (s, e) =>
                            {
                                if (_messenger != null) 
                                    _messenger.Unregister(this);
                            };
        }

        #region Implementation of IMessageable

        public void StartMessaging(IMessenger messenger)
        {
            _messenger = messenger;

            RegisterDialogMessages();
        }

        #endregion

        private void RegisterDialogMessages()
        {
            _messenger.Register<DialogMessage<FolderBrowserDialog>>(this, AppMessages.ShowSelectRdpDataPathDialog,
                (msg) =>
                {
                    msg.Result.DialogResult =
                        msg.Dialog.ShowDialog();
                    msg.ProcessResult();
                });

            _messenger.Register<DialogMessage<OpenFileDialog>>(this, AppMessages.ShowSelectRdpDataPathDialog,
                (msg) =>
                {
                    msg.Result.DialogResult =
                        msg.Dialog.ShowDialog();
                    msg.ProcessResult();
                });

            _messenger.Register<DialogMessage<SaveFileDialog>>(this, AppMessages.ShowSelectRdpDataPathDialog,
                (msg) =>
                {
                    msg.Result.DialogResult =
                        msg.Dialog.ShowDialog();
                    msg.ProcessResult();
                });

            _messenger.Register<DialogMessage>(this, AppMessages.ErrorDialogMessage, 
                msg => System.Windows.MessageBox.Show(msg.Content, msg.Caption, msg.Button, msg.Icon));
        }
    }
}
