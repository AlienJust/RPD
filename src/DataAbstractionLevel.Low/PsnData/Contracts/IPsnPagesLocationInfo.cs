using System;

namespace DataAbstractionLevel.Low.PsnData.Contracts {
	internal interface IPsnPagesLocationInfo {
		IInterval<int> PagesInterval { get; }
		FragmentSplitInfo SplitInfo { get; }

		IInterval<DateTime?> TimesInterval { get; }
	}
}