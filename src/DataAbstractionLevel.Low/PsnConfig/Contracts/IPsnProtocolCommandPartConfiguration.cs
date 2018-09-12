using AlienJust.Support.Identy.Contracts;

namespace DataAbstractionLevel.Low.PsnConfig.Contracts {
	/// <summary>
	/// Описывает кусок команды ПСН
	/// </summary>
	public interface IPsnProtocolCommandPartConfiguration : IObjectWithIdentifier {
		/// <summary>
		/// Название куска команды
		/// </summary>
		string PartName { get; }

		/// <summary>
		/// Тип части команды (запрос или ответ)
		/// </summary>
		PsnProtocolCommandPartType PartType { get; }

		/// <summary>
		/// Список известных значений, встречающихся в данной части команды
		/// </summary>
		IPsnProtocolParameterDefinedConfiguration[] DefParams { get; } // TODO: convert to array

		/// <summary>
		/// Список переменных, встречающихся в данной части команды
		/// </summary>
		IPsnProtocolParameterConfigurationVariable[] VarParams { get; } // TODO: convert to array

		/// <summary>
		/// Длина части
		/// </summary>
		byte Length { get; }

		/// <summary>
		/// Смещение относительно начала команды
		/// </summary>
		int Offset { get; }

		/// <summary>
		/// Адрес устройства
		/// </summary>
		IPsnProtocolParameterDefinedConfiguration Address { get; }

		/// <summary>
		/// Код команды
		/// </summary>
		IPsnProtocolParameterDefinedConfiguration CommandCode { get; }

		/// <summary>
		/// Младший байт CRC
		/// </summary>
		IPsnProtocolParameterConfigurationVariable CrcLow { get; }

		/// <summary>
		/// Старший байт CRC
		/// </summary>
		IPsnProtocolParameterConfigurationVariable CrcHigh { get; }

		/// <summary>
		/// Идентификатор команды, к которой относится данная часть
		/// </summary>
		IIdentifier CommandId { get; }
	}
}
