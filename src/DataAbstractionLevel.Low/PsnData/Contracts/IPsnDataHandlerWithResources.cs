using System;
using System.Collections.Generic;
using DataAbstractionLevel.Low.PsnConfig.Contracts;

namespace DataAbstractionLevel.Low.PsnData.Contracts {
	public interface IPsnDataHandlerWithResources : IPsnDataHandler {
		void FillResources(
			IPsnProtocolConfiguration protocol, 
			DateTime beginTime, 
			IEnumerable<IPsnProtocolCommandPartConfigurationCycle> cycleCmdPartInfo, 
			IEnumerable<IPsnProtocolCommandPartConfiguration> commandPartsToMap);

		void FreeResources();
	}
}