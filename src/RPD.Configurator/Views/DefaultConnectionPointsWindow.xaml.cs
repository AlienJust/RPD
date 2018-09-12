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
using Dnv.Utils.Settings;
using GalaSoft.MvvmLight.Messaging;
using RPD.Configurator.Messages;
using RPD.Presentation.Utils.Classes;

namespace RPD.Configurator.Views
{
    /// <summary>
    /// Interaction logic for DefaultConnectionPointsWindow.xaml
    /// </summary>
    public partial class DefaultConnectionPointsWindow : Window
    {
        WindowStateSaver state;

        public DefaultConnectionPointsWindow()
        {
            InitializeComponent();

            state = new WindowStateSaver(this, ApplicationSettingsBase.LocalApplicationDataPath + 
                "\\RPD.Configurator.DefaultConnectionPointsWindowState.xml");

            Loaded += 
                (s, e) =>
                {
                    Messenger.Default.Register<AppMessages>(this, AppMessages.CloseDefaultConnectionPointsWindow,
                        (msg) =>
                        {
                            Close();

                            if (Owner != null)
                                Owner.Focus();
                        });
                };

            Unloaded += (s, e) => Messenger.Default.Unregister(this);
        }
    }
}
