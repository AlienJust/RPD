using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.PsnData.Contracts;

namespace DataAbstractionLevel.Low.Storage.StreamableStorages.SingleFile {
	internal interface IIntervalWithId<out T> : IObjectWithIdentifier {
		IInterval<T> Interval { get; }
	}
}