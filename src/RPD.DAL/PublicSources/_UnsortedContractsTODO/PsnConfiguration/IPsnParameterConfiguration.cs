using System.Collections.Generic;

namespace RPD.DAL {
	/// <summary>
	/// ������������ ��������� ������� ��� 
	/// </summary>
	public interface IPsnParameterConfiguration : IObjectWithId
	{
		/// <summary>
		/// �������� ���������
		/// </summary>
		string Name { get; }

		/// <summary>
		/// ����, �������� �� ������ �������
		/// </summary>
		bool IsBitSignal { get; }

		/// <summary>
		/// �������� �������� ��������� �� ������ ����
		/// </summary>
		/// <param name="cmdPartContext">������� ������ ���� (������ - ����������)</param>
		/// <param name="startByte">������ ������������ ����� ���� ��������� �� � ������ �������� �������</param>
		/// <returns></returns>
		double GetValue(byte[] cmdPartContext, int startByte);
	}
}