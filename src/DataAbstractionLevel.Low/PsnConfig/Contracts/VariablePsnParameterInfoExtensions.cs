namespace DataAbstractionLevel.Low.PsnConfig.Contracts {
	/// <summary>
	/// ��������������� ����� ���������� ��� ��� ���������
	/// </summary>
	public static class VariablePsnParameterInfoExtensions {
		/// <summary>
		/// ���������� 2 ��������� ��� ����� �����
		/// </summary>
		/// <param name="info1">������ �������� ��� ���������</param>
		/// <param name="info2">������ �������� ��� ���������</param>
		/// <returns>������, ���� ��������� ���������</returns>
		public static bool IsEqualTo(this IPsnProtocolParameterConfigurationVariable info1, IPsnProtocolParameterConfigurationVariable info2)
		{
			if (!((IPsnProtocolParameterConfiguration)info1).IsEqualTo(info2)) return false;
			return true;
		}
	}
}