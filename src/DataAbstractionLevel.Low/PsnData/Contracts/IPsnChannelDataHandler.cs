using System;
using System.Collections.Generic;

namespace DataAbstractionLevel.Low.PsnData.Contracts {
	internal interface IPsnChannelDataHandler {
		List<DataPoint> GetDataPoints(DateTime beginTime);

		/// <summary>
		/// Another way to load points, each comes in callback
		/// </summary>
		/// <param name="beginTime">Trend begin time</param>
		/// <param name="nextPointLoaded">Callback action, when another point was loaded</param>
		void GetDataPoints(DateTime beginTime, Action<DataPoint> nextPointLoaded);
	}
}