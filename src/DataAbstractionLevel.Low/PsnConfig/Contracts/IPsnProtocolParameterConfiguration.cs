using System.Collections.Generic;
using AlienJust.Support.Identy.Contracts;

namespace DataAbstractionLevel.Low.PsnConfig.Contracts {
	/// <summary>
	/// ������������ ��������� ������� ��� 
	/// </summary>
	public interface IPsnProtocolParameterConfiguration : IObjectWithIdentifier
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