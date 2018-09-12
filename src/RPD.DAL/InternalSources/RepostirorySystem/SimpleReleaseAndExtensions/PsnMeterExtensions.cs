using System.Linq;

namespace RPD.DAL.RepostirorySystem.SimpleReleaseAndExtensions {
	internal static class PsnMeterExtensions {
		public static bool IsEqualTo(this IPsnMeter meter1, IPsnMeter meter2) {
			if (meter1.Name != meter2.Name) return false;
			if (meter1.Channels.Count != meter2.Channels.Count) return false;

			// below is a for () analog:
			if (meter1.Channels.Where((t, i) => !t.IsEqualTo(meter2.Channels[i])).Any()) {
				return false;
			}
			if (meter1.Name != meter2.Name) return false;
			return true;
		}
	}
}