using System.Collections.Generic;
using RPD.DAL.PsnConfiguration.Contracts.Internal;

namespace RPD.DAL {
	/// <summary>
	/// ��������� ����� ������� ���
	/// </summary>
	public interface IPsnCommandPartInfo : IObjectWithId
	{
		/// <summary>
		/// �������� ����� �������
		/// </summary>
		string PartName { get; }

		/// <summary>
		/// ��� ����� ������� (������ ��� �����)
		/// </summary>
		PsnCommandPartType PartType { get; }

		/// <summary>
		/// ������ ��������� ��������, ������������� � ������ ����� �������
		/// </summary>
		List<IDefinedPsnParameterInfo> DefParams { get; }

		/// <summary>
		/// ������ ����������, ������������� � ������ ����� �������
		/// </summary>
		List<IVariablePsnParameterInfo> VarParams { get; }

		/// <summary>
		/// ����� �����
		/// </summary>
		int Length { get; }

		/// <summary>
		/// �������� ������������ ������ �������
		/// </summary>
		int Offset { get; }

		/// <summary>
		/// ����� ����������
		/// </summary>
		IDefinedPsnParameterInfo Address { get; }

		/// <summary>
		/// ��� �������
		/// </summary>
		IDefinedPsnParameterInfo CommandCode { get; }

		/// <summary>
		/// ������� ���� CRC
		/// </summary>
		IVariablePsnParameterInfo CrcLow { get; }

		/// <summary>
		/// ������� ���� CRC
		/// </summary>
		IVariablePsnParameterInfo CrcHigh { get; }

		/// <summary>
		/// ������������� �������, � ������� ��������� ������ �����
		/// </summary>
		IUid CommandId { get; }
	}
}