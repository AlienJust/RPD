using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace RPD.Exporter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Process.Start(new ProcessStartInfo
                        {
                            Arguments = "-mode:simple",
                            FileName =
                                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                                "\\RPD.exe",
                            CreateNoWindow = true
                        });
            Shutdown();
        }
    }
}
