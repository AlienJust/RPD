using System;
using System.Windows;
using System.Windows.Threading;
using System.Linq;
using GalaSoft.MvvmLight.Threading;
using NLog;
using RPD.Presentation.ViewModels;
using RPD.Presentation.Views;
using RPD.Presentation.Views.AddDataView;

namespace RPD.Presentation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public App()
        {
            DispatcherUnhandledException += OnDispatcherUnhandledException;
            Exit += OnExit;
        }

        private void OnExit(object sender, ExitEventArgs exitEventArgs)
        {
            var locator = (ViewModelLocator) Resources["Locator"];
            locator.Dispose();
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args)
        {

            MessageBox.Show(args.Exception.Message + "\n\n"+ args.Exception.StackTrace);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (IsSimpleMode)
                new AddDataWindow(IsSimpleMode).Show();
            else
                new MainWindow().Show();
        }

        static App()
        {
            DispatcherHelper.Initialize();
        }

        static public string AppPath
        {
            get
            {
                return System.IO.Path.GetDirectoryName(System.Diagnostics.Process.
                    GetCurrentProcess().MainModule.FileName);
            }
        }

        public static bool IsSimpleMode
        {
            get { return Environment.GetCommandLineArgs().Contains("-mode:simple"); }
        }
    }
}
