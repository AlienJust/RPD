using System;

namespace DataAbstractionLevel.Low.PsnData {
	internal sealed class SimplePsnPagesLocationInfo : IPsnPagesLocationInfo {
		public IInterval<int> PagesInterval { get; set; }
		public FragmentSplitInfo SplitInfo { get; set; }

		public IInterval<DateTime?> TimesInterval { get; set; }
	}
}