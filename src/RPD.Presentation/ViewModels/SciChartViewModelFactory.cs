using RPD.Presentation.Contracts.ViewModels;

namespace RPD.Presentation.ViewModels {
	internal interface ISciChartViewModelFactory {
		ISciChartViewModel Create();
	}

	class SciChartViewModelFactory : ISciChartViewModelFactory {
		private readonly IColorsStorage _colorsStorage;

		public SciChartViewModelFactory(IColorsStorage colorsStorage) {
			_colorsStorage = colorsStorage;
		}

		#region Implementation of ISciChartViewModelFactory

		public ISciChartViewModel Create() {
			return new SciChartViewModel(_colorsStorage);
		}

		#endregion
	}
}
