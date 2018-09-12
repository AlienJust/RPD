using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.ChartModifiers;
using Abt.Controls.SciChart.Model.DataSeries;
using Abt.Controls.SciChart.Visuals.Axes;
using Abt.Controls.SciChart.Visuals.PointMarkers;
using Abt.Controls.SciChart.Visuals.RenderableSeries;
using Dnv.Utils.Collections;
using Dnv.Utils.Messages;
using GalaSoft.MvvmLight.Messaging;
using Abt.Controls.SciChart.Numerics;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using NCalc;
using Newtonsoft.Json;
using NLog;
using RPD.SciChartControl.Converters;
using RPD.SciChartControl.Events;
using RPD.SciChartControl.Messages;
using RPD.SciChartControl.Settings;
using Binding = System.Windows.Data.Binding;
using Expression = NCalc.Expression;

namespace RPD.SciChartControl.ViewModel {
	internal class ChartControlViewModel : ViewModelBase, IChartControlViewModel {
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
		private readonly IMessenger _messenger;

		ChartSettingsPdo _chartSettingsPdo = new ChartSettingsPdo();

		private ObservableCollectionsLinker<IChartSeriesViewModel, IRpdChartSeriesViewModel> _analogSeriesLinker;
		private ObservableCollectionsLinker<IChartSeriesViewModel, IDiscreteChartSeriesViewModel> _discreteSeriesLinker;
		private int _lastAxisId = 0;
		private ChartDataObject _seriesData;

		public const string MainYAxisId = "mainYAxis";

		/// <summary>
		/// Признак того, что для только что добавленного дискретного графика нужно настроить видимый диапазон по оси Х.
		/// </summary>
		private bool _needToAdjustVisibleRangeForNewDiscreteSeries;
		private bool _needToAdjustVisibleRangeForNewAnalogSeries;

		#region Private Properties Fields

		private ObservableCollection<IChartSeriesViewModel> _analogSeries;
		private ObservableCollection<ISeriesAdditionalData> _analogSeriesAdditionalData;

		private ObservableCollection<IChartSeriesViewModel> _discreteSeries;
		private ObservableCollection<ISeriesAdditionalData> _discreteSeriesAdditionalData;

		private ObservableCollection<IRpdChartSeriesViewModel> _rpdAnalogSeriesViewModels;
		private ObservableCollection<IDiscreteChartSeriesViewModel> _rpdDiscreteSeriesViewModels;

		private DateRange _sharedXVisibleRange;
		private object _bindingTest;
		private ObservableCollection<ChartBookmarkViewModel> _bookmarks = new ObservableCollection<ChartBookmarkViewModel>();
		private bool _isMultipleYAxes;

		#endregion

		public ChartControlViewModel(IMessenger messenger) {
			_messenger = messenger;

			AnalogSeries = new ObservableCollection<IChartSeriesViewModel>();
			DiscreteSeries = new ObservableCollection<IChartSeriesViewModel>();

			IsAnnotationsVisible = true;
			IsToZoomXAxisOnly = false;
			IsAntialiasingEnabled = true;
			StrokeThickness = 1;
			SelectedResamplingMode = ResamplingMode.MinMax;

			CreateMainYAxis();

			InitializeCommands();

			//AddDefaultColors();
		}

		#region Public Properties

		public ChartDataObject SeriesData {
			get => _seriesData;
			set { Set(() => SeriesData, ref _seriesData, value); }
		}

		public RelayCommand ApplyMathFunCommand { get; set; }

		public RelayCommand TestCommand { get; set; }
		public RelayCommand AddBookmark { get; set; }
		public RelayCommand<ChartBookmarkViewModel> GoToBookmark { get; set; }
		public RelayCommand MoveRight { get; set; }
		public RelayCommand MoveLeft { get; set; }
		public RelayCommand MoveUp { get; set; }
		public RelayCommand MoveDown { get; set; }

		public RelayCommand GoToNextBookmark { get; set; }
		public RelayCommand GoToPreviousBookmark { get; set; }
		public RelayCommand DeleteCurrentBookmark { get; set; }
		public RelayCommand DeleteAllBookmarks { get; set; }

		public ObservableCollection<IChartModifier> ChartModifiers { get; } = new ObservableCollection<IChartModifier>();

