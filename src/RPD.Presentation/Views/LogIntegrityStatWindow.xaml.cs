using System.Windows;
using Dnv.Utils.Settings;
using RPD.Presentation.Utils.Classes;

namespace RPD.Presentation.Views
{
    /// <summary>
    /// Interaction logic for LogIntegrityStatWindow.xaml
    /// </summary>
    public partial class LogIntegrityStatWindow : Window
    {
        private WindowStateSaver _state;

        public LogIntegrityStatWindow()
        {
            InitializeComponent();

            _state = new WindowStateSaver(this, ApplicationSettingsBase.LocalApplicationDataPath + 
                "\\RPD.Presentation.LogIntegrityStatWindowState.xml");

            Closing += (sender, args) => _state.Dispose();
        }
    }
}
