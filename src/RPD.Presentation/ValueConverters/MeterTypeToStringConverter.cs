//using RPD.DAL;
using System;
using System.Globalization;
using System.Windows.Data;
using RPD.DAL;

namespace RPD.Presentation.ValueConverters {
	/// <summary>
	/// Преобразователь Bool в Visibility.
	/// </summary>
	[ValueConversion(typeof(long), typeof(string))]
	public class MeterTypeToStringConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			switch ((RpdMeterType)value) {
				case RpdMeterType.Irvi: return "ИРВИ";
				case RpdMeterType.Uran: return "УРАН";
				default: return "Неопределен";
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}
}
