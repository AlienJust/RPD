using System.Windows;
using Abt.Controls.SciChart.ChartModifiers;

namespace RPD.SciChartControl.ChartModifiers {
	internal class RolloverModifierExt : RolloverModifier {
		public static readonly DependencyProperty TestProperty = DependencyProperty.Register(
				"Test", typeof(string), typeof(RolloverModifierExt), new PropertyMetadata(default(string)));


		public string Test {
			get => (string)GetValue(TestProperty);
			set => SetValue(TestProperty, value);
		}

		public static readonly DependencyProperty AnnotationStyleProperty = DependencyProperty.Register(
				"AnnotationStyle", typeof(Style), typeof(RolloverModifierExt), new PropertyMetadata(default(Style)));

		public Style AnnotationStyle {
			get => (Style)GetValue(AnnotationStyleProperty);
			set => SetValue(AnnotationStyleProperty, value);
		}

		/*
		public override void OnModifierMouseDown(ModifierMouseArgs e) {
			base.OnModifierMouseDown(e);
			return;


			/*
			if (e.MouseButtons != MouseButtons.Right)
				return;

			var allSeries = ParentSurface.RenderableSeries;

			// Translates the mouse point to chart area, e.g. when you have left axis
			var pt = GetPointRelativeTo(e.MousePoint, ModifierSurface);

			// TODO: Using Clear will affect other modifiers so it is best to remove only those added by this modifier
			// this.ModifierSurface.Clear();

			// Add the rollover line to the ModifierSurface, which is just a canvas over the main chart area, on top of series
			//ModifierSurface.Children.Add();

			var hitTestResults = allSeries.Select(x => x.HitTest(pt)).ToArray();
			foreach (var hitTestResult in hitTestResults) {
				var anot = new TextAnnotation {
					Text = hitTestResult.DataSeriesName + ": " + ((DateTime)hitTestResult.XValue).ToString("HH:mm:ss fff") +
						"; " + hitTestResult.YValue,
					FontSize = 12,
					CoordinateMode = AnnotationCoordinateMode.Absolute,
					YAxisId = "mainYAxis",
					Y1 = hitTestResult.YValue,
					X1 = hitTestResult.XValue,
					IsEditable = true
				};
				anot.Style = AnnotationStyle;
				anot.ContextMenu = new ContextMenu();
				anot.ContextMenu.Items.Add(new TextBlock() { Text = "Удалить" });

				ParentSurface.Annotations.Add(anot);
			}
			*/
		//}
	}
}