		public ObservableCollection<ChartBookmarkViewModel> Bookmarks {
			get => _bookmarks;
			set { Set(() => Bookmarks, ref _bookmarks, value); }
		}

		public object BindingTest {
			get => _bindingTest;
			set { Set(() => BindingTest, ref _bindingTest, value); }
		}

		public ObservableCollection<IChartSeriesViewModel> AnalogSeries {
			get => _analogSeries;
			set {
				if (!SetValue(ref _analogSeries, value, SeriesOnCollectionChanged))
					return;

				RpdAnalogSeriesViewModels = null;
				_analogSeriesLinker?.Dispose();

				_analogSeriesLinker = null;
				if (_analogSeries == null)
					return;

				RpdAnalogSeriesViewModels = new ObservableCollection<IRpdChartSeriesViewModel>();
				foreach (var chartSeriesViewModel in _analogSeries)
					RpdAnalogSeriesViewModels.Add(new RpdChartSeriesViewModel(chartSeriesViewModel) { IsOnMainYAxis = true });

				_analogSeriesLinker = new
						ObservableCollectionsLinker<IChartSeriesViewModel, IRpdChartSeriesViewModel>(
						_analogSeries, _rpdAnalogSeriesViewModels,
						x => new RpdChartSeriesViewModel(x) { IsOnMainYAxis = true });

				RaisePropertyChanged(() => AnalogSeries);
			}
		}

		public ObservableCollection<ISeriesAdditionalData> AnalogSeriesAdditionalData {
			get => _analogSeriesAdditionalData;
			set {
				if (!SetValue(ref _analogSeriesAdditionalData, value, MetadataCollectionChanged))
					return;

				RaisePropertyChanged(() => AnalogSeriesAdditionalData);
			}
		}

		private void MetadataCollectionChanged(object sender, NotifyCollectionChangedEventArgs args) {
			;
		}

		public ObservableCollection<IChartSeriesViewModel> DiscreteSeries {
			get => _discreteSeries;
			set {
				if (!SetValue(ref _discreteSeries, value, SeriesOnCollectionChanged))
					return;

				RpdDiscreteSeriesViewModels = null;
				_discreteSeriesLinker?.Dispose();

				_discreteSeriesLinker = null;

				if (_discreteSeries == null)
					return;

				RpdDiscreteSeriesViewModels = new ObservableCollection<IDiscreteChartSeriesViewModel>();
				foreach (var chartSeriesViewModel in _discreteSeries)
					RpdDiscreteSeriesViewModels.Add(new DiscreteChartSeriesViewModel(chartSeriesViewModel));

				_discreteSeriesLinker = new
						ObservableCollectionsLinker<IChartSeriesViewModel, IDiscreteChartSeriesViewModel>(
						_discreteSeries, _rpdDiscreteSeriesViewModels, (x) => new DiscreteChartSeriesViewModel(x));

				RaisePropertyChanged(() => DiscreteSeries);
			}
		}

		public ObservableCollection<ISeriesAdditionalData> DiscreteSeriesAdditionalData {
			get => _discreteSeriesAdditionalData;
			set {
				if (!SetValue(ref _discreteSeriesAdditionalData, value, MetadataCollectionChanged))
					return;

				RaisePropertyChanged(() => DiscreteSeriesAdditionalData);
			}
		}

		public ObservableCollection<IRpdChartSeriesViewModel> RpdAnalogSeriesViewModels {
			get => _rpdAnalogSeriesViewModels;
			set { Set(() => RpdAnalogSeriesViewModels, ref _rpdAnalogSeriesViewModels, value); }
		}

		public ObservableCollection<IDiscreteChartSeriesViewModel> RpdDiscreteSeriesViewModels {
			get => _rpdDiscreteSeriesViewModels;
			set { Set(() => RpdDiscreteSeriesViewModels, ref _rpdDiscreteSeriesViewModels, value); }
		}

		public AxisCollection YAxes { get; } = new AxisCollection();

		public bool IsMultipleYAxes {
			get => _isMultipleYAxes;
			set { Set(() => IsMultipleYAxes, ref _isMultipleYAxes, value); }
		}

		public bool IsAnalogSeriesItemsCountZero => AnalogSeries.Count == 0;

