using System;

namespace DataAbstractionLevel.Low.PsnData.Contracts {
	/// <summary>
	/// Класс, описывающий точку канала
	/// </summary>
	public struct DataPoint {
		/// <summary>
		/// Значение точки
		/// </summary>
		public readonly double Value;

		/// <summary>
		/// Время точки
		/// </summary>
		public readonly DateTime Time;

		/// <summary>
		/// Флаг валидности значения точки, если значение точки - false, значит Value необходимо игнорировать
		/// </summary>
		public readonly bool Valid;

		/// <summary>
		/// Позиция точки в потоке данных
		/// </summary>
		public readonly int DataPosition;

		public DataPoint(double value, DateTime time, bool valid, int dataPosition) : this() {
			Value = value;
			Time = time;
			Valid = valid;
			DataPosition = dataPosition;
		}
	}
}
