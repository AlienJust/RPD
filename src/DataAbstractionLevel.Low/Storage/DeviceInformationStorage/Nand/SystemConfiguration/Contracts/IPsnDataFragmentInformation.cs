using System;

namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts {
	/// <summary>
	/// ���������� � ��������� ���� ���
	/// </summary>
	public interface IPsnDataFragmentInformation {
		/// <summary>
		/// �������� ������
		/// </summary>
		int StartOffset { get; }

		/// <summary>
		/// ������ ��������� ����
		/// </summary>
		DateTime? StartTime { get; }
	}
}