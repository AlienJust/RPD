using System;
using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts;

namespace RPD.DAL.WholeDeviceConfiguration.RelayOnLowLevel {
	class PsnLogFragmentInfoOnLowLevel : IPsnLogFragmentInfo {
		private readonly IPsnDataFragmentInformation _psnDataFragmentInformation;
		public PsnLogFragmentInfoOnLowLevel(IPsnDataFragmentInformation psnDataFragmentInformation) {
			_psnDataFragmentInformation = psnDataFragmentInformation;
		}

		public int StartOffset {
			get { return _psnDataFragmentInformation.StartOffset; }
		}

		public DateTime? StartTime {
			get { return _psnDataFragmentInformation.StartTime; }
		}
	}
}