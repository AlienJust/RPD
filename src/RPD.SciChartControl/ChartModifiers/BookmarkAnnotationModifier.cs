using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Abt.Controls.SciChart.ChartModifiers;
using Abt.Controls.SciChart.Visuals.Annotations;
using RPD.SciChartControl.ViewModel;

namespace RPD.SciChartControl.ChartModifiers
{
    internal class BookmarkAnnotationModifier : ChartModifierBase
    {
        public static readonly DependencyProperty AnnotationsSourceProperty = DependencyProperty.Register("AnnotationsSource",
            typeof(ObservableCollection<ChartBookmarkViewModel>),
            typeof(BookmarkAnnotationModifier), 
            new PropertyMetadata(null, OnAnnotationsSourceChanged));



        public ObservableCollection<ChartBookmarkViewModel> AnnotationsSource
        {
            get => (ObservableCollection<ChartBookmarkViewModel>)GetValue(AnnotationsSourceProperty);
            set => SetValue(AnnotationsSourceProperty, value);
        }

        // Get a notification when new labels are set.
        private static void OnAnnotationsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var modifier = (BookmarkAnnotationModifier)d;
            var newValue = e.NewValue as ObservableCollection<ChartBookmarkViewModel>;
            if (newValue == null) 
                return;

            newValue.CollectionChanged += modifier.NewValueOnCollectionChanged;

            modifier.RebuildAnnotations();
        }

        private void NewValueOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var anotVm in args.NewItems)
                {
                    CreateAnnotation((ChartBookmarkViewModel)anotVm);
                }
            }
            else if (args.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var anotVm in args.OldItems)
                {
                    var anot = ParentSurface.Annotations.FirstOrDefault(e => e.DataContext == anotVm);
                    ParentSurface.Annotations.Remove(anot);
                }
            }
            else if (args.Action == NotifyCollectionChangedAction.Reset)
            {
                var remList = new List<IAnnotation>();
                foreach (var anot in ParentSurface.Annotations )
                {
                    if (anot.DataContext is ChartBookmarkViewModel)
                        remList.Add(anot);
                }

                foreach (var annotation in remList)
                {
                    ParentSurface.Annotations.Remove(annotation);
                }
            }
        }

        private void CreateAnnotation(ChartBookmarkViewModel vm)
        {
            var annotationCollection = ParentSurface.Annotations;

            var line = new VerticalLineAnnotation
            {
                DataContext = vm,
                X1 = vm.Center,
                ShowLabel = false,
                VerticalAlignment = VerticalAlignment.Stretch,
                Stroke = new SolidColorBrush(Colors.Red),
                CoordinateMode = AnnotationCoordinateMode.Absolute,
                YAxisId = "mainYAxis",
                StrokeThickness = 1
            };

            line.AnnotationLabels.Add(new AnnotationLabel
            {
                Text = vm.Title, 
                LabelPlacement = LabelPlacement.Top
            });

            annotationCollection.Add(line);
        }


        private void RebuildAnnotations()
        {
            if (ParentSurface == null || AnnotationsSource == null)
                return;

            var annotationCollection = ParentSurface.Annotations;
            //annotationCollection.Clear();

            foreach (var anot in annotationCollection)
            {
                if (anot is VerticalLineAnnotation)
                    annotationCollection.Remove(anot);
            }

            foreach (var item in AnnotationsSource)
            {
                CreateAnnotation(item);
            }
        }

        /// <summary>
        /// Called when the Chart Modifier is attached to the Chart Surface
        /// </summary>
        public override void OnAttached()
        {
            base.OnAttached();
            /*
            base.XAxis.VisibleRangeChanged += (s, e) =>
            {
                if (e.NewVisibleRange.IsAnimating)
                {
                    // VisibleRangeChanged occurred during animation                       
                }
                else
                {
                    RebuildAnnotations(e.NewVisibleRange.Min, e.NewVisibleRange.Max);
                }
            };
            */
            // Catch the condition where AnnotationsSource binds before chart is shown. Rebuild annotations
            if (AnnotationsSource != null && ParentSurface.Annotations.Count == 0)
            {
                RebuildAnnotations();
            }
        }
    }
}