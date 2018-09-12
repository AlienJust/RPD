using System.Collections.Generic;
using AlienJust.Support.Identy.Contracts;

namespace DataAbstractionLevel.Low.Storage.Contracts {

	/// <summary>
	/// ��������� ���������� �� �����������
	/// </summary>
	public interface IDeviceInformationStorage {
		/// <summary>
		/// ������������ ���� ���������� ���������� �� �����������
		/// </summary>
		IEnumerable<IDeviceInformation> DeviceInformations { get; }

		/// <summary>
		/// ��������� ����� ���������� �� ����������
		/// </summary>
		/// <param name="id">������������� ���������� �� ����������</param>
		/// <param name="name">�������� ���������� (��� ����������)</param>
		/// <param name="description">�������� ���������� (����� ������)</param>
		void Add(IIdentifier id, string name, string description);

		/// <summary>
		/// ������� ���������� �� ����������
		/// </summary>
		/// <param name="id">������������� ���������� ��� ��������</param>
		void Remove(IIdentifier id);

		/// <summary>
		/// ��������� ���������� �� ����������
		/// </summary>
		/// <param name="id">������������� ���������� ��� ����������</param>
		/// <param name="setName">����� ��� ����������</param>
		/// <param name="setDescription">����� ��������</param>
		void Update(IIdentifier id, string setName, string setDescription);
	}
}