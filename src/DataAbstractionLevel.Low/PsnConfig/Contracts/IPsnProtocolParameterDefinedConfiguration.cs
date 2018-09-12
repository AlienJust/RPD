namespace DataAbstractionLevel.Low.PsnConfig.Contracts {
	/// <summary>
	/// ��������� �������� ��� � ������� ��������� ��������� (����� �������, ��� ������� � �.�.)
	/// </summary>
	public interface IPsnProtocolParameterDefinedConfiguration : IPsnProtocolParameterConfiguration
	{
		/// <summary>
		/// ������������ ������� �������� ���������
		/// </summary>
		double DefinedValue { get; }
	}
}