using DataAbstractionLevel.Low.PsnConfig.Contracts;

namespace RPD.DAL.PsnConfiguration.Contracts.Internal {
	/// <summary>
	/// Конфигурация ПСН канала
	/// </summary>
	internal interface IPsnChannelConfiguration {
		/// <summary>
		/// Часть команды линии ПСН (запрос или ответ), которой принадлежит канал
		/// </summary>
		IPsnProtocolCommandPartConfiguration CommandPartInfo { get; }

		/// <summary>
		/// Команда линии ПСН, которой принадлежит канал
		/// </summary>
		IPsnProtocolParameterConfigurationVariable CommandParameterConfigurationInfo { get; }
	}
}