using System.IO;
using System.Reflection;
using GalaSoft.MvvmLight;
using RPD.Presentation.Contracts.ViewModels;

namespace RPD.Presentation.ViewModels {
	internal class AboutProgramViewModel : ViewModelBase, IAboutProgramViewModel {
		private const string VersionPattern = "version=";

		public AboutProgramViewModel() {
			GetInstallerVersion();

			PresentationVersion = typeof(App).Assembly.GetName().Version.ToString();
			PresentationDate = File.GetLastWriteTime(typeof(App).Assembly.Location).ToShortDateString();
			DalVersion = typeof(DAL.ILoader).Assembly.GetName().Version.ToString();
			DalDate = File.GetLastWriteTime(typeof(App).Assembly.Location).ToShortDateString();
		}

		private void GetInstallerVersion() {
			var fileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\version.txt";
			if (File.Exists(fileName)) {
				var text = File.ReadAllText(fileName);
				var ind = text.IndexOf(VersionPattern, System.StringComparison.Ordinal);
				InstallerVersion = text.Substring(ind + VersionPattern.Length).TrimEnd('\r', '\n');
			}
		}

		public string InstallerVersion { get; private set; }

		public string InstallerDate { get; private set; }

		public string PresentationVersion { get; }

		public string PresentationDate { get; }

		public string DalVersion { get; }

		public string DalDate { get; }
	}
}
