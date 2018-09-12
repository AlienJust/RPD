namespace RPD.DAL.PsnConfiguration.SimpleReleaseAndExtensions {
	/// <summary>
	/// ��������������� ����� ���������� ��� ��� ���������
	/// </summary>
	public static class VariablePsnParameterInfoExtensions
	{
		/// <summary>
		/// ���������� 2 ��������� ��� ����� �����
		/// </summary>
		/// <param name="info1">������ �������� ��� ���������</param>
		/// <param name="info2">������ �������� ��� ���������</param>
		/// <returns>������, ���� ��������� ���������</returns>
		public static bool IsEqualTo(this IVariablePsnParameterInfo info1, IVariablePsnParameterInfo info2)
		{
			if (!((IPsnParameterConfiguration)info1).IsEqualTo(info2)) return false;
			return true;
		}
	}
}