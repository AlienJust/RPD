using System;

namespace RPD.DAL {
	/// <summary>
	/// Класс, описывающий точку канала
	/// </summary>
	public interface IDataPoint
	{
		/// <summary>
		/// Значение точки
		/// </summary>
		double Value { get; }

		/// <summary>
		/// Время точки
		/// </summary>
		DateTime Time { get; }

		/// <summary>
		/// Флаг валидности значения точки, если значение точки - false, значит Value необходимо игнорировать
		/// Ну или можно усложнить сво-во Value, чтобы оно вырабатывало исключение, если сигнал невалиден
		/// </summary>
		bool Valid { get; }

		/// <summary>
		/// Позиция точки в потоке данных
		/// </summary>
		int DataPosition { get; }

		/// <summary>
		/// Получает байты запроса/ответа, из которых было получено значение точки
		/// </summary>
		/// <returns>Результирующие байты</returns>
		byte[] GetCommandBytes();
	}
}