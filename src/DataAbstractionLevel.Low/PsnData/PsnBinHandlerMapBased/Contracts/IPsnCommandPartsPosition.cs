using DataAbstractionLevel.Low.PsnConfig.Contracts;


namespace DataAbstractionLevel.Low.PsnData.PsnBinHandlerMapBased.Contracts {
	interface IPsnCommandPartsPosition
	{
		int DataPosition { get; }
		IPsnProtocolCommandPartConfiguration CrcOkPartLocationInfo { get; }
	}
}