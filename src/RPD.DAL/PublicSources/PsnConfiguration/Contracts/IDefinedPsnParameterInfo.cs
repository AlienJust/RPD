namespace RPD.DAL {
	/// <summary>
	/// ��������� �������� ��� � ������� ��������� ��������� (����� �������, ��� ������� � �.�.)
	/// </summary>
	public interface IDefinedPsnParameterInfo : IPsnParameterConfiguration
	{
		/// <summary>
		/// ������������ ������� �������� ���������
		/// </summary>
		double DefinedValue { get; }
	}
}