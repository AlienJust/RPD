using DataAbstractionLevel.Low.PsnConfig.Contracts;
using DataAbstractionLevel.Low.PsnData.PsnBinHandlerMapBased.Contracts;

namespace DataAbstractionLevel.Low.PsnData.PsnBinHandlerMapBased {
	sealed class PsnCommandPartsAndPosition : IPsnCommandPartsPosition {

		public PsnCommandPartsAndPosition(int dataPosition, IPsnProtocolCommandPartConfiguration crcOkPartLocationInfo) {
			DataPosition = dataPosition;
			CrcOkPartLocationInfo = crcOkPartLocationInfo;
		}

		public int DataPosition { get; }
		public IPsnProtocolCommandPartConfiguration CrcOkPartLocationInfo { get; }

		/*
		public override string ToString() {
			string result = DataPosition + " >> ";
			if (CrcOkPartLocationInfo != null) result += "Найдена часть команды:" + CrcOkPartLocationInfo.PartName;
			else result += "Null";
			return result;
		}*/
	}
}