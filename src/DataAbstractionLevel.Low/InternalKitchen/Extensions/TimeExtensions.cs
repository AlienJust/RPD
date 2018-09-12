using System;

namespace DataAbstractionLevel.Low.InternalKitchen.Extensions {
	internal static class TimeExtensions {
		/// <summary>
		/// Сравнивает даты с точностью до секунды, если совпадают - возвращает истину
		/// (подумать, возможно реализация TimeSpan.FromTicks(dt.Ticks).TotalSeconds == -//- юудет быстрее)
		/// </summary>
		/// <param name="dt">Сам объект</param>
		/// <param name="dateTimeToCompare">Дата для сравнения</param>
		/// <returns>Истина, если даты совпадают</returns>
		public static bool EqualToEvenSecond(this DateTime dt, DateTime dateTimeToCompare) {
			bool result = dt.Year == dateTimeToCompare.Year &&
										dt.Month == dateTimeToCompare.Month &&
										dt.Day == dateTimeToCompare.Day &&
										dt.Hour == dateTimeToCompare.Hour &&
										dt.Minute == dateTimeToCompare.Minute &&
										dt.Second > dateTimeToCompare.Second - 3 && dt.Second < dateTimeToCompare.Second + 3;
			return result;
		}

		public static bool EqualToEvenSecond(this DateTime? dt, DateTime? dateTimeToCompare) {
			if (!dt.HasValue || !dateTimeToCompare.HasValue) return false;

			bool result = dt.Value.Year == dateTimeToCompare.Value.Year &&
								dt.Value.Month == dateTimeToCompare.Value.Month &&
								dt.Value.Day == dateTimeToCompare.Value.Day &&
								dt.Value.Hour == dateTimeToCompare.Value.Hour &&
								dt.Value.Minute == dateTimeToCompare.Value.Minute &&
										dt.Value.Second > dateTimeToCompare.Value.Second - 3 && dt.Value.Second < dateTimeToCompare.Value.Second + 3;
			return result;
		}

		public static string ToMsString(this DateTime dt) {
			return dt.ToString("yyyy.MM.dd-HH:mm:ss.fff");
		}

		public static string ToMsString(this DateTime? dt) {
			if (dt.HasValue)
				return dt.Value.ToString("yyyy.MM.dd-HH:mm:ss.fff");
			return "NULL";
		}

		public static string ToFileMsString(this DateTime? dt) {
			if (dt.HasValue)
				return dt.Value.ToString("yyyy.MM.dd.HH.mm.ss.fff");
			return "NULL";
		}
		public static string ToFileMsString(this DateTime dt) {
			return dt.ToString("yyyy.MM.dd.HH.mm.ss.fff");
		}

		public static DateTime? FromFileMsString(this string value) {
			try {
				var parts = value.Split('.');
				return new DateTime(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[3]), int.Parse(parts[4]), int.Parse(parts[5]), int.Parse(parts[6]));
			}
			catch {
				return null;
			}
		}
	}
}
