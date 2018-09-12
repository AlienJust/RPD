using System;
using AlienJust.Support.Identy.Contracts;

namespace DataAbstractionLevel.Low.Storage.Contracts {
	/// <summary>
	/// ���������� � ���� ���
	/// </summary>
	public interface IRpdDataInformation : IObjectWithIdentifier
	{
		/// <summary>
		/// ����� ������ ����
		/// </summary>
		DateTime? BeginTime { get; }

		/// <summary>
		/// ����� ���������� ����
		/// </summary>
		DateTime? EndTime { get; }

		/// <summary>
		/// ����� ���������� ���� � �����������
		/// </summary>
		DateTime? SaveTime { get; }

		/// <summary>
		/// ���� ����, ��� ��� �������� ��������� ����� �� ����������
		/// </summary>
		bool IsLastDeviceLog { get; }

		/// <summary>
		/// ������������� ���������� �� ����������
		/// </summary>
		IIdentifier DeviceInformationId { get; }

		//IUid LinkedPsnLogId { get; }
	}
}