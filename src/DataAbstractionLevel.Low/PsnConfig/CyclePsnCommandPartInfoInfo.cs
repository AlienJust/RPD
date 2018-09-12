using System;
using DataAbstractionLevel.Low.PsnConfig.Contracts;

namespace DataAbstractionLevel.Low.PsnConfig {
	internal class CyclePsnCommandPartInfoInfo : PsnCommandPartInfo, IPsnProtocolCommandPartConfigurationCycle {
		public TimeSpan CyclePeriod { get; set; }
	}
}