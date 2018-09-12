using System;
using System.Collections.Generic;
using AlienJust.Support.Identy.Contracts;

namespace DataAbstractionLevel.Low.Storage.Contracts {
	/// <summary>
	/// ��������� ������ ���������� � ��� ����
	/// </summary>
	public interface IRpdDataInformationStorage
	{
		/// <summary>
		/// ������ �������� ��������� (�������: ���������� � ���� ���)
		/// </summary>
		IEnumerable<IRpdDataInformation> RpdDataInformations { get; }

		/// <summary>
		/// ��������� � ��������� ���������� � ������ ���
		/// </summary>
		/// <param name="rpdLogInformationId">������������� �������</param>
		/// <param name="beginTime">����� ������ ����</param>
		/// <param name="endTime">����� ��������� ����</param>
		/// <param name="saveTime">����� ���������� ����</param>
		/// <param name="isLastDeviceLog">�������� �� ��� ��������� �� ����������</param>
		/// <param name="deviceInformationId">������������� ���������� �� ����������</param>
		void Add(IIdentifier rpdLogInformationId, DateTime? beginTime, DateTime? endTime, DateTime? saveTime, bool isLastDeviceLog, IIdentifier deviceInformationId);


		/// <summary>
		/// ������� �� ��������� ���������� � ������ ���
		/// </summary>
		/// <param name="rpdLogInformationId">������������� ���� ��� ��������</param>
		void Remove(IIdentifier rpdLogInformationId);
	}
}