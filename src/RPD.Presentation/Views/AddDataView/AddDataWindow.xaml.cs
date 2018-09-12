using System;
using System.ComponentModel;
using System.Windows;
using Dnv.Utils.Settings;
using GalaSoft.MvvmLight.Messaging;
using RPD.Presentation.Contracts;
using RPD.Presentation.Messages;
using RPD.Presentation.Utils;
using RPD.Presentation.Utils.Classes;
using RPD.Presentation.Utils.Messages;
using RPD.Presentation.ViewModels;

namespace RPD.Presentation.Views.AddDataView
{
    /// <summary>
    /// Окно "Добавить данные". Является фреймом для страниц диалога, на которых 
    /// пользователю предлагается выбрать источник данных и сами данные для копирования. После всех 
    /// действий окно закрывается и отображается окно CopyProgressWindow.
    /// </summary>
    public partial class AddDataWindow : Window
    {
        WindowStateSaver _state;

        /// <summary>
        /// Окно использует свой собственный мессенджер для того, чтобы была возможность открывать несколько таких окон одновременно.
        /// Этот мессенждер передаётся в модель представления.
        /// </summary>
        private readonly IMessenger _messenger = new Messenger();

        private readonly bool _isSimpleMode;

        public AddDataWindow(bool isSimpleMode)
        {
            _isSimpleMode = isSimpleMode;
            InitializeComponent();

            _state = new WindowStateSaver(this, ApplicationSettingsBase.LocalApplicationDataPath + "\\RPD.Presentation.AddDataWindowState.xml");

            Loaded += (s, e) => OnWindowLoaded();

            Closing += OnClosing;
        }

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            _messenger.Unregister(this);

            var viewModel = (IDisposable) DataContext;
            viewModel.Dispose();

            if (_isSimpleMode)
                Application.Current.Shutdown();

            _state.Dispose();
        }

        private void OnWindowLoaded()
        {
            ((IMessageable)DataContext).StartMessaging(_messenger);

            _messenger.Register<DialogMessage>(this, AppMessages.ErrorDialogMessage,
                msg => msg.Callback(MessageBox.Show(msg.Content, msg.Caption, MessageBoxButton.OK, MessageBoxImage.Error)));

            _messenger.Register<AddDataWindowMessages>(this, NavigateToPage);

            _messenger.Register<ViewMessage>(this, Views.CopyProggress, ShowCopyProgressWindow);

            if (_isSimpleMode)
                _messenger.Register<ViewMessage>(this, Views.ExportProgress,ShowExportProggressWindow);

            _messenger.Register<ViewMessage>(this, Views.AddData,
                (m) =>
                {
                    if (m.Action == ViewAction.Close)
                        Close();
                });
        }

        private void ShowExportProggressWindow(ViewMessage msg)
        {
            if (msg.Action == ViewAction.Show)
            {
                new ExportProgressWindow(_messenger, _isSimpleMode).Show();
                
                if (_isSimpleMode)
                    Visibility = Visibility.Hidden;
                else
                    Close();
            }
        }

        private void ShowCopyProgressWindow(ViewMessage msg)
        {
            if (msg.Action == ViewAction.Show)
            {
                new CopyProgressWindow(_messenger).Show();
                Close();
            }
        }

        /// <summary>
        /// Навигация по страницам. Источниками сообщений обычно являются ViewModels страниц, 
        /// которые хостятся в этом окне.
        /// </summary>
        /// <param name="msg">Сообщение на какую страницу перейти</param>
        void NavigateToPage(AddDataWindowMessages msg)
        {
            switch (msg)
            {
                case AddDataWindowMessages.SelectUsbDevicePage:
                    frame.NavigationService.Navigate(
                        new Uri("Views/AddDataView/SelectUsbDevicePage.xaml", UriKind.RelativeOrAbsolute));
                    break;

                case AddDataWindowMessages.SelectZippedRepositoryPage:
                    frame.NavigationService.Navigate(
                        new Uri("Views/AddDataView/SelectPathPage.xaml", UriKind.RelativeOrAbsolute));
                    break;

                case AddDataWindowMessages.SelectFolderPage:
                    frame.NavigationService.Navigate(
                        new Uri("Views/AddDataView/SelectPathPage.xaml", UriKind.RelativeOrAbsolute));
                    break;

                case AddDataWindowMessages.SelectExportPathPage:
                    frame.NavigationService.Navigate(
                        new Uri("Views/AddDataView/SelectPathPage.xaml", UriKind.RelativeOrAbsolute));
                    break;

                case AddDataWindowMessages.SelectDataSourcePage:
                    frame.NavigationService.Navigate(
                        new Uri("Views/AddDataView/SelectDataSourcePage.xaml", UriKind.RelativeOrAbsolute));
                    break;

                case AddDataWindowMessages.SelectFtpServerPage:
                    frame.NavigationService.Navigate(
                        new Uri("Views/AddDataView/SelectFtpServerPage.xaml", UriKind.RelativeOrAbsolute));
                    break;

                case AddDataWindowMessages.AvailableFaultsPage:
                    frame.NavigationService.Navigate(
                        new Uri("Views/AddDataView/AvailableFaultsPage.xaml", UriKind.RelativeOrAbsolute));
                    break;

                case AddDataWindowMessages.ReadProgressPage:
                    frame.NavigationService.Navigate(
                        new Uri("Views/AddDataView/ReadProgressPage.xaml", UriKind.RelativeOrAbsolute));
                    break;

                case AddDataWindowMessages.SelectPsnConfiguration:
                    frame.NavigationService.Navigate(
                        new Uri("Views/AddDataView/SelectPsnConfigurationPage.xaml", UriKind.RelativeOrAbsolute));
                    break;

                case AddDataWindowMessages.SelectFtpDevicePage:
                    frame.NavigationService.Navigate(
                        new Uri("Views/AddDataView/SelectFtpDevicePage.xaml", UriKind.RelativeOrAbsolute));
                    break;

                case AddDataWindowMessages.DeviceLocomotiveNameViewModel:
                    frame.NavigationService.Navigate(
                        new Uri("Views/AddDataView/DeviceLocomotiveNamePage.xaml", UriKind.RelativeOrAbsolute));
                    break;
            }
        }

        private void FrameNavigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            // Вручную выставляем датаконтекст у страниц
            var currentPage = SetPageDataContext();
            SetPageMessenger(currentPage);
        }

        private FrameworkElement SetPageDataContext()
        {
            var currentPage = frame.Content as FrameworkElement;
            if (currentPage != null)
                currentPage.DataContext = DataContext;

            return currentPage;
        }

        /// <summary>
        /// Установить мессенждер для страницы (для отобажения диалогов например).
        /// </summary>
        /// <param name="currentPage"></param>
        private void SetPageMessenger(FrameworkElement currentPage)
        {
            var messageable = currentPage as IMessageable;
            if (messageable != null)
                messageable.StartMessaging(_messenger);
        }
    }
}
