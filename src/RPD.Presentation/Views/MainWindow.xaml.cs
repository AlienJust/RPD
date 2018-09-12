using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using Dnv.Utils.Messages;
using Dnv.Utils.Settings;
using GalaSoft.MvvmLight.Messaging;
using RPD.Presentation.Messages;
using RPD.Presentation.Utils.Classes;
using System.IO;
using System.Windows.Controls.Primitives;
using RPD.Presentation.Utils.Messages;
using RPD.Presentation.Views.AddDataView;
using Xceed.Wpf.AvalonDock.Layout.Serialization;
using CheckBox = System.Windows.Controls.CheckBox;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;
using TreeView = System.Windows.Controls.TreeView;

namespace RPD.Presentation.Views {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		WindowStateSaver _state;
		readonly string _dockLayoutFileName = ApplicationSettingsBase.LocalApplicationDataPath + "\\mainlayout.xml";
		readonly string _defaultLayoutName = App.AppPath + "\\defaultlayout.xml";

		private readonly string _psnChartSettingsFileName = ApplicationSettingsBase.LocalApplicationDataPath +
																												"\\PsnChartSettings.json";

		private readonly string _mainChartWorkspaceFileName = ApplicationSettingsBase.LocalApplicationDataPath +
																												"\\workspace.json";

		/*
		private readonly string _psnFaultsChartSettingsFileName = ApplicationSettingsBase.LocalApplicationDataPath +
																												"\\PsnFaultsChartSettings.json";

		private readonly string _rpdChartSettingsFileName = ApplicationSettingsBase.LocalApplicationDataPath +
																												"\\RpdChartSettings.json";
		*/


		public MainWindow() {
			InitializeComponent();

			_state = new WindowStateSaver(this, ApplicationSettingsBase.LocalApplicationDataPath +
					"\\RPD.Presentation.MainWindowState.xml");

			RegisterMessages();

			LoadChartsSettings();

			Closing += OnClosing;

			MouseUp += OnMouseUp;
			KeyDown += OnKeyDown;
		}


		private void OnMouseUp(object sender, MouseButtonEventArgs mouseButtonEventArgs) {
			;
		}

		private void OnKeyDown(object sender, KeyEventArgs keyEventArgs) {

		}

		private void OnClosing(object o, CancelEventArgs cancelEventHandler) {
			SaveDockLayout();
			SaveChartsSettings();

			Messenger.Default.Unregister(this);
		}

		private void DockManagerLoaded(object sender, RoutedEventArgs e) {
			RestoreDockLayout();

			// для устранения бага AvalonDock
			layoutAnchorableStub.Hide();
		}

		private void SaveDockLayout() {
			var serializer = new XmlLayoutSerializer(dockingManager);
			using (var stream = new StreamWriter(_dockLayoutFileName))
				serializer.Serialize(stream);
		}

		private void RestoreDockLayout() {
			RestoreDefaultLayoutIfNeed();

			LoadLayout();
		}

		private void LoadLayout() {
			try {
				if (File.Exists(_dockLayoutFileName)) {
					var serializer = new XmlLayoutSerializer(dockingManager);
					using (var stream = new StreamReader(_dockLayoutFileName))
						serializer.Deserialize(stream);
				}
			}
			catch (Exception) {
				MessageBox.Show("Ошибка при восстановлении расположения окон.");
			}
		}

		private void RestoreDefaultLayoutIfNeed() {
			bool needToRestore = false;
			if (File.Exists(_dockLayoutFileName)) {
				// файл от предыдущей версии AvalonDock
				var text = File.ReadAllText(_dockLayoutFileName);
				if (text.Contains("ResizingPanel") || !text.Contains("ContentId=\"psnPowerOnLayoutAnchorable\""))
					needToRestore = true;
			}
			else
				needToRestore = true;

			if (needToRestore)
				ReplaceLayoutFileByDefault();
		}

		private void SaveChartsSettings() {
			Chart.SaveSettings(_psnChartSettingsFileName);
			/*
			psnFaultsChart.SaveSettings(_psnFaultsChartSettingsFileName);
			rpdChart.SaveSettings(_rpdChartSettingsFileName);
			*/
		}

