namespace DataAbstractionLevel.Low.PsnData.Contracts {
	internal interface IInterval<out T> {
		T Begin { get; }
		T End { get; }
	}
}