namespace DataAbstractionLevel.Low.PsnData {
	internal interface IInterval<out T> {
		T Begin { get; }
		T End { get; }
	}
}