using System.Collections.Generic;

namespace RPD.DAL.LowLevel.Storage.PsnConfig {
	/// <summary>
	/// Конфигурация параметра части команды протокола ПСН
	/// </summary>
	public interface IConfigurationPsnProtocolCommandPartParameter : IObjectWithIdentifier
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
		/// <returns>Значение</returns>
		double GetValue(IList<byte> cmdPartContext, int startByte);

		/// <summary>
		/// Идентификатор части команды ПСН, которой принадлежит данный параметр
		/// </summary>
		IIdentifier IdConfigurationPsnProtocolCommandPart { get; }
	}
}