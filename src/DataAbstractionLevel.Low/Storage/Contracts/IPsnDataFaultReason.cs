using AlienJust.Support.Identy.Contracts;

namespace DataAbstractionLevel.Low.Storage.Contracts {
	public interface IPsnDataFaultReason : IObjectWithIdentifier {
		/// <summary>
		/// Причина аварии
		/// </summary>
		string FaultReason { get; }
	}
}