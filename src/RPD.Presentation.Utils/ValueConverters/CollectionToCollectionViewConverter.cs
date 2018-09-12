using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RPD.Presentation.Utils.ValueConverters
{
    public class CollectionToCollectionViewConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var view = new ListCollectionView((System.Collections.IList)value);
            if (parameter == null)
                throw new ArgumentException("Please pass sort property name to CollectionToCollectionViewConverter");
            var prop = parameter as string;
            if (prop == null)
                throw new ArgumentException("Please pass sort property name to CollectionToCollectionViewConverter");

            view.SortDescriptions.Add(new SortDescription(prop, ListSortDirection.Ascending));
            return view;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // not really necessary could just throw notsupportedexception
            var view = (CollectionView)value;
            return view.SourceCollection;
        }
    }
}