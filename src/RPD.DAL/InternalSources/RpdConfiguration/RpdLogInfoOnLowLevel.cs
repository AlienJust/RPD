using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts;

namespace RPD.DAL {
	class RpdLogInfoOnLowLevel : IRpdLogInfo {
		private readonly IRpdDataInformation _rpdDataInformation;
		public RpdLogInfoOnLowLevel(IRpdDataInformation rpdDataInformation) {
			_rpdDataInformation = rpdDataInformation;
		}

		public int Number {
			get { return _rpdDataInformation.Number; }
		}

		public int Status {
			get { return _rpdDataInformation.Status; }
		}

		public int Year {
			get { return _rpdDataInformation.Year; }
		}

		public int Month {
			get { return _rpdDataInformation.Month; }
		}

		public int Day {
			get { return _rpdDataInformation.Day; }
		}

		public int Hour {
			get { return _rpdDataInformation.Hour; }
		}

		public int Minute {
			get { return _rpdDataInformation.Minute; }
		}

		public int Second {
			get { return _rpdDataInformation.Second; }
		}

		public int DescriptionPageAddress {
			get { return _rpdDataInformation.DescriptionPageAddress; }
		}

		public int LastWrittenPageAddress {
			get { return _rpdDataInformation.LastWrittenPageAddress; }
		}

		public int FaultWasReaded {
			get { return _rpdDataInformation.FaultWasReaded; }
		}

		public int BadPageCounter {
			get { return _rpdDataInformation.BadPageCounter; }
		}
	}
}