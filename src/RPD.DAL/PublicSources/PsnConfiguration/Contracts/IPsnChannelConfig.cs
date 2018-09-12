using System;

namespace RPD.DAL {
	/// <summary>
	/// ������������ ������ ���
	/// </summary>
	public interface IPsnChannelConfig {
		/// <summary>
		/// ���������� ������������� ������
		/// </summary>
		Guid Id { get; }

		/// <summary>
		/// �������� ������
		/// </summary>
		String Name { get; }
	}
}