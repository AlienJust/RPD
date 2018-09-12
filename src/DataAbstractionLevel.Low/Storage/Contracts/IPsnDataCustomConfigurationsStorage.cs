using System.Collections.Generic;
using AlienJust.Support.Identy.Contracts;

namespace DataAbstractionLevel.Low.Storage.Contracts {
	/// <summary>
	/// ��������� "�����" �������� ��� ����� ���
	/// </summary>
	public interface IPsnDataCustomConfigurationsStorage {
		/// <summary>
		/// ������������ ���� �������� ���������
		/// </summary>
		IEnumerable<IPsnDataCustomConfigration> Configurations { get; }

		/// <summary>
		/// ��������� ������������ ���� � ���������
		/// </summary>
		/// <param name="psnDataCustomConfigurationId">������������� ������������</param>
		/// <param name="psnConfigruationId">������������� ������������ ��� �����</param>
		/// <param name="customLogName">���� �������� ���� ���</param>
		void Add(IIdentifier psnDataCustomConfigurationId, IIdentifier psnConfigruationId, string customLogName);

		/// <summary>
		/// ��������� ������������ ������ ���������
		/// </summary>
		/// <param name="psnDataCustomConfigurationId">������������� ������������ ��� ����������</param>
		/// <param name="setPsnConfigruationId">����� ������������� ������������ ���������� ���</param>
		/// <param name="setCustomLogName">����� "����" �������� ���� ���</param>
		void Update(IIdentifier psnDataCustomConfigurationId, IIdentifier setPsnConfigruationId, string setCustomLogName);

		/// <summary>
		/// ������� ������������ ���� ��� �� ���������
		/// </summary>
		/// <param name="psnDataCustomConfigurationId"></param>
		void Remove(IIdentifier psnDataCustomConfigurationId);
	}
}