		public bool IsDiscreteSeriesItemsCountZero => DiscreteSeries.Count == 0;

		public bool CombineYAxesZeroes {
			get => _combineYAxesZeroes;
			set {
				Set(() => CombineYAxesZeroes, ref _combineYAxesZeroes, value);

				if (_combineYAxesZeroes)
					CombineYAxesZero();
			}
		}


		public ObservableCollection<SolidColorBrush> ColorsBrush {
			get => DefaultColors.ColorBrushes;
			set {; }
		}


		public DateRange SharedXVisibleRange {
			get => _sharedXVisibleRange;
			set {
				if (_needToAdjustVisibleRangeForNewDiscreteSeries ||
						(DiscreteSeries.Count > 0 && _needToAdjustVisibleRangeForNewAnalogSeries)) {
					Set(() => SharedXVisibleRange, ref _sharedXVisibleRange, _sharedXVisibleRange);
					_needToAdjustVisibleRangeForNewDiscreteSeries = false;
					_needToAdjustVisibleRangeForNewAnalogSeries = false;
					return;
				}

				Set(() => SharedXVisibleRange, ref _sharedXVisibleRange, value);
			}
		}

		/// <summary>
		/// Выставляет атоматический диапазон видимой области, если график пустой. Т.е. при появалении на 
		/// графике первого сигнала область видимости будет автоматически растянута. При появлении послоедующих 
		/// сигналов обл. видимости не будет меняться.
		/// </summary>
		public AutoRange AutoRange {
			get {
				if (DiscreteSeries.Count == 1 && AnalogSeries.Count == 0)
					return AutoRange.Once;

				if (AnalogSeries.Count > 0)
					return AutoRange.Never;

				return AutoRange.Once;
			}

			// сет нужнен для того, чтобы работали биндинги
			set { }
		}

		public bool ZoomExtentsY {
			get => _chartSettingsPdo.ZoomExtentsY;
			set { Set(() => ZoomExtentsY, ref _chartSettingsPdo.ZoomExtentsY, value); }
		}


		public bool IsAnnotationsVisible {
			get => _chartSettingsPdo.IsAnnotationsVisible;
			set { Set(() => IsAnnotationsVisible, ref _chartSettingsPdo.IsAnnotationsVisible, value); }
		}

		public bool IsToZoomXAxisOnly {
			get => _chartSettingsPdo.IsToZoomXAxisOnly;
			set { Set(() => IsToZoomXAxisOnly, ref _chartSettingsPdo.IsToZoomXAxisOnly, value); }
		}

		public string SelectedTheme {
			get => _chartSettingsPdo.SelectedTheme;
			set { Set(() => SelectedTheme, ref _chartSettingsPdo.SelectedTheme, value); }
		}

		public bool IsMarkersVisible {
			get => _chartSettingsPdo.IsMarkersVisible;
			set {
				Set(() => IsMarkersVisible, ref _chartSettingsPdo.IsMarkersVisible, value);
				SetDiscreteAndAnalogSeriesMarkers(value);
			}
		}

		public bool IsAntialiasingEnabled {
			get => _chartSettingsPdo.IsAntialiasingEnabled;
			set {
				Set(() => IsAntialiasingEnabled, ref _chartSettingsPdo.IsAntialiasingEnabled, value);

				foreach (var chartSeriesViewModel in AnalogSeries)
					chartSeriesViewModel.RenderSeries.AntiAliasing = value;
			}
		}

		public int StrokeThickness {
			get => _chartSettingsPdo.StrokeThickness;
			set {
				Set(() => StrokeThickness, ref _chartSettingsPdo.StrokeThickness, value);

				foreach (var chartSeriesViewModel in AnalogSeries)
					chartSeriesViewModel.RenderSeries.StrokeThickness = value;
			}
		}

		public ResamplingMode SelectedResamplingMode {
			get => _chartSettingsPdo.SelectedResamplingMode;
			set {
				Set(() => SelectedResamplingMode, ref _chartSettingsPdo.SelectedResamplingMode, value);
				foreach (var chartSeriesViewModel in AnalogSeries)
					chartSeriesViewModel.RenderSeries.ResamplingMode = value;
			}
		}

