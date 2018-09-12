using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPD.DAL;

namespace RPD.InternalContracts
{
	interface IApproximator
	{
		List<IDataPoint> Approximate(List<IDataPoint> trend, DateTime startMoment, DateTime stopMoment, int suggestedPointsCount);
	}
}
