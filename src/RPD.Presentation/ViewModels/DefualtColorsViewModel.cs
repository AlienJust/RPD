using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media;
using Dnv.Utils.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RPD.DAL;
using RPD.Presentation.Contracts.ViewModels;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;
using RPD.Presentation.Messages;
using RPD.Presentation.Settings;
using RPD.Presentation.Utils;
using RPD.Presentation.Utils.Messages;
using RPD.Presentation.ViewModels.DalViewModels;

namespace RPD.Presentation.ViewModels {
	// ReSharper disable once ClassNeverInstantiated.Global
	internal class DefualtColorsViewModel : ViewModelBase, IDeafultColorsViewModel {
		private readonly IMessenger _messenger;
		private readonly IColorsStorage _colorsStorage;

		private IPsnConfigurationViewModel _selectedConfiguration;

		private IPsnMeterConfigViewModel _selectedPsnMeterConfig;
		private IPsnChannelConfigViewModel _selectedChannelConfig;

		private static int _lastColorIndex = 0;

		public DefualtColorsViewModel(ILoader loader, IMessenger messenger, IColorsStorage colorsStorage) {
			_messenger = messenger;
			_colorsStorage = colorsStorage;

			Configurations = new ObservableCollection<IPsnConfigurationViewModel>();

			InitializeDefaultColors();
			FillAvailablePsnConfigurations(loader);

			Close = new RelayCommand(CloseExecute);
			ShowSelectColorDialog = new RelayCommand(ShowSelectColorDialogExecute);
			AddSelectedColorToPalette = new RelayCommand(AddSelectedColorToPaletteExecute);
		}

		/// <summary>
		/// Генерирует цвета по умолчанию для всех сигналов, после чего отображает сообщение 
		/// пользователю и запоминает это. При следующем запуске, генерации и сообщения не будет.
		/// </summary>
		public static void GenerateDefaultColorsIfNeed(IApplicationSettings settings, ILoader loader, IColorsStorage colorsStorage) {
			if (settings.IsDefaultColorsGenerated)
				return;

			foreach (var config in loader.AvailablePsnConfigruations) {
				foreach (var psnMeterConfig in config.PsnMeterConfigs) {
					foreach (var channel in psnMeterConfig.PsnChannelConfigs) {
						if (!colorsStorage.GetColor(channel.Id).HasValue)
							colorsStorage.SetColor(channel.Id, GetColorForNewLine());
					}
				}
			}

			settings.IsDefaultColorsGenerated = true;
			colorsStorage.Save();
			settings.Save();

			MessageBox.Show(null, "Для всех каналов ПСН сгенерированы цвета отображения на графике. Чтобы изменить настройки цветов по умолчанию воспользуйтесь пунктом главного меню \"Настройки\" -> \"Настройки цветов по умолчанию...\".",
					"РПД", MessageBoxButtons.OK, MessageBoxIcon.None);
		}

		private void AddSelectedColorToPaletteExecute() {
			if (SelectedColor != Colors.White) {
				var col = ColorBrushes.FirstOrDefault(e => e.Color == SelectedColor);
				if (col == null) {
					var prev = SelectedColor;
					ColorBrushes.Add(new SolidColorBrush(SelectedColor));
					SelectedColor = prev;
				}
			}
		}

		private void FillAvailablePsnConfigurations(ILoader loader) {
			foreach (var config in loader.AvailablePsnConfigruations)
				Configurations.Add(new PsnConfigurationViewModel(config, _colorsStorage));

		}

		private void InitializeDefaultColors() {
			ColorBrushes = new ObservableCollection<SolidColorBrush>();

			foreach (var solidColorBrush in SciChartControl.DefaultColors.ColorBrushes)
				ColorBrushes.Add(solidColorBrush);
		}

		private void ShowSelectColorDialogExecute() {
			var msg = new DialogMessage<ColorDialog>(this, new ColorDialog(),
					resultCallback: dialog => {
						if (dialog.DialogResult != DialogResult.OK)
							return;

						SelectedColor = Color.FromArgb(dialog.Dialog.Color.A,
											dialog.Dialog.Color.R,
											dialog.Dialog.Color.G,
											dialog.Dialog.Color.B);

					});

			_messenger.Send(msg);
		}

		private void CloseExecute() {
			_colorsStorage.Save();
			_messenger.Send(new ViewMessage(ViewAction.Close), Views.Views.DefaultColorSettings);
		}

		public ObservableCollection<IPsnConfigurationViewModel> Configurations { get; }

		public IPsnConfigurationViewModel SelectedConfiguration {
			get => _selectedConfiguration;
			set { Set(() => SelectedConfiguration, ref _selectedConfiguration, value); }
		}

		public IPsnMeterConfigViewModel SelectedPsnMeterConfig {
			get => _selectedPsnMeterConfig;
			set {
				if (_selectedPsnMeterConfig == value)
					return;

				_selectedPsnMeterConfig = value;
				RaisePropertyChanged(() => SelectedPsnMeterConfig);
			}
		}

		private static Color GetColorForNewLine() {
			if (_lastColorIndex >= SciChartControl.DefaultColors.ColorBrushes.Count)
				_lastColorIndex = 0;

			return SciChartControl.DefaultColors.ColorBrushes[_lastColorIndex++].Color;
		}

		public IPsnChannelConfigViewModel SelectedChannelConfig {
			get => _selectedChannelConfig;
			set { Set(() => SelectedChannelConfig, ref _selectedChannelConfig, value); }
		}

		public ObservableCollection<SolidColorBrush> ColorBrushes { get; set; }

		public Color SelectedColor {
			get {
				if (SelectedChannelConfig == null)
					return Colors.White;

				return SelectedChannelConfig.Color;
			}
			set {
				if (SelectedChannelConfig == null)
					return;

				if (SelectedChannelConfig.Color == value)
					return;

				SelectedChannelConfig.Color = value;
				RaisePropertyChanged(() => SelectedColor);
			}
		}

		public RelayCommand Close { get; set; }
		public RelayCommand ShowSelectColorDialog { get; set; }
		public RelayCommand AddSelectedColorToPalette { get; set; }
	}
}