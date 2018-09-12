using System;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.Shared;

namespace DataAbstractionLevel.Low.Storage.Contracts {
	/// <summary>
	/// ���������� � ��� ����
	/// </summary>
	public interface IPsnDataInformation : IObjectWithIdentifier
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
		/// ��� ����
		/// </summary>
		PsnDataFragmentType DataFragmentType { get; }

		/// <summary>
		/// ���� ����, ��� ��� �������� ��������� ����� �� ����������
		/// </summary>
		bool IsLastDeviceLog { get; }

		/// <summary>
		/// ������������� ���������� �� ����������
		/// </summary>
		IIdentifier DeviceInformationId { get; }
	}
}