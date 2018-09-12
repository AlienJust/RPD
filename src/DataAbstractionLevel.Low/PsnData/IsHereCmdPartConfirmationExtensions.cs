using DataAbstractionLevel.Low.PsnData.Contracts;
using DataAbstractionLevel.Low.PsnData.PsnBinHandlerMapBased;

namespace DataAbstractionLevel.Low.PsnData {
	static class IsHereCmdPartConfirmationExtensions {
		public static string ToCustomString(this PsnCommandPartConfirmation val) {
			switch (val) {
				case PsnCommandPartConfirmation.EverythyngIsOk:
					return "Подтверждено";
				case PsnCommandPartConfirmation.WrongFirstCrcByte:
					return "Первый байт CRC не совпадает";
				case PsnCommandPartConfirmation.WrongSecondCrcByte:
					return "Второй байт CRC не совпадает";
				case PsnCommandPartConfirmation.WrongAddress:
					return "Адрес не совпадает";
				case PsnCommandPartConfirmation.WrongCommandCode:
					return "Код команды не совпадает";
				default:
					return null;
			}
		}
	}
}