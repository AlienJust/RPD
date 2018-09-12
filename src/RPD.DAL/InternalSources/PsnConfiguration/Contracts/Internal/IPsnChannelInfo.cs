namespace RPD.DAL.PsnConfiguration.Contracts.Internal {

	/// <summary>
	/// ���������� � ������ ����� ���
	/// </summary>
	internal interface IPsnChannelInfo {
		/// <summary>
		/// ��� ������ ������
		/// </summary>
		PsnChannelTrendDataType Type { get; }

		/// <summary>
		/// �������� ������
		/// </summary>
		string Name { get; }

		/// <summary>
		/// ����� ��������/������������
		/// </summary>
		bool IsEnabled { get; }

		/// <summary>
		/// ����� �������� ��������
		/// </summary>
		bool IsInput { get; }

		/// <summary>
		/// ����� ����� ���� ����������� ������ ���
		/// </summary>
		bool CanBeFaultSign { get; }

		/// <summary>
		/// ����� ������������������ ���� ����������� ������ ���
		/// </summary>
		bool IsFaultSign { get; }

		/// <summary>
		/// ������������ ������
		/// </summary>
		IPsnChannelConfiguration Configuration { get; }

		/// <summary>
		/// ����� �������� ���, � �������� ��������� �����
		/// </summary>
		int MeterAddress { get; }
	}
}