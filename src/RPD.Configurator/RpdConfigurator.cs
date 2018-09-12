using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPD.DAL;
using RPD.Configurator.ViewModels;
using RPD.Configurator.Views;
using RPD.Configurator.Classes;
using NLog;
using System.Windows;

namespace RPD.Configurator {
	public class RpdConfigurator : IRpdConfigurator {
		private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

		readonly IDeviceConfiguration deviceConfiguration;
		MainWindow _main;

		public RpdConfigurator(IDeviceConfiguration deviceConfiguration) {
			this.deviceConfiguration = deviceConfiguration;
		}

		public void Show() {
			try {
				_main = new MainWindow {DataContext = new MainViewModel(deviceConfiguration, this)};
				_main.Show();
			}
			catch (Exception e) {
				Logger.Fatal(e, "чпок");
				throw e;
			}
		}

		public void ShowSelectMeterTypeDialog(Action<SelectMeterTypeResult> onComplete) {
			var window = new SelectMeterTypeWindow {DataContext = new SelectMeterTypeViewModel(onComplete)};
			window.Show();
		}

		public void ShowEditChannelWindow(RpdChannelViewModel rdpChannel) {
			var window = new EditRpdChannelWindow
			{
				Owner = _main,
				DataContext = new EditRpdChannelViewModel(rdpChannel, this)
			};
			window.Show();
		}

		public void ShowConnectionPointDialog(EditRpdChannelViewModel editRdpChannel) {
			var connectionPoint = new DefaultConnectionPointsWindow
			{
				DataContext = editRdpChannel.RpdChannel.ConnectionPoints,
				Owner = _main
			};
			connectionPoint.Show();
		}

		public MessageBoxResult ShowMessageBox(string content, string caption, MessageBoxImage icon, MessageBoxButton button) {
			return MessageBox.Show(content, caption, button, icon);
		}
	}
}
