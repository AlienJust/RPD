using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using Microsoft.Practices.ObjectBuilder2;
using RPD.DAL;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;

namespace RPD.Presentation.ViewModels.DalViewModels {
	internal class PsnConfigurationViewModel : ViewModelBase, IPsnConfigurationViewModel {
		public PsnConfigurationViewModel(IPsnConfiguration psnConfiguration, IColorsStorage colorsStorage) {
			Model = psnConfiguration;

			PsnMeterConfigs = new ObservableCollection<IPsnMeterConfigViewModel>();

			Model.PsnMeterConfigs.ForEach(m => PsnMeterConfigs.Add(new PsnMeterConfigViewModel(m, colorsStorage)));
		}

		public IPsnConfiguration Model { get; }


		public string Version => Model.Version;

		public string Name => Model.Name;

		public string Description => Model.Description;

		public string DescriptionAndVersionString {
			get {
				if (string.IsNullOrWhiteSpace(Description) && string.IsNullOrWhiteSpace(Version))
					return "";

				if (string.IsNullOrWhiteSpace(Description))
					return "Версия " + Version + ".";

				if (string.IsNullOrWhiteSpace(Version))
					return Description;

				return Description.TrimEnd('.', ' ') + ". Версия " + Version + ".";
			}
		}

		public ObservableCollection<IPsnMeterConfigViewModel> PsnMeterConfigs { get; }
	}
}
