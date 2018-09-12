using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.Visuals.Annotations;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Dnv.Utils.Messages;
using RPD.SciChartControl.Events;
using RPD.SciChartControl.Messages;
using RPD.SciChartControl.ViewModel;
using Control = System.Windows.Controls.Control;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using Point = System.Windows.Point;
using PrintDialog = System.Windows.Controls.PrintDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using UserControl = System.Windows.Controls.UserControl;

namespace RPD.SciChartControl.Views {
	/// <summary>
	/// График. Для добавления данных на график используются AnalogSeries и DiscreteSeries. Поддерживаются биндинги.
	/// </summary>
	public partial class ChartControl : UserControl {
		#region Dependency Properties


		#region SeriesColorChanged

		/// <summary>
		/// Событие возникает когда юзер меняет цвет серии.
		/// </summary>
		public event RoutedEventHandler SeriesColorChanged {
			add { AddHandler(SeriesColorChangedEvent, value); }
			remove { RemoveHandler(SeriesColorChangedEvent, value); }
		}


		private void RaiseSeriesColorChanged(SeriesColorChangedEventArgs e) {
			e.RoutedEvent = SeriesColorChangedEvent;
			RaiseEvent(e);
		}

		public static readonly RoutedEvent SeriesColorChangedEvent =
				EventManager.RegisterRoutedEvent("SeriesColorChanged",
				RoutingStrategy.Bubble,
				typeof(RoutedEventHandler),
				typeof(ChartControl));


		#endregion


		#region DiscreteSeries

		public const string DiscreteSeriesPropertyName = "DiscreteSeries";

		/// <summary>
		/// Коллекция дискретных серий, которые нужно отображать на графике.
		/// </summary>
		public ObservableCollection<IChartSeriesViewModel> DiscreteSeries {
			get { return (ObservableCollection<IChartSeriesViewModel>)GetValue(DiscreteSeriesProperty); }
			set { SetValue(DiscreteSeriesProperty, value); }
		}

		public static readonly DependencyProperty DiscreteSeriesProperty = DependencyProperty.Register(
				DiscreteSeriesPropertyName,
				typeof(ObservableCollection<IChartSeriesViewModel>),
				typeof(ChartControl),
				new FrameworkPropertyMetadata(null, DiscreteSeriesPropertyChangedCallback));

		private static void DiscreteSeriesPropertyChangedCallback(DependencyObject dependencyObject,
				DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs) {
			var obj = (ChartControl)dependencyObject;

			// Присваиваем коллекцию серий в модель представления, чтобы работали биндинги в XAML.
			obj.ViewModel.DiscreteSeries =
					(ObservableCollection<IChartSeriesViewModel>)dependencyPropertyChangedEventArgs.NewValue;
		}


		#endregion


		#region DiscreteSeriesAdditionalData

		public const string DiscreteSeriesAdditionalDataPropertyName = "DiscreteSeriesAdditionalData";

		/// <summary>
		/// Коллекция дополнительных данных связанных с серией. Данные должны добавляться вручную 
		/// одновременно с добавлением данных в DiscreteSeries).
		/// </summary>
		public ObservableCollection<ISeriesAdditionalData> DiscreteSeriesAdditionalData {
			get { return (ObservableCollection<ISeriesAdditionalData>)GetValue(DiscreteSeriesAdditionalDataProperty); }
			set { SetValue(DiscreteSeriesAdditionalDataProperty, value); }
		}

		public static readonly DependencyProperty DiscreteSeriesAdditionalDataProperty = DependencyProperty.Register(
				DiscreteSeriesAdditionalDataPropertyName,
				typeof(ObservableCollection<ISeriesAdditionalData>),
				typeof(ChartControl),
				new FrameworkPropertyMetadata(null, DiscreteSeriesAdditionalDataPropertyChangedCallback));

		private static void DiscreteSeriesAdditionalDataPropertyChangedCallback(DependencyObject dependencyObject,
				DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs) {
			var obj = (ChartControl)dependencyObject;

			// Присваиваем коллекцию серий в модель представления, чтобы работали биндинги в XAML.
			obj.ViewModel.DiscreteSeriesAdditionalData =
					(ObservableCollection<ISeriesAdditionalData>)dependencyPropertyChangedEventArgs.NewValue;
		}

		#endregion


		#region AnalogSeries

		private const string SeriesPropertyName = "AnalogSeries";

		/// <summary>
		/// Коллекция аналоговых серий, которые нужно отображать на графике.
		/// </summary>
		public ObservableCollection<IChartSeriesViewModel> AnalogSeries {
			get { return (ObservableCollection<IChartSeriesViewModel>)GetValue(AnalogSeriesProperty); }
			set { SetValue(AnalogSeriesProperty, value); }
		}

		public static readonly DependencyProperty AnalogSeriesProperty = DependencyProperty.Register(
				SeriesPropertyName,
				typeof(ObservableCollection<IChartSeriesViewModel>),
				typeof(ChartControl),
				new FrameworkPropertyMetadata(null, AnalogSeriesPropertyChangedCallback));


		private static void AnalogSeriesPropertyChangedCallback(DependencyObject dependencyObject,
				DependencyPropertyChangedEventArgs args) {
			var thisObject = (ChartControl)dependencyObject;
			var newValue = (ObservableCollection<IChartSeriesViewModel>)args.NewValue;

			// Присваиваем коллекцию серий в модель представления, чтобы работали биндинги в XAML.
			thisObject.ViewModel.AnalogSeries = newValue;
		}

		#endregion


		#region AnalogSeriesAdditionalData

		private const string AnalogSeriesAdditionalDataPropertyName = "AnalogSeriesAdditionalData";

		/// <summary>
		/// Коллекция дополнительных данных связанных с серией. Данные должны добавляться вручную 
		/// одновременно с добавлением данных в AnalogSeries.
		/// </summary>
		public ObservableCollection<ISeriesAdditionalData> AnalogSeriesAdditionalData {
			get { return (ObservableCollection<ISeriesAdditionalData>)GetValue(AnalogSeriesAdditionalDataProperty); }
			set { SetValue(AnalogSeriesAdditionalDataProperty, value); }
		}

		public static readonly DependencyProperty AnalogSeriesAdditionalDataProperty = DependencyProperty.Register(
				AnalogSeriesAdditionalDataPropertyName,
				typeof(ObservableCollection<ISeriesAdditionalData>),
				typeof(ChartControl),
				new FrameworkPropertyMetadata(null, AnalogSeriesAdditionalDataPropertyChangedCallback));


		private static void AnalogSeriesAdditionalDataPropertyChangedCallback(DependencyObject dependencyObject,
				DependencyPropertyChangedEventArgs args) {
			var thisObject = (ChartControl)dependencyObject;
			var newValue = (ObservableCollection<ISeriesAdditionalData>)args.NewValue;

			// Присваиваем коллекцию серий в модель представления, чтобы работали биндинги в XAML.
			thisObject.ViewModel.AnalogSeriesAdditionalData = newValue;
		}

		#endregion



		#region ViewModel

		/// <summary>
		/// Модель представления.
		/// </summary>

		private const string ViewModelPropertyName = "ViewModel";

		internal IChartControlViewModel ViewModel {
			get => (IChartControlViewModel)GetValue(ViewModelProperty);
			set => SetValue(ViewModelProperty, value);
		}

		internal static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
				ViewModelPropertyName,
				typeof(IChartControlViewModel),
				typeof(ChartControl),
				new FrameworkPropertyMetadata());

		#endregion



		#region Annotations 


		public const string AnnotationsPropertyName = "Annotations";

		/// <summary>
		/// Аннотации.
		/// </summary>
		public ObservableCollection<IAnnotation> Annotations {
			get => (ObservableCollection<IAnnotation>)GetValue(AnnotationsProperty);
			set => SetValue(AnnotationsProperty, value);
		}


		public static readonly DependencyProperty AnnotationsProperty = DependencyProperty.Register(
				AnnotationsPropertyName,
				typeof(ObservableCollection<IAnnotation>),
				typeof(ChartControl),
				new UIPropertyMetadata(null, AnnotationsPropertyChangedCallback));

		private static void AnnotationsPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args) {
			var thisObject = (ChartControl)dependencyObject;

			// отписываемся от событий для предыдущего значения
			var oldValue = (ObservableCollection<IAnnotation>)args.OldValue;
			if (oldValue != null)
				oldValue.CollectionChanged -= thisObject.AnnotationsOnCollectionChanged;

			var newValue = (ObservableCollection<IAnnotation>)args.NewValue;
			if (newValue == null)
				return;

			newValue.CollectionChanged += thisObject.AnnotationsOnCollectionChanged;
			thisObject.AddAnnotations(newValue);
		}

		#endregion

		#endregion

		private readonly IMessenger _messenger;

		private readonly IDictionary<string, Type> _annotationTypes = new Dictionary<string, Type>
		{
			{"LineAnnotation", typeof(LineAnnotation)},
			{"LineArrowAnnotation", typeof(LineArrowAnnotation)},
			{"TextAnnotation", typeof(TextAnnotation)},
			{"BoxAnnotation", typeof(BoxAnnotation)},
			{"HorizontalLineAnnotation", typeof(HorizontalLineAnnotation)},
			{"VerticalLineAnnotation", typeof(VerticalLineAnnotation)},
			{"AxisMarkerAnnotation", typeof(AxisMarkerAnnotation)},
		};

		private Point MouseDownPoint { get; set; }
		private bool _isAnnotationLeftMbDown;

		/// <summary>
		/// Creates new ChartControl
		/// </summary>
		public ChartControl() {
			InitializeComponent();

			if (ViewModelBase.IsInDesignModeStatic)
				ViewModel = new ChartControlViewModelDesign();
			else {
				_messenger = new Messenger();
				ViewModel = new ChartControlViewModel(_messenger);
				ViewModel.OnSeriesColorChanged += RaiseSeriesColorChanged;

				sciChart.RenderableSeries.CollectionChanged += SeriesSourceOnCollectionChanged;

				_messenger.Register<WindowMessages>(this, WindowMessages.ShowHelpWindow, s => new HelpWindow().Show());
				_messenger.Register<DialogMessage<ColorDialog>>(this,
						msg => {
							msg.Result.DialogResult = msg.Dialog.ShowDialog();
							msg.ProcessResult();
						});

				_messenger.Register<ViewMessageWithCallback<ChartBookmarkViewModel>>(this,
						WindowMessages.BookmarkNameWindow,
						msg => {
							if (msg.Action == ViewAction.ShowModal) {
								var wind = new BookmarkNameView(_messenger) {
									DataContext = new BookmarkNameViewModel(_messenger, msg.Callback)
								};
								wind.ShowDialog();
							}
						});
			}
		}

		private void SeriesSourceOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs) {
			if (notifyCollectionChangedEventArgs.Action == NotifyCollectionChangedAction.Add) {
				if (sciChart.RenderableSeries.Count == 1)
					sciChart.ZoomExtents();
			}
		}

		#region Public Events


		public new event MouseButtonEventHandler MouseRightButtonUp;

		internal void OnMouseRightButtonUp(object sender, MouseButtonEventArgs e) {
			MouseButtonEventHandler handler = MouseRightButtonUp;
			handler?.Invoke(this, e);
		}

		#endregion //Public Events


		#region Public Methods

		/// <summary>
		/// Сохранить настойки графика. Сюда входят настройки, которые пользователь вручную изменил на панели инструментов.
		/// </summary>
		/// <param name="fileName">Файл настроек.</param>
		public void SaveSettings(string fileName) {
			ViewModel.SaveSettings(fileName);
		}

		/// <summary>
		/// Загрузить настойки графика. Сюда входят настройки, которые пользователь вручную изменил на панели инструментов.
		/// </summary>
		/// <param name="fileName">Файл настроек.</param>
		public void LoadSettings(string fileName) {
			ViewModel.LoadSetings(fileName);
		}

		public void SaveState(string fileName) {
			ViewModel.SaveState(fileName);
		}

		public void LoadState(string fileName) {
			ViewModel.LoadState(fileName);
		}

		#endregion

		#region Private Methods

		private void VisibleRangeChanged(object sender, VisibleRangeChangedEventArgs e) {
			e.NewVisibleRange.Min = -0.2;
			e.NewVisibleRange.Max = 1.8;
		}

		private void SaveAsPngClick(object sender, RoutedEventArgs e) {
			var dialog = new SaveFileDialog {
				DefaultExt = "png",
				AddExtension = true,
				Filter = "PNG |*.png",
				InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
			};
			if (dialog.ShowDialog() == true) {
				// See SciChartSurfaceExtensions where ExportBitmapToFile is defined
				sciChart.ExportBitmapToFile(dialog.FileName);
				Process.Start(dialog.FileName);
			}
		}

		private void PrintClick(object sender, RoutedEventArgs e) {
			var dialog = new PrintDialog();
			if (dialog.ShowDialog() == true) {
				// Если тема содержит градиенты, то на win7 выбрасывается исключение. 
				//Поэтому временно включаем тему без градиентов.
				var prev = ThemeManager.GetTheme(sciChart);
				ThemeManager.SetTheme(sciChart, "Chrome");
				void PrintAction() => dialog.PrintVisual(sciChart, "График");
				PrintAction();
				ThemeManager.SetTheme(sciChart, prev);
			}
		}

		#endregion

		#region Private Methods For Working with user created annotations (lines etc)

		private void AddAnnotations(IEnumerable<IAnnotation> annotations) {
			foreach (var annotation in annotations) {
				sciChart.Annotations.Add(annotation);
			}
		}

		private void AnnotationsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args) {
			if (args.Action == NotifyCollectionChangedAction.Add) {
				foreach (var newItem in args.NewItems) {
					sciChart.Annotations.Add((IAnnotation)newItem);
				}
			}
			else if (args.Action == NotifyCollectionChangedAction.Remove) {
				foreach (var oldItem in args.OldItems) {
					sciChart.Annotations.Remove((IAnnotation)oldItem);
				}
			}
		}

		private void OnAnnotationCreated(object sender, EventArgs e) {
			var newAnnotation = (annotationCreation.Annotation as AnnotationBase);
			if (newAnnotation != null) {
				newAnnotation.IsEditable = true;
				newAnnotation.CanEditText = true;
			}

			pointerButton.IsChecked = true;
		}

		private void OnAnnotationTypeChanged(object sender, RoutedEventArgs e) {
			var annotationType = (sender as Control).Tag.ToString();

			var type = _annotationTypes[annotationType];

			var resourceName = String.Format("{0}Style", type.Name);
			if (Resources.Contains(resourceName)) {
				annotationCreation.AnnotationStyle = (Style)Resources[resourceName];
			}

			annotationCreation.AnnotationType = type;
		}

		private void OnDeleteSelectedAnnotationsClick(object sender, RoutedEventArgs e) {
			var selectedAnnotations = sciChart.Annotations.Where(annotation => annotation.IsSelected).ToList();

			foreach (var selectedAnnotation in selectedAnnotations) {
				sciChart.Annotations.Remove(selectedAnnotation);
			}
		}

		private void OnEditingEnabled(object sender, RoutedEventArgs e) {
			if (annotationCreation != null)
				annotationCreation.IsEnabled = false;

			foreach (var annotation in sciChart.Annotations) {
				annotation.IsEditable = true;
			}
		}

		private void OnEditDisabled(object sender, RoutedEventArgs e) {
			annotationCreation.IsEnabled = true;
		}

		#endregion

		private void SciChartMouseRightButtonUp(object sender, MouseButtonEventArgs e) {
			OnMouseRightButtonUp(sender, e);
		}

		private void UIElement_OnMouseUp(object sender, MouseButtonEventArgs e) {
			CreateValueLabelAnnotation();
		}


		/// <summary>
		/// Создаёт метку с текущим значением.
		/// </summary>
		private void CreateValueLabelAnnotation() {
			var allSeries = sciChart.RenderableSeries;

			var hitTestResults = allSeries.Select(x => x.HitTest(MouseDownPoint, 5)).ToArray();
			foreach (var hitTestResult in hitTestResults) {
				var y1 = sciChart.YAxes[0].GetDataValue(hitTestResult.HitTestPoint.Y + 15);

				///TODO: см метод ExportChartDataModifier.GetXAxisDataValueFixed
				var x1 = sciChart.XAxis.GetDataValue(hitTestResult.HitTestPoint.X + 15);

				var anot = new TextAnnotation {
					Text = hitTestResult.DataSeriesName + ": " + ((DateTime)hitTestResult.XValue).ToString("HH:mm:ss.fff") + "; " + ((double)hitTestResult.YValue).ToString("0.0000"),
					CoordinateMode = AnnotationCoordinateMode.Absolute,
					YAxisId = "mainYAxis",
					Y1 = y1,
					X1 = x1,
					IsEditable = true,
					Style = (Style)Resources["LabelsAnnotationStyle"]
				};
				anot.Unloaded += AnotOnUnloaded;


				var line = new LineArrowAnnotation {
					X1 = anot.X1,
					Y2 = hitTestResult.YValue,
					X2 = hitTestResult.XValue,
					YAxisId = "mainYAxis",
				};

				line.Selected += LineOnSelected;
				line.Style = (Style)Resources["LabelArrowAnnotationStyle"];

				line.Tag = anot;

				anot.Tag = line;
				anot.SizeChanged += AnotOnSizeChanged;
				anot.MouseLeftButtonUp += AnotOnMouseLeftButtonUp;
				anot.MouseLeftButtonDown += AnotOnMouseLeftButtonDown;
				anot.MouseMove += AnotOnMouseMove;

				sciChart.Annotations.Add(anot);

				sciChart.Annotations.Add(line);
			}
		}



		private void AnotOnMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs) {
			_isAnnotationLeftMbDown = true;
		}

		private void AnotOnMouseMove(object sender, MouseEventArgs args) {
			if (_isAnnotationLeftMbDown) {
				var anot = (TextAnnotation)args.Source;
				var line = (LineArrowAnnotation)anot.Tag;

				ConnectArrowAndValueLabelCoordinates(line, anot);
			}
		}

		private void AnotOnSizeChanged(object sender, SizeChangedEventArgs args) {
			var anot = (TextAnnotation)args.Source;
			var line = (LineArrowAnnotation)anot.Tag;

			ConnectArrowAndValueLabelCoordinates(line, anot);
		}

		private void AnotOnMouseLeftButtonUp(object sender, MouseButtonEventArgs args) {
			_isAnnotationLeftMbDown = false;

			var anot = (TextAnnotation)args.Source;
			var line = (LineArrowAnnotation)anot.Tag;

			ConnectArrowAndValueLabelCoordinates(line, anot);
		}

		private void ConnectArrowAndValueLabelCoordinates(LineArrowAnnotation line, TextAnnotation anot) {
			line.X1 = anot.X1;
			line.Y1 = GetAnnotationLeftBottomCornerCoordinate(anot);
		}

		private IComparable GetAnnotationLeftBottomCornerCoordinate(IAnnotation anot) {
			return sciChart.YAxes[0].GetDataValue(sciChart.YAxes[0].GetCoordinate(anot.Y1) + anot.ActualHeight);
		}


		private void AnotOnUnloaded(object sender, RoutedEventArgs args) {
			var anot = (TextAnnotation)args.Source;
			sciChart.Annotations.Remove((LineArrowAnnotation)anot.Tag);
		}

		private void LineOnSelected(object sender, EventArgs eventArgs) {

		}

		private void SciChart_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e) {
			MouseDownPoint = e.GetPosition((UIElement)sciChart.RenderSurface);
			//MouseDownPoint = e.GetPosition(sciChart.GridLinesPanel as UIElement);
		}

	}
}
