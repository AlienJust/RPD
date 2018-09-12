using AlienJust.Support.Identy.Contracts;

namespace DataAbstractionLevel.Low.PsnData.PsnBinHandlerMapBased.Contracts {
	interface IPsnMapRecord {
		IIdentifier CommandId { get; }
		IPsnMapSubrecord RequestRecordInfo { get; }
		IPsnMapSubrecord ReplyRecordInfo { get; }
	}
}