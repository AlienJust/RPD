using System.Collections.Generic;
using RPD.DAL.PsnConfiguration.Contracts.Internal;

namespace RPD.DAL {
	/// <summary>
	/// Описывает кусок команды ПСН
	/// </summary>
	public interface IPsnCommandPartInfo : IObjectWithId
	{
		/// <summary>
		/// Название куска команды
		/// </summary>
		string PartName { get; }

		/// <summary>
		/// Тип части команды (запрос или ответ)
		/// </summary>
		PsnCommandPartType PartType { get; }

		/// <summary>
		/// Список известных значений, встречающихся в данной части команды
		/// </summary>
		List<IDefinedPsnParameterInfo> DefParams { get; }

		/// <summary>
		/// Список переменных, встречающихся в данной части команды
		/// </summary>
		List<IVariablePsnParameterInfo> VarParams { get; }

		/// <summary>
		/// Длина части
		/// </summary>
		int Length { get; }

		/// <summary>
		/// Смещение относительно начала команды
		/// </summary>
		int Offset { get; }

		/// <summary>
		/// Адрес устройства
		/// </summary>
		IDefinedPsnParameterInfo Address { get; }

		/// <summary>
		/// Код команды
		/// </summary>
		IDefinedPsnParameterInfo CommandCode { get; }

		/// <summary>
		/// Младший байт CRC
		/// </summary>
		IVariablePsnParameterInfo CrcLow { get; }

		/// <summary>
		/// Старший байт CRC
		/// </summary>
		IVariablePsnParameterInfo CrcHigh { get; }

		/// <summary>
		/// Идентификатор команды, к которой относится данная часть
		/// </summary>
		IUid CommandId { get; }
	}
}