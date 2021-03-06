using AlienJust.Support.Numeric;
using DataAbstractionLevel.Low.PsnConfig.Contracts;
using DataAbstractionLevel.Low.PsnData.Contracts;

namespace DataAbstractionLevel.Low.PsnData {
	public class PsnCommandPartSearcherStandart : IPsnCommandPartSearcher {
		public PsnCommandPartConfirmation IsHereCmdPart(byte[] cmdPartData, int startByte, IPsnProtocolCommandPartConfiguration cmdPartInfo) {
			// TODO: remove double comparison
			if (cmdPartInfo.Address.DefinedValue != cmdPartInfo.Address.GetValue(cmdPartData, startByte)) return PsnCommandPartConfirmation.WrongAddress;
			if (cmdPartInfo.CommandCode.DefinedValue != cmdPartInfo.CommandCode.GetValue(cmdPartData, startByte)) return PsnCommandPartConfirmation.WrongCommandCode;

			var crc16 = MathExtensions.GetCrc16FromArray(cmdPartData, startByte, cmdPartInfo.Length - 2);
			//Console.WriteLine("CRC16=" + crc16);
			if (crc16.High != cmdPartInfo.CrcHigh.GetValue(cmdPartData, startByte)) return PsnCommandPartConfirmation.WrongFirstCrcByte;
			if (crc16.Low != cmdPartInfo.CrcLow.GetValue(cmdPartData, startByte)) return PsnCommandPartConfirmation.WrongSecondCrcByte;

			return PsnCommandPartConfirmation.EverythyngIsOk;
		}
	}
}