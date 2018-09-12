using System;
using System.Collections.Generic;
using DataAbstractionLevel.Low.PsnConfig.Contracts;

namespace DataAbstractionLevel.Low.PsnData.Contracts {
	public interface IPsnDataHandler {
		List<DataPoint> GetDataPoints(
			IPsnProtocolConfiguration protocol,
			IPsnProtocolCommandPartConfiguration commandPart,
			IPsnProtocolParameterConfiguration paramInfo,
			DateTime beginTime);

		void GetDataPoints(
			IPsnProtocolConfiguration protocol,
			IPsnProtocolCommandPartConfiguration commandPart,
			IPsnProtocolParameterConfiguration paramInfo,
			DateTime beginTime, 
			Action<DataPoint> nextPointLoaded);
	}
}