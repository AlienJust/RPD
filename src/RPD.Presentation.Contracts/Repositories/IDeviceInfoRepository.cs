using RPD.Presentation.Contracts.Model;

namespace RPD.Presentation.Contracts.Repositories {
	/// <summary>
	/// Позволяет получать и хранить объекты DeviceInfo.
	/// </summary>
	public interface IDeviceInfoRepository {
		/// <summary>
		/// Найти по номеру устройсва.
		/// </summary>
		/// <param name="deviceNumber">Номер устройства.</param>
		/// <returns>В случае если такого номера нет возвращает null.</returns>
		IDeviceInfo GetByDeviceNumber(int deviceNumber);

		void AddOrUpdate(IDeviceInfo deviceInfo);
	}
}