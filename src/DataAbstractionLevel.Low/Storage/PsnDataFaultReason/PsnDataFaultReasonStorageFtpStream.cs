using System;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.Contracts;
using DataAbstractionLevel.Low.Storage.StreamableData.Contracts;

namespace DataAbstractionLevel.Low.Storage.PsnDataFaultReason {
	internal sealed class PsnDataFaultReasonStorageFtpStream : IPsnDataFaultReason {
		private readonly IStreamedData _relayOnStream;
		private readonly IIdentifier _id;

		private readonly Lazy<string> _faultReason;

		public PsnDataFaultReasonStorageFtpStream(IStreamedData relayOnStream, IIdentifier id)
		{
			_relayOnStream = relayOnStream;
			_id = id;
			_faultReason = new Lazy<string>(() => {
				var buffer = new byte[10];
				using (var stream = _relayOnStream.GetStreamForReading()) {
					stream.Read(buffer, 0, 10);
				}

				var result = string.Empty;
				result += "Номер протокола: 0x" + buffer[0].ToString("X2") + buffer[1].ToString("X2") + Environment.NewLine;
				result += "Причина создания дампа: "
				          + buffer[3].ToString("X2") + " "
				          + buffer[4].ToString("X2") + " "
				          + buffer[5].ToString("X2") + " "
				          + buffer[6].ToString("X2") + " "
				          + "  "
				          + buffer[7].ToString("X2") + " "
				          + buffer[8].ToString("X2") + " "
				          + buffer[9].ToString("X2") + " "
				          + buffer[10].ToString("X2");
				return result;
			});
		}

		public IIdentifier Id
		{
			get { return _id; }
		}

		public string FaultReason {
			get { return _faultReason.Value; }
		}
	}
}