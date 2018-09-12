
using System.Windows;
using Dnv.Utils.Messages;
using Dnv.Utils.Settings;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Forms;
using RPD.Presentation.Utils.Classes;
using RPD.Configurator.Messages;


namespace RPD.Configurator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WindowStateSaver state;

        public MainWindow()
        {
            InitializeComponent();

            state = new WindowStateSaver(this, ApplicationSettingsBase.LocalApplicationDataPath + "\\RPD.Configurator.MainWindowState.xml");

            Loaded += (s, e) =>
                {                 
                    Messenger.Default.Register<DialogMessage<FolderBrowserDialog>>(this, AppMessages.ShowSaveToFileDialog, 
                        (msg) =>
                        {
                            msg.Result.DialogResult = msg.Dialog.ShowDialog();
                            msg.ProcessResult();
                        });

                    Messenger.Default.Register<DialogMessage<FolderBrowserDialog>>(this, AppMessages.ShowLoadFromFileDialog,
                        (msg) =>
                        {
                            msg.Result.DialogResult = msg.Dialog.ShowDialog();
                            msg.ProcessResult();
                        });

                    Messenger.Default.Register<DialogMessage<CommonDialog>>(this, AppMessages.ShowDialog,
                        (msg) =>
                        {
                            msg.Result.DialogResult = msg.Dialog.ShowDialog();
                            msg.ProcessResult();
                        });
                };

            Unloaded += (s, e) => { Messenger.Default.Unregister(this); };
        }
    }
}
