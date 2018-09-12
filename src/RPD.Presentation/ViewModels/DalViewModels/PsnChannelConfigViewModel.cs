using System;
using GalaSoft.MvvmLight;
using RPD.DAL;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;
using Color = System.Windows.Media.Color;

namespace RPD.Presentation.ViewModels.DalViewModels {
	internal class PsnChannelConfigViewModel : ViewModelBase, IPsnChannelConfigViewModel {
		private readonly IColorsStorage _colorsStorage;
		private Color _color;

		/// <param name="model">Модель</param>
		/// <param name="colorsStorage">Хранилище цветов. Может быть null</param>
		public PsnChannelConfigViewModel(IPsnChannelConfig model, IColorsStorage colorsStorage) {
			Model = model;
			_colorsStorage = colorsStorage;

			var col = _colorsStorage?.GetColor(Model.Id);
			if (col != null)
				_color = col.Value;
		}

		public IPsnChannelConfig Model { get; }

		public Guid Id => Model.Id;

		public string Name => Model.Name;

		/// <summary>
		/// При установке запоминает цвет. 
		/// </summary>
		public Color Color {
			get => _color;
			set {
				if (_color == value)
					return;

				_color = value;
				_colorsStorage?.SetColor(Model.Id, _color);

				RaisePropertyChanged(() => Color);
			}
		}
	}
}