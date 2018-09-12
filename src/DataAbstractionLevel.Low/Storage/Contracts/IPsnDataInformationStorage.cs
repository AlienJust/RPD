using System;
using System.Collections.Generic;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.Shared;

namespace DataAbstractionLevel.Low.Storage.Contracts {
	/// <summary>
	/// ��������� ������ ���������� � ��� ����
	/// </summary>
	public interface IPsnDataInformationStorage {
		/// <summary>
		/// ������ ����� ���
		/// </summary>
		IEnumerable<IPsnDataInformation> PsnDataInformations { get; }

		/// <summary>
		/// ��������� � ��������� ���������� � ������ ���
		/// </summary>
		/// <param name="psnLogInformationId"></param>
		/// <param name="beginTime"></param>
		/// <param name="endTime"></param>
		/// <param name="saveTime"></param>
		/// <param name="psnDataType"></param>
		/// <param name="isLastDeviceLog"></param>
		/// <param name="deviceInformationId"></param>
		void Add(IIdentifier psnLogInformationId, DateTime? beginTime, DateTime? endTime, DateTime? saveTime, PsnDataFragmentType psnDataType, bool isLastDeviceLog, IIdentifier deviceInformationId);


		/// <summary>
		/// ������� �� ��������� ���������� � ������ ���
		/// </summary>
		/// <param name="psnLogInformationId"></param>
		void Remove(IIdentifier psnLogInformationId);
	}
}