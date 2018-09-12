using System.Windows;
using System.Windows.Threading;
using AlienJust.Adaptation.WindowsPresentation;

namespace SampleApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			DataContext = new MainWindowViewModel(new WpfUiNotifier(Dispatcher.CurrentDispatcher), new WpfWindowSystem());
		}
	}
}
