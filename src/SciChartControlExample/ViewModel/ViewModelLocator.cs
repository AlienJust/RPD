using GalaSoft.MvvmLight.Ioc;
//using Microsoft.Practices.ServiceLocation;

namespace SciChartControlExample.ViewModel {
	/// <summary>
	/// This class contains static references to all the view models in the
	/// application and provides an entry point for the bindings.
	/// </summary>
	public class ViewModelLocator {
		/// <summary>
		/// Initializes a new instance of the ViewModelLocator class.
		/// </summary>
		public ViewModelLocator() {
			SimpleIoc.Default.Register<MainViewModel>();
		}

		public MainViewModel Main {
			get {
				return SimpleIoc.Default.GetInstance<MainViewModel>();
			}
		}
	}
}