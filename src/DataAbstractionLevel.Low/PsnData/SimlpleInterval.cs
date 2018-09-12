using DataAbstractionLevel.Low.PsnData.Contracts;

namespace DataAbstractionLevel.Low.PsnData {
	internal sealed class SimlpleInterval<T> : IInterval<T> {
		public T Begin { get; set; }
		public T End { get; set; }
	}
}