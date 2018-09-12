using System;
using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts;

namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Common {
	internal class PsnDataFragmentInformationSimple : IPsnDataFragmentInformation
	{
		public int StartOffset { get; set; }
		public DateTime? StartTime { get; set; }

		public override string ToString() {
			return "StartOffset=" + StartOffset + "     StartTime=" + (StartTime.HasValue ? StartTime.Value.ToString("yyyy.MM.dd HH:mm:ss") : "null");
		}
	}
}