		public IEnumerable<string> AllThemes => ThemeManager.AllThemes;

		public IEnumerable<int> StrokeThicknesses => new[] { 1, 2, 3, 4, 5 };

		#endregion // Public Properties



		#region Commands

		public RelayCommand<MoveSignalToYAxisCommandParameter> MoveSignalToYAxisCommand { get; set; }

		private void MoveSignalToYAxisCommandExcute(MoveSignalToYAxisCommandParameter param) {
			param.SeriesVm.ChartSeries.RenderSeries.YAxisId = param.Axis.Id;

			if (param.Axis.Id != MainYAxisId)
				param.SeriesVm.IsOnMainYAxis = false;
			else
				param.SeriesVm.IsOnMainYAxis = true;
		}

		public RelayCommand<IRpdChartSeriesViewModel> MoveToOwnYAxis { get; set; }

		private void MoveToOwnAxisExecute(IRpdChartSeriesViewModel seriesVm) {
			if (seriesVm.ChartSeries.RenderSeries.YAxisId != MainYAxisId)
				return;

			AddNewNumericYAxis(seriesVm, "axis" + _lastAxisId);

			_lastAxisId++;
		}

		private void AddNewNumericYAxis(IRpdChartSeriesViewModel seriesVm, string axisId) {
			var yaxis = new NumericAxis
			{
				Id = axisId,
				AxisTitle = seriesVm.ChartSeries.DataSeries.SeriesName,
				AutoRange = AutoRange.Once,
				AxisAlignment = AxisAlignment.Left,
				BorderThickness = new Thickness(0, 0, 1, 0)
			};
			AdjustYAxis(yaxis);

			BindAxisColorToSeriesColor(seriesVm.ChartSeries, yaxis);

			YAxes.Add(yaxis);

			seriesVm.ChartSeries.RenderSeries.YAxisId = yaxis.Id;
			seriesVm.IsOnMainYAxis = false;
		}

		private void CreateMainYAxis() {
			YAxes.CollectionChanged +=
					(sender, args) => {
						IsMultipleYAxes = YAxes.Count > 1;
					};

			var main = new NumericAxis
			{
				Id = MainYAxisId,
				AxisTitle = "Основная ось Y",
				IsPrimaryAxis = true,
				AutoRange = AutoRange.Once,
				AxisAlignment = AxisAlignment.Left,
				Width = 50
			};

			AdjustYAxis(main);

			YAxes.Add(main);
		}

		private void AdjustYAxis(NumericAxis axis) {
			axis.VisibleRangeChanged += OnMainYAxisVisibleRangeChanged;
			axis.MouseDown += (sender, args) => {
				_mouseDownAxis = sender;
			};
		}

		/// <summary>
		/// Ось Y на которой была нажата мышь
		/// </summary>
		private object _mouseDownAxis;
		private bool _combineYAxesZeroes;

		private void OnMainYAxisVisibleRangeChanged(object sender, VisibleRangeChangedEventArgs args) {
			if (!CombineYAxesZeroes)
				return;

			if (sender != _mouseDownAxis)
				return;

			var axis = (AxisBase)sender;
			var axisRange = (DoubleRange)axis.VisibleRange;

			foreach (var yAx in YAxes) {
				if (yAx != sender) {
					var range = (DoubleRange)yAx.VisibleRange;
					var dt = range.Min - axisRange.Min * ((range.Max - range.Min) / (axisRange.Max - axisRange.Min));
					Logger.Info(yAx.Id + ", dt=" + dt);
					range.SetMinMax(range.Min - dt, range.Max - dt);
				}
			}
		}

		private void CombineYAxesZero() {
			if (YAxes.Count == 0)
				return;

			_mouseDownAxis = YAxes[0];

			OnMainYAxisVisibleRangeChanged(_mouseDownAxis, null);
		}


		private static void BindAxisColorToSeriesColor(IChartSeriesViewModel series, NumericAxis axis) {
			axis.SetBinding(AxisBase.TickTextBrushProperty, CreateSeriesColorBinding(series));
			axis.SetBinding(AxisBase.BorderBrushProperty, CreateSeriesColorBinding(series));
		}

