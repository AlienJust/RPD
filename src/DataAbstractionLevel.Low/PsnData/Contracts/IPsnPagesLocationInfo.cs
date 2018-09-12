using System;

namespace DataAbstractionLevel.Low.PsnData {
	internal interface IPsnPagesLocationInfo {
		IInterval<int> PagesInterval { get; }
		FragmentSplitInfo SplitInfo { get; }

		IInterval<DateTime?> TimesInterval { get; }
	}
}