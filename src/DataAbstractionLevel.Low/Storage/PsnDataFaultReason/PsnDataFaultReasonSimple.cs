using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.Contracts;

namespace DataAbstractionLevel.Low.Storage.PsnDataFaultReason {
	public class PsnDataFaultReasonSimple : IPsnDataFaultReason {
		private readonly IIdentifier _id;
		private readonly string _faultReason;
		public PsnDataFaultReasonSimple(IIdentifier id, string faultReason) {
			_id = id;
			_faultReason = faultReason;
		}

		public IIdentifier Id {
			get { return _id; }
		}

		public string FaultReason {
			get { return _faultReason; }
		}
	}
}