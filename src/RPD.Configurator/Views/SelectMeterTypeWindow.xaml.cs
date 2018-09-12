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
    /// Interaction logic for MeterTypeWindow.xaml
    /// </summary>
    public partial class SelectMeterTypeWindow : Window
    {
        //byte[] leak = new byte[50*1024*1024];

        WindowStateSaver state;

        public SelectMeterTypeWindow()
        {
            InitializeComponent();

            state = new WindowStateSaver(this, ApplicationSettingsBase.LocalApplicationDataPath + 
                "\\RPD.Configurator.SelectMeterTypeWindowState.xml");

            Loaded += (s, e) =>
                {                    
                    Messenger.Default.Register<AppMessages>(this, AppMessages.CloseSelectMeterTypeWindow, 
                        (msg) =>
                        {
                            Close();
                        });                   
                };

            Unloaded += 
                (s, e) => 
                {
                    Messenger.Default.Unregister(this); 
                };
        }
    }
}
