using AlienJust.Support.Identy.Contracts;

namespace DataAbstractionLevel.Low.Storage.Contracts {
	/// <summary>
	/// Информация об устройстве
	/// </summary>
	public interface IDeviceInformation : IObjectWithIdentifier {
		/// <summary>
		/// Название устройства
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Описание устройства
		/// </summary>
		string Description { get; }
	}
}