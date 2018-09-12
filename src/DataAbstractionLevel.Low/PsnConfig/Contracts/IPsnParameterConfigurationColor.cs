using AlienJust.Support.Identy.Contracts;

namespace DataAbstractionLevel.Low.PsnConfig.Contracts {
	/// <summary>
	/// Цветовая конфигурация ПСН параметра
	/// </summary>
	public interface IPsnParameterConfigurationColor : IObjectWithIdentifier
	{
		/// <summary>
		/// Цвет по умолчанию
		/// </summary>
		int DefaultColor { get; set; }
	}
}