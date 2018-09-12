using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RPD.DAL {
	/// <summary>
	/// ��� ���������� ���
	/// </summary>
	public interface IPsnLog : IObjectWithId, IStreamReadableObject
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
		PsnLogType LogType { get; }

		/// <summary>
		/// �������, ��� ��� �������� ��������� �� ����������
		/// </summary>
		bool IsLastDeviceLog { get; }

		/// <summary>
		/// ����������
		/// </summary>
		ObservableCollection<IPsnMeter> Meters { get; }


		/// <summary>
		/// ��������� ������������ ���
		/// </summary>
		/// <param name="psnConfig">��� ������������</param>
		/// <param name="customName">���������������� �������� ��� ����</param>
		void Update(IPsnConfiguration psnConfig, string customName);

		/// <summary>
		/// ������������ ��� ��� ������� ����
		/// </summary>
		IPsnConfiguration PsnConfiguration { get; }

		/// <summary>
		/// �������� ���������� � ���� ����������
		/// </summary>
		/// <param name="callback">����� ��������� ������, � ����������� ���������� �� ����� ����� ���������� � ����������� � ��������� ����</param>
		void GetStatisticAsync(Action<Exception, IEnumerable<string>> callback);

		/// <summary>
		/// ��� �� �� ��� � �����?
		/// </summary>
		PsnLogIntegrity LogIntegrity { get; }

		/// <summary>
		/// �������, ���������� ��� ����� ��������� ����� LogIntegrity
		/// </summary>
		event EventHandler IsSomethingWrongWithLogChanged;


		/// <summary>
		/// �������� ����
		/// </summary>
		string Name { get; }
	}
}