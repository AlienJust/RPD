using System.Collections.ObjectModel;
using RPD.DAL;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;

namespace RPD.Presentation.ViewModels.DalViewModels {
	internal class PsnMeterConfigViewModel : IPsnMeterConfigViewModel {
		private readonly IPsnMeterConfig _model;

		/// <param name="model">Модель</param>
		/// <param name="colorsStorage">Хранилище цветов. Может быть null</param>
		public PsnMeterConfigViewModel(IPsnMeterConfig model, IColorsStorage colorsStorage) {
			_model = model;

			foreach (var psnChannelConfig in _model.PsnChannelConfigs) {
				PsnChannelConfigs.Add(new PsnChannelConfigViewModel(psnChannelConfig, colorsStorage));
			}
		}

		public string Name => _model.Name;

		public ObservableCollection<IPsnChannelConfigViewModel> PsnChannelConfigs { get; } = new ObservableCollection<IPsnChannelConfigViewModel>();
	}
}