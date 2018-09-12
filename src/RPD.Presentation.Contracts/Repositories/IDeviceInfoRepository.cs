using RPD.Presentation.Contracts.Model;

namespace RPD.Presentation.Contracts.Repositories {
	/// <summary>
	/// ��������� �������� � ������� ������� DeviceInfo.
	/// </summary>
	public interface IDeviceInfoRepository {
		/// <summary>
		/// ����� �� ������ ���������.
		/// </summary>
		/// <param name="deviceNumber">����� ����������.</param>
		/// <returns>� ������ ���� ������ ������ ��� ���������� null.</returns>
		IDeviceInfo GetByDeviceNumber(int deviceNumber);

		void AddOrUpdate(IDeviceInfo deviceInfo);
	}
}