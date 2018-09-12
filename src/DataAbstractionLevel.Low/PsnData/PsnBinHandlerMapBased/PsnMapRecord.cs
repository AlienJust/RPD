using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.PsnData.PsnBinHandlerMapBased.Contracts;

namespace DataAbstractionLevel.Low.PsnData.PsnBinHandlerMapBased {
	internal sealed class PsnMapRecord : IPsnMapRecord {
		public IIdentifier CommandId { get; }
		public IPsnMapSubrecord RequestRecordInfo { get; }
		public IPsnMapSubrecord ReplyRecordInfo { get; }

		public PsnMapRecord(IIdentifier commandId, IPsnMapSubrecord requestRecordInfo, IPsnMapSubrecord replyRecordInfo)
		{
			CommandId = commandId;
			RequestRecordInfo = requestRecordInfo;
			ReplyRecordInfo = replyRecordInfo;
		}

		public override string ToString() {
			return CommandId.IdentyString + ": " + (RequestRecordInfo != null ? ">>" : "__") + (ReplyRecordInfo != null ? " <<" : " __");
		}
	}
}