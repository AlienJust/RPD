namespace RPD.DAL {
	internal static class RpdMeterExtensions {
		public static bool IsEqualTo(this IRpdMeter meter1, IRpdMeter meter2) {
			if (meter1.Address != meter2.Address) return false;
			if (meter1.Channels.Count != meter2.Channels.Count) return false;
			for (int i = 0; i < meter1.Channels.Count; ++i) {
				if (!meter1.Channels[i].IsEqualTo(meter2.Channels[i])) return false;
			}
			if (meter1.Name != meter2.Name) return false;
			if (meter1.Type != meter2.Type) return false;


			return true;
		}
	}
}