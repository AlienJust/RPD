using System.Collections.Generic;
using AlienJust.Support.Identy.Contracts;

namespace DataAbstractionLevel.Low.PsnConfig.Contracts {
	public interface IPsnMergedParameter : IObjectWithIdentifier {
		string Name { get; }
		string Expression { get; }
		IEnumerable<IPsnMergedParameterPart> Parts { get; }
		bool IsMsIntegrated { get; }
	}
}