		private void LoadChartsSettings() {
			Chart.LoadSettings(_psnChartSettingsFileName);
			/*
			psnFaultsChart.LoadSettings(_psnFaultsChartSettingsFileName);
			rpdChart.LoadSettings(_rpdChartSettingsFileName);
			*/
		}

		private void RegisterMessages() {
			// Показать окно добавления данных.
			Messenger.Default.Register<ViewMessage>(this, Views.AddData,
					msg => {
						if (msg.Action == ViewAction.Show)
							new AddDataWindow(false).Show();
					});

			// Показать окно настроек.
			Messenger.Default.Register<ViewMessage>(this, Views.HelpWindow,
					msg => {
						if (msg.Action == ViewAction.Show)
							new SciChartControl.HelpWindow().Show();
					});

			Messenger.Default.Register<ViewMessage>(this, Views.LogIntegrityStatWindow,
					msg => {
						if (msg.Action == ViewAction.Show)
							new LogIntegrityStatWindow().Show();
					});

			// Показать окно о программе.
			Messenger.Default.Register<ViewMessage>(this, Views.AboutProgramWindow,
					msg => {
						if (msg.Action == ViewAction.Show)
							new AboutProgramWindow() { Owner = this }.Show();
					});

			// Показать окно настроек.
			Messenger.Default.Register<ViewMessage>(this, Views.Settings,
					msg => {
						if (msg.Action == ViewAction.ShowModal)
							ShowSettingsWindow();
					});

			Messenger.Default.Register<ViewMessage>(this, Views.DefaultColorSettings,
					msg => {
						if (msg.Action == ViewAction.Show)
							new DefaultColorsView(Messenger.Default) { Owner = this }.Show();
					});

			Messenger.Default.Register<ViewMessage>(this, Views.ExportProgress,
					msg => {
						if (msg.Action == ViewAction.Show)
							new ExportProgressWindow(Messenger.Default, false) { Owner = this }.Show();
					});

			Messenger.Default.Register<ViewMessage>(this, Views.ChangePsnConfiguration,
					msg => {
						if (msg.Action == ViewAction.Show)
							new ChangePsnConfigurationWindow(Messenger.Default) { Owner = this }.Show();
					});

			Messenger.Default.Register<DialogMessage>(this, AppMessages.ErrorDialogMessage, ShowError);

			Messenger.Default.Register<MessageBoxMessage>(this,
			msg => {
				msg.Show(this);
				msg.ProcessResult();
			});

			Messenger.Default.Register<DialogMessage<SaveFileDialog>>(this,
					msg => {
						msg.Result.DialogResult = msg.Dialog.ShowDialog();
						msg.ProcessResult();
					});


			// Показать окно настроек с колбэком.
			Messenger.Default.Register<MessageWithCallback>(this, Views.Settings,
					msg => {
						ShowSettingsWindow();
						msg.ProcessCallback();
					});

			Messenger.Default.Register<AppMessages>(this, AppMessages.ShowPsnFakeWindow,
					msg => new PsnFakeWindow().Show());

			// Закрыть приложение.
			Messenger.Default.Register<AppMessages>(this, AppMessages.CloseApplication,
					msg => Close());


			Messenger.Default.Register<ViewModelToViewMessage>(this, ViewModelToViewMessage.SaveWorkspaceState,
					m => {
						Chart.SaveState(_mainChartWorkspaceFileName);
					});

			Messenger.Default.Register<ViewModelToViewMessage>(this, ViewModelToViewMessage.LoadWorkspaceState,
					m => {
						Chart.LoadState(_mainChartWorkspaceFileName);
					});
		}

		void ShowSettingsWindow() {
			var settings = new SettingsWindow { Owner = this };
			settings.ShowDialog();
		}

		static void ShowError(DialogMessage message) {
			MessageBoxResult res = MessageBox.Show(message.Content, message.Caption,
					message.Button, message.Icon);
			message.ProcessCallback(res);
		}

		private void DefaultLayout(object sender, RoutedEventArgs e) {
			ReplaceLayoutFileByDefault();
			LoadLayout();
		}

		private void ReplaceLayoutFileByDefault() {
			if (File.Exists(_defaultLayoutName))
				File.Copy(_defaultLayoutName, _dockLayoutFileName, true);
		}


		/// <summary>
		/// Находит все видимые чекбоксы логов (имя начинается с LogCheckBox), которые содержатся в визуальном дереве контрола.
		/// </summary>
		/// <param name="control"></param>
		/// <returns></returns>
		private static IEnumerable<CheckBox> FindVisibleLogCheckBoxes(Visual control) {
			var result = new List<CheckBox>();

			for (int i = 0, count = VisualTreeHelper.GetChildrenCount(control); i < count; i++) {
				var child = VisualTreeHelper.GetChild(control, i) as Visual;

				var checkbox = child as CheckBox;
				if (checkbox?.Visibility == Visibility.Visible && checkbox.Name.StartsWith("LogCheckBox"))
					result.Add(checkbox);

				foreach (var childTreeViewItem in FindVisibleLogCheckBoxes(child)) {
					result.Add(childTreeViewItem);
				}
			}
			return result;
		}

		private TreeView GetParentTreeView(FrameworkElement element) {
			var parent = VisualTreeHelper.GetParent(element);
			if (parent == null)
				return null;

			if (parent is TreeView view)
				return view;

			if (parent is FrameworkElement frameworkElement)
				return GetParentTreeView(frameworkElement);

			return null;
		}

		private bool _isMultiCheckStarted;

		private CheckBox _firstCheckBox;

		private void TreeViewLogCheckBox_OnChecked(object sender, RoutedEventArgs e) {
			var senderElement = (FrameworkElement)sender;

			// обязательное условие, т.к. обработчик будет автоматически вызываться даже для невидимых чекбоксов, 
			// у которых св-во IsChecked привязано к VM
			if (senderElement.Visibility != Visibility.Visible)
				return;

			if (_isMultiCheckStarted)
				return;

			if (!_leftShiftKeyDown) {
				_firstCheckBox = (CheckBox)sender;
				return;
			}


			// Выставляем флаг, чтобы избежать повторного входа в обработчик, 
			// который возникнет в следствие установки св-ва IsChecked внутри CheckAllCheckBoxesInDiapason
			_isMultiCheckStarted = true;

			var tree = GetParentTreeView(senderElement);
			CheckAllCheckBoxesInDiapason((CheckBox)senderElement, FindVisibleLogCheckBoxes(tree));

			_isMultiCheckStarted = false;
		}

		/// <summary>
		/// Отмечает все чекбоксы в списке как IsChecked начиная с первого, у которого IsChecked true и до lastCheckBox
		/// </summary>
		/// <param name="lastCheckBox">Последний чекбокс на котором нужно остановиться</param>
		/// <param name="checkboxes"></param>
		private void CheckAllCheckBoxesInDiapason(CheckBox lastCheckBox, IEnumerable<CheckBox> checkboxes) {
			var checkBoxes = checkboxes.ToArray();

			var firstIndex = Array.FindIndex(checkBoxes, it => ReferenceEquals(it, _firstCheckBox));
			var secondIndex = Array.FindIndex(checkBoxes, it => ReferenceEquals(it, lastCheckBox));

			if (secondIndex > firstIndex) {
				for (int i = firstIndex; i < secondIndex; i++) {
					checkBoxes[i].SetValue(ToggleButton.IsCheckedProperty, true);
				}
			}
			else {
				for (int i = firstIndex; i > secondIndex; i--) {
					checkBoxes[i].SetValue(ToggleButton.IsCheckedProperty, true);
				}
			}
		}

		private bool _leftShiftKeyDown;

		private void MainWindow_OnKeyDown(object sender, KeyEventArgs e) {
			_leftShiftKeyDown = e.Key == Key.LeftShift;
		}

		private void MainWindow_OnKeyUp(object sender, KeyEventArgs e) {
			if (e.Key == Key.LeftShift)
				_leftShiftKeyDown = false;
		}
	}
}
