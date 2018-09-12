using System.Collections.Generic;

namespace RPD.DAL {
	/// <summary>
	/// Конфигурация параметра команды ПСН 
	/// </summary>
	public interface IPsnParameterConfiguration : IObjectWithId
	{
		/// <summary>
		/// Название параметра
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Флаг, является ли сигнал битовым
		/// </summary>
		bool IsBitSignal { get; }

		/// <summary>
		/// Получает значение параметра из набора байт
		/// </summary>
		/// <param name="cmdPartContext">Входной массив байт (иногда - избыточный)</param>
		/// <param name="startByte">Иногда интересующий набор байт находится не в начале входного массива</param>
		/// <returns></returns>
		double GetValue(byte[] cmdPartContext, int startByte);
	}
}