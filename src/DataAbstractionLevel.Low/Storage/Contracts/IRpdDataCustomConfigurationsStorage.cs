using System.Collections.Generic;
using AlienJust.Support.Identy.Contracts;

namespace DataAbstractionLevel.Low.Storage.Contracts {
	/// <summary>
	/// ��������� "�����" �������� ��� ����� ���
	/// </summary>
	public interface IRpdDataCustomConfigurationsStorage
	{
		/// <summary>
		/// ������������ ���� �������� ���������
		/// </summary>
		IEnumerable<IRpdDataCustomConfigration> Configurations { get; }

		/// <summary>
		/// ��������� ������������ ���� � ���������
		/// </summary>
		/// <param name="rpdDataCustomConfigurationId">������������� ������������</param>
		/// <param name="rpdConfigruationId">������������� ������������ ��� �����</param>
		/// <param name="customLogName">���� �������� ���� ���</param>
		void Add(IIdentifier rpdDataCustomConfigurationId, IIdentifier rpdConfigruationId, string customLogName);

		/// <summary>
		/// ��������� ������������ ������ ���������
		/// </summary>
		/// <param name="rpdDataCustomConfigurationId">������������� ������������ ��� ����������</param>
		/// <param name="setRpdConfigruationId">����� ������������� ������������ ���������� ���</param>
		/// <param name="setCustomLogName">����� "����" �������� ���� ���</param>
		void Update(IIdentifier rpdDataCustomConfigurationId, IIdentifier setRpdConfigruationId, string setCustomLogName);

		/// <summary>
		/// ������� ������������ ���� ��� �� ���������
		/// </summary>
		/// <param name="rpdDataCustomConfigurationId"></param>
		void Remove(IIdentifier rpdDataCustomConfigurationId);
	}
}