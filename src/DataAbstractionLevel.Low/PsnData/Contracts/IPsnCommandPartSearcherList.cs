using System.Collections.Generic;
using DataAbstractionLevel.Low.PsnConfig.Contracts;

namespace DataAbstractionLevel.Low.PsnData.Contracts {
	public interface IPsnCommandPartSearcherList {
		/// <summary>
		/// ��������� ������� �� ���������� ���������������� �������� ������� � ������� �������� �������
		/// </summary>
		/// <param name="cmdPartData">������ ������, � �������� ����� ������������ ������ ����� �������</param>
		/// <param name="startByte">Start byte</param>
		/// <param name="cmdPartInfo">����� ������� (������ ��� �����)</param>
		/// <returns>���� "������� �������"</returns>
		PsnCommandPartConfirmation IsHereCmdPart(List<byte> cmdPartData, int startByte, IPsnProtocolCommandPartConfiguration cmdPartInfo);
	}
}