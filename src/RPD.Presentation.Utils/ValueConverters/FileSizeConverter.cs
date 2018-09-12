using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace RPD.Presentation.Utils.ValueConverters
{
    /// <summary>
    /// Преобразует число размер файла в human readable form.
    /// </summary>
    [ValueConversion(typeof(long), typeof(string))]
    public class FileSizeConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            long size = (long)value;
            
            // размер меньше мегабайта, отображаем килобайты
            if (size <= 1024*1024)
                return (size/1024).ToString() + " Кб";
            // размер меньше гигабайта, отображаем мегабайты
            else if (size <= 1024*1024*1024)
                return (size / (1024*1024)).ToString() + " Мб";
            else
                return (size / (1024 * 1024*1024)).ToString() + " Гб";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
