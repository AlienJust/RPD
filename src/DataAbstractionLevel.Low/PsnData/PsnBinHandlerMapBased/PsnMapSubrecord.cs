using DataAbstractionLevel.Low.PsnConfig.Contracts;
using DataAbstractionLevel.Low.PsnData.PsnBinHandlerMapBased.Contracts;

namespace DataAbstractionLevel.Low.PsnData.PsnBinHandlerMapBased {
	internal sealed class PsnMapSubrecord : IPsnMapSubrecord {
		public PsnMapSubrecord(PsnCommandPartLocation locationInfo, IPsnProtocolCommandPartConfiguration commandPart)
		{
			LocationInfo = locationInfo;
			CommandPart = commandPart;
		}

		public PsnCommandPartLocation LocationInfo { get; }

		public IPsnProtocolCommandPartConfiguration CommandPart { get; }
	}
}