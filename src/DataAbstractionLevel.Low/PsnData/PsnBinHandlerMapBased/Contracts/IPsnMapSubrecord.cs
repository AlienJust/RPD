using DataAbstractionLevel.Low.PsnConfig.Contracts;

namespace DataAbstractionLevel.Low.PsnData.PsnBinHandlerMapBased.Contracts {
	interface IPsnMapSubrecord {
		PsnCommandPartLocation LocationInfo { get; }
		IPsnProtocolCommandPartConfiguration CommandPart { get; }
	}
}