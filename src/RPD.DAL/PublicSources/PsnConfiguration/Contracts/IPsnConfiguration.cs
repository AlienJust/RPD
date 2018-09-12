using System;
using System.Collections.Generic;

namespace RPD.DAL {
	/// <summary>
	/// ������������ �������� ���
	/// </summary>
	public interface IPsnConfiguration {
		/// <summary>
		/// ������������� ������������ ��������� ���
		/// </summary>
		Guid Id { get; }

		/// <summary>
		/// ������������� ��� ��������� ����� ���
		/// </summary>
		int RpdId { get; }

		/// <summary>
		/// �������� ��������� ���
		/// </summary>
		string Name { get; }

		/// <summary>
		/// ������ ��������� ���
		/// </summary>
		string Version { get; }

		/// <summary>
		/// �������� ��������� ���
		/// </summary>
		string Description { get; }

		/// <summary>
		/// ������������ ����������� ���
		/// </summary>
		IEnumerable<IPsnMeterConfig> PsnMeterConfigs { get; }
	}
}