using System;
using RPD.Configurator.ViewModels;
using RPD.Configurator.Classes;
using System.Windows;

namespace RPD.Configurator
{
    public interface IRpdConfigurator
    {
        void Show();
        void ShowEditChannelWindow(RpdChannelViewModel rdpChannel);
        void ShowSelectMeterTypeDialog(Action<SelectMeterTypeResult> onComplete);
        void ShowConnectionPointDialog(EditRpdChannelViewModel rdpChannel);
        MessageBoxResult ShowMessageBox(string content, string caption, MessageBoxImage icon, MessageBoxButton button);
    }
}
