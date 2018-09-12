using System.Collections.Generic;

namespace DataAbstractionLevel.Low.PsnConfig.Contracts {
	public interface IPsnMergedDevice
	{
		string Name { get; }
		IEnumerable<IPsnMergedParameter> Parameters { get; }
	}
}