using AlienJust.Support.Identy.Contracts;

namespace DataAbstractionLevel.Low.Storage.Contracts {
	public interface IPsnDataFaultReason : IObjectWithIdentifier {
		/// <summary>
		/// ������� ������
		/// </summary>
		string FaultReason { get; }
	}
}