using DataAbstractionLevel.Low.PsnConfig.Contracts;
using DataAbstractionLevel.Low.PsnData.Contracts;

namespace DataAbstractionLevel.Low.PsnData.PsnBinHandlerMapBased.Contracts {
	internal interface IPsnCommandPartAndConfirmation {
		IPsnProtocolCommandPartConfiguration CommandPart { get; }
		PsnCommandPartConfirmation Confirmation { get; }
	}
}