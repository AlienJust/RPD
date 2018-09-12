using AlienJust.Support.Identy.Contracts;

namespace DataAbstractionLevel.Low.PsnConfig.Contracts {
	public interface IPsnMergedParameterPart {
		IIdentifier RealParameterId { get; }
		string ExpressionName { get; }
	}
}