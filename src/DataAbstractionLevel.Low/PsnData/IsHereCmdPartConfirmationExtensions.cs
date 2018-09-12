using DataAbstractionLevel.Low.PsnData.Contracts;
using DataAbstractionLevel.Low.PsnData.PsnBinHandlerMapBased;

namespace DataAbstractionLevel.Low.PsnData {
	static class IsHereCmdPartConfirmationExtensions {
		public static string ToCustomString(this PsnCommandPartConfirmation val) {
			switch (val) {
				case PsnCommandPartConfirmation.EverythyngIsOk:
					return "������������";
				case PsnCommandPartConfirmation.WrongFirstCrcByte:
					return "������ ���� CRC �� ���������";
				case PsnCommandPartConfirmation.WrongSecondCrcByte:
					return "������ ���� CRC �� ���������";
				case PsnCommandPartConfirmation.WrongAddress:
					return "����� �� ���������";
				case PsnCommandPartConfirmation.WrongCommandCode:
					return "��� ������� �� ���������";
				default:
					return null;
			}
		}
	}
}