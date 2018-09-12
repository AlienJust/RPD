using DataAbstractionLevel.Low.PsnConfig.Contracts;
using DataAbstractionLevel.Low.PsnData.Contracts;
using DataAbstractionLevel.Low.PsnData.PsnBinHandlerMapBased.Contracts;

namespace DataAbstractionLevel.Low.PsnData.PsnBinHandlerMapBased {
	sealed class PsnCommandPartAndConfirmation : IPsnCommandPartAndConfirmation {
		private readonly IPsnProtocolCommandPartConfiguration _commandPart;
		private readonly PsnCommandPartConfirmation _confirmation;

		public PsnCommandPartAndConfirmation(IPsnProtocolCommandPartConfiguration commandPart, PsnCommandPartConfirmation confirmation)
		{
			_commandPart = commandPart;
			_confirmation = confirmation;
		}

		public IPsnProtocolCommandPartConfiguration CommandPart => _commandPart;

		public PsnCommandPartConfirmation Confirmation => _confirmation;

		public override string ToString() {
			return _commandPart.PartName + " >> " + _confirmation.ToCustomString();
		}
	}
}