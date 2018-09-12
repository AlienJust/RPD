using System.Collections.Generic;

namespace RPD.DAL {
	/// <summary>
	/// ������������ ���������� ���
	/// </summary>
	public interface IPsnMeterConfig {
		/// <summary>
		/// �������� ����������
		/// </summary>
		string Name { get; }

		/// <summary>
		/// ������������ �������
		/// </summary>
		IEnumerable<IPsnChannelConfig> PsnChannelConfigs { get; }
	}
}