		private static Binding CreateSeriesColorBinding(IChartSeriesViewModel series) {
			return new Binding
			{
				Source = series.RenderSeries,
				Path = new PropertyPath("SeriesColor"),
				Converter = new ColorToBrushConverter()
			};
		}


		public RelayCommand<IRpdChartSeriesViewModel> MoveToMainYAxis { get; set; }

		private void MoveToMainYAxisExecute(IRpdChartSeriesViewModel seriesVm) {
			if (seriesVm.ChartSeries.RenderSeries.YAxisId == MainYAxisId)
				return;

			RemoveSeriesRelatedYAxisIfAxisEmpty(seriesVm.ChartSeries);

			// помещаем на основную Ось ординат
			seriesVm.ChartSeries.RenderSeries.YAxisId = MainYAxisId;
			seriesVm.IsOnMainYAxis = true;
		}


		public RelayCommand<IRpdChartSeriesViewModel> ShowSelectColorWindowCommand { get; set; }

		void ShowSelectColorWindowCommandExecute(IRpdChartSeriesViewModel series) {
			var msg = new DialogMessage<ColorDialog>(this, new ColorDialog(),
					dialog => {
						if (dialog.DialogResult != DialogResult.OK)
							return;

						series.ChartSeries.RenderSeries.SeriesColor = new Color
						{
							R = dialog.Dialog.Color.R,
							G = dialog.Dialog.Color.G,
							B = dialog.Dialog.Color.B,
							A = dialog.Dialog.Color.A
						};
					});

			_messenger.Send(msg);
		}

		/// <summary>
		/// Возникает при смене цвета серии на графике.
		/// </summary>
		public event SeriesColorChangedEventHandler OnSeriesColorChanged;

		private void RaiseOnSeriesColorChanged(IChartSeriesViewModel series, Color color) {
			var e = OnSeriesColorChanged;
			e?.Invoke(new SeriesColorChangedEventArgs(series, color));
		}

		public RelayCommand<SetSeriesColorCommandParameter> SeriesColorCommand { get; set; }

		private void SeriesColorExecute(SetSeriesColorCommandParameter param) {
			param.ViewModel.ChartSeries.RenderSeries.SeriesColor = param.Color;
			RaiseOnSeriesColorChanged(param.ViewModel.ChartSeries, param.Color);
		}

		public RelayCommand ShowHelpWindowCommand { get; set; }

		private void ShowHelpWindowCommandExecute() {
			_messenger.Send(new WindowMessages(), WindowMessages.ShowHelpWindow);
		}


		public RelayCommand<IRpdChartSeriesViewModel> ChartCloseCommand { get; set; }

		private void ChartCloseCommandExecute(IRpdChartSeriesViewModel seriesViewModel) {
			var ser = AnalogSeries.SingleOrDefault(x => x.DataSeries == seriesViewModel.ChartSeries.DataSeries);
			if (ser != null)
				AnalogSeries.Remove(ser);
			else {
				ser = DiscreteSeries.Single(x => x.DataSeries == seriesViewModel.ChartSeries.DataSeries);
				DiscreteSeries.Remove(ser);
			}
		}

		#endregion // Commands


		#region Private Methods

		private IRpdChartSeriesViewModel FindAnalogRpdChartSeriesBySeriesInfo(SeriesInfo seriesInfo) {
			return RpdAnalogSeriesViewModels.SingleOrDefault(x => x.ChartSeries.RenderSeries == seriesInfo.RenderableSeries);
		}

		/// <summary>
		/// Ищет сначала аналоговый, а затем дискретный сигнал.
		/// </summary>
		/// <param name="predicate">Предикат</param>
		/// <returns>Сигнал или null</returns>
		private IChartSeriesViewModel FindAnalogOrDiscreteSeries(Func<IChartSeriesViewModel, bool> predicate) {
			return AnalogSeries.SingleOrDefault(predicate) ?? DiscreteSeries.Single(predicate);
		}

		private void InitializeCommands() {
			ChartCloseCommand = new RelayCommand<IRpdChartSeriesViewModel>(ChartCloseCommandExecute);
			SeriesColorCommand = new RelayCommand<SetSeriesColorCommandParameter>(SeriesColorExecute);

			ShowHelpWindowCommand = new RelayCommand(ShowHelpWindowCommandExecute);
			ShowSelectColorWindowCommand = new RelayCommand<IRpdChartSeriesViewModel>(ShowSelectColorWindowCommandExecute);

			MoveToOwnYAxis = new RelayCommand<IRpdChartSeriesViewModel>(MoveToOwnAxisExecute);
			MoveToMainYAxis = new RelayCommand<IRpdChartSeriesViewModel>(MoveToMainYAxisExecute);
			TestCommand = new RelayCommand(TestCommandExecute);
			ApplyMathFunCommand = new RelayCommand(ApplyMathFunCommandExecute);

			AddBookmark = new RelayCommand(AddBookmarkExecute);

			GoToBookmark = new RelayCommand<ChartBookmarkViewModel>(GoToBookmarkExecute);

			GoToNextBookmark = new RelayCommand(GoToNextBookmarkExecute);
			GoToPreviousBookmark = new RelayCommand(GoToPreviousBookmarkExecute);
			DeleteCurrentBookmark = new RelayCommand(DeleteCurrentBookmarkExecute);
			DeleteAllBookmarks = new RelayCommand(DeleteAllBookmarksExecute);

			MoveRight = new RelayCommand(MoveRightExecute);
			MoveLeft = new RelayCommand(MoveLeftExecute);
			MoveUp = new RelayCommand(MoveUpExecute);
			MoveDown = new RelayCommand(MoveDownExecute);

			MoveSignalToYAxisCommand = new RelayCommand<MoveSignalToYAxisCommandParameter>(MoveSignalToYAxisCommandExcute);
		}



		private void MoveDownExecute() {

		}

		private void MoveUpExecute() {

		}

		private void MoveLeftExecute() {
			var dt = (SharedXVisibleRange.Max - SharedXVisibleRange.Min).Ticks / 15;

			SharedXVisibleRange = new DateRange(SharedXVisibleRange.Min - TimeSpan.FromTicks(dt),
					SharedXVisibleRange.Max - TimeSpan.FromTicks(dt));
		}

		private void MoveRightExecute() {
			var dt = (SharedXVisibleRange.Max - SharedXVisibleRange.Min).Ticks / 15;

			SharedXVisibleRange = new DateRange(SharedXVisibleRange.Min + TimeSpan.FromTicks(dt),
					SharedXVisibleRange.Max + TimeSpan.FromTicks(dt));

		}

		private void DeleteAllBookmarksExecute() {
			Bookmarks.Clear();
		}

		private void DeleteCurrentBookmarkExecute() {
			var current = Bookmarks.FirstOrDefault(e => e.IsCurrent);
			if (current == null)
				return;

			var index = Bookmarks.IndexOf(current);
			Bookmarks.Remove(current);

			index--;
			if (index < 0)
				index = 0;

			if (Bookmarks.Count > 0)
				GoToBookmarkExecute(Bookmarks.ElementAt(index));
		}

		private void GoToPreviousBookmarkExecute() {
			var current = Bookmarks.FirstOrDefault(e => e.IsCurrent);
			if (current == null)
				return;

			var index = Bookmarks.IndexOf(current);
			if (index == 0)
				return;

			GoToBookmarkExecute(Bookmarks.ElementAt(index - 1));
		}

		private void GoToNextBookmarkExecute() {
			var current = Bookmarks.FirstOrDefault(e => e.IsCurrent);
			if (current == null)
				return;

			var index = Bookmarks.IndexOf(current);
			if (index == Bookmarks.Count - 1)
				return;

			GoToBookmarkExecute(Bookmarks.ElementAt(index + 1));
		}

		private void GoToBookmarkExecute(ChartBookmarkViewModel bookmark) {
			RemoveCurrentBookmarkFlag();
			bookmark.IsCurrent = true;

			SharedXVisibleRange = bookmark.XVisibleRange;
		}

		private void RemoveCurrentBookmarkFlag() {
			foreach (var chartBookmarkViewModel in Bookmarks) {
				if (chartBookmarkViewModel.IsCurrent)
					chartBookmarkViewModel.IsCurrent = false;
			}
		}

		private void AddBookmarkExecute() {
			_messenger.Send(new ViewMessageWithCallback<ChartBookmarkViewModel>(ViewAction.ShowModal,
					bookmark => {
						RemoveCurrentBookmarkFlag();

						bookmark.XVisibleRange = SharedXVisibleRange;
						Bookmarks.Add(bookmark);

						bookmark.IsCurrent = true;

					}), WindowMessages.BookmarkNameWindow);
		}

		private void ApplyMathFunCommandExecute() {
			ApplyMath(RpdAnalogSeriesViewModels);

			ApplyMath(RpdDiscreteSeriesViewModels);
		}

		private void ApplyMath<T>(ObservableCollection<T> rpdAnalogSeriesViewModels)
				where T : IRpdChartSeriesViewModel {
			foreach (var seriesViewModel in rpdAnalogSeriesViewModels) {
				if (!seriesViewModel.IsMathFuncChanged)
					continue;

				seriesViewModel.CloneDataSeriesIfNull();

				var series = (XyDataSeries<DateTime, double>)seriesViewModel.ChartSeries.DataSeries;

				var expr = new Expression(seriesViewModel.MathFunc, EvaluateOptions.IgnoreCase);
				using (series.SuspendUpdates()) {
					int count = series.Count;

					if (seriesViewModel is IDiscreteChartSeriesViewModel) {
						for (int i = 0; i < count; i++) {
							expr.Parameters["x"] = seriesViewModel.OridinalDataSeries.YValues[i];

							var res = expr.Evaluate();
							if (res is int i1)
								series.Update(series.XValues[i], i1);
							else if (res is bool b)
							{
								series.Update(series.XValues[i], b ? 1 : 0);
							}
							else
								series.Update(series.XValues[i], (double)expr.Evaluate());
						}
					}
					else {
						for (int i = 0; i < count; i++) {
							expr.Parameters["x"] = seriesViewModel.OridinalDataSeries.YValues[i];
							series.Update(series.XValues[i], (double)expr.Evaluate());
						}

					}
				}

				seriesViewModel.IsMathFuncChanged = true;
			}
		}



		private void TestCommandExecute() {
			var diff = (SharedXVisibleRange.Max - SharedXVisibleRange.Min).Ticks / 5;
			var min = SharedXVisibleRange.Min + TimeSpan.FromTicks(diff);
			var max = SharedXVisibleRange.Max + TimeSpan.FromTicks(diff);

			SharedXVisibleRange = new DateRange(min, max);
		}


		bool SetValue<T>(ref ObservableCollection<T> field,
				ObservableCollection<T> value,
				NotifyCollectionChangedEventHandler collectionChanged) {
			if (field == value)
				return false;

			// отписываем предыдущее значение поля от обработчика
			if (field != null)
				field.CollectionChanged -= collectionChanged;

			field = value;
			if (field != null)
				field.CollectionChanged += collectionChanged;

			return true;
		}

		private void SeriesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args) {
			if (args.Action == NotifyCollectionChangedAction.Add) {
				AdjustNewSeriesProperties(sender.Equals(DiscreteSeries), args);

				if (args.NewItems.Count > 0) {
					if (sender.Equals(DiscreteSeries))
						_needToAdjustVisibleRangeForNewDiscreteSeries = true;
					else
						_needToAdjustVisibleRangeForNewAnalogSeries = true;
				}

				RaisePropertyChanged(() => SharedXVisibleRange);
			}
			else if (args.Action == NotifyCollectionChangedAction.Remove) {
				foreach (var ser in args.OldItems) {
					var seriesVm = (IChartSeriesViewModel)ser;
					if (seriesVm.RenderSeries.YAxisId != MainYAxisId)
						RemoveSeriesRelatedYAxisIfAxisEmpty(seriesVm);
				}
			}

			RaisePropertyChanged(() => AutoRange);

			if (sender.Equals(DiscreteSeries))
				RaisePropertyChanged(() => IsDiscreteSeriesItemsCountZero);
			else
				RaisePropertyChanged(() => IsAnalogSeriesItemsCountZero);
		}

		private void RemoveSeriesRelatedYAxisIfAxisEmpty(IChartSeriesViewModel seriesVm) {
			if (IsYAxisNotEmpty(seriesVm.RenderSeries.YAxisId))
				return;

			var axis = YAxes.FirstOrDefault(y => y.Id == seriesVm.RenderSeries.YAxisId);
			YAxes.Remove(axis);
		}

		/// <summary>
		/// На заданной оси Y находятся серии.
		/// </summary>
		/// <param name="axisId">Id оси Y</param>
		private bool IsYAxisNotEmpty(string axisId) {
			return AnalogSeries.Count(e => e.RenderSeries.YAxisId == axisId) > 0;
		}

		/// <summary>
		/// Устанавливает настройки отображения серий такие же как на графике.
		/// </summary>
		/// <param name="isDiscreteSeries"> </param>
		/// <param name="args">Новые серии.</param>
		private void AdjustNewSeriesProperties(bool isDiscreteSeries, NotifyCollectionChangedEventArgs args) {
			foreach (var ser in args.NewItems) {
				var seriesVm = (IChartSeriesViewModel)ser;
				seriesVm.RenderSeries.AntiAliasing = IsAntialiasingEnabled;

				if (!isDiscreteSeries)
					seriesVm.RenderSeries.YAxisId = MainYAxisId;

				SetSeriesMarkers((BaseRenderableSeries)seriesVm.RenderSeries, IsMarkersVisible);
			}
		}

		private void SetSeriesMarkers(BaseRenderableSeries series, bool isMarkersVisible) {
			if (isMarkersVisible) {
				series.PointMarker = new EllipsePointMarker {
					Width = 7,
					Height = 7,
					Fill = series.SeriesColor
				};
			}
			else
				series.PointMarker = null;
		}

		private void SetDiscreteAndAnalogSeriesMarkers(bool value) {
			foreach (var chartSeriesViewModel in AnalogSeries)
				SetSeriesMarkers((BaseRenderableSeries)chartSeriesViewModel.RenderSeries, value);

			foreach (var chartSeriesViewModel in DiscreteSeries)
				SetSeriesMarkers((BaseRenderableSeries)chartSeriesViewModel.RenderSeries, value);
		}

		#endregion // Private Methods


		#region Public Methods

		public IRpdChartSeriesViewModel FindRpdChartSeriesBySeriesInfo(SeriesInfo seriesInfo) {
			return RpdAnalogSeriesViewModels.SingleOrDefault(x => x.ChartSeries.RenderSeries == seriesInfo.RenderableSeries) ??
					RpdDiscreteSeriesViewModels.SingleOrDefault(x => x.ChartSeries.RenderSeries == seriesInfo.RenderableSeries);
		}

		public void SaveSettings(string fileName) {
			var outputJson = JsonConvert.SerializeObject(_chartSettingsPdo, Formatting.Indented);
			File.WriteAllText(fileName, outputJson);
		}

		public void SaveState(string fileName) {
			var state = new ChartStatePdo {
				XVisibleRangeMin = SharedXVisibleRange.Min,
				XVisibleRangeMax = SharedXVisibleRange.Max
			};

			foreach (var axes in YAxes) {
				state.AxesState.Add(new AxisSettingsPdo {
					Id = axes.Id,
					AxisTitle = axes.AxisTitle,
					YVisibleRangeMin = axes.VisibleRange.AsDoubleRange().Min,
					YVisibleRangeMax = axes.VisibleRange.AsDoubleRange().Max
				});
			}

			var outputJson = JsonConvert.SerializeObject(state, Formatting.Indented);
			File.WriteAllText(fileName, outputJson);
		}

		public void LoadState(string fileName) {
			if (!File.Exists(fileName))
				return;

			var json = File.ReadAllText(fileName);
			var state = JsonConvert.DeserializeObject<ChartStatePdo>(json);

			foreach (var axis in state.AxesState) {
				var series = RpdAnalogSeriesViewModels.FirstOrDefault(e => e.ChartSeries.RenderSeries.YAxisId == axis.Id);
				if (series != null) {
					AddNewNumericYAxis(series, axis.Id);
				}
			}

			SharedXVisibleRange = new DateRange(state.XVisibleRangeMin, state.XVisibleRangeMax);
		}

		public void LoadSetings(string fileName) {
			if (!File.Exists(fileName))
				return;

			var json = File.ReadAllText(fileName);
			_chartSettingsPdo = JsonConvert.DeserializeObject<ChartSettingsPdo>(json);
			RaisePropertyChanged("");
		}

		#endregion // Public Methods
	}
}
