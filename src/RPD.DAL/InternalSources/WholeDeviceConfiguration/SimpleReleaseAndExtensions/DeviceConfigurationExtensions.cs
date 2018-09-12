namespace RPD.DAL.WholeDeviceConfiguration.SimpleReleaseAndExtensions {
	internal static class DeviceConfigurationExtensions {
		public static bool IsEqualTo(this IDeviceConfiguration config1, IDeviceConfiguration config2) {
			if (config1.Address != config2.Address) return false;
			if (config1.Comment != config2.Comment) return false;
			if (!config1.Diagnostic.IsEqualTo(config2.Diagnostic)) return false;
			if (config1.LocomotiveName != config2.LocomotiveName) return false;
			if (config1.LogPsn != config2.LogPsn) return false;
			if (config1.NetAddress != config2.NetAddress) return false;

			if (config1.RpdMeters.Count != config2.RpdMeters.Count) return false;
			for (int i = 0; i < config1.RpdMeters.Count; ++i) {
				if (!config1.RpdMeters[i].IsEqualTo(config2.RpdMeters[i])) {
					return false;
				}
			}
			if (config1.ResetFaultsDump != config2.ResetFaultsDump) return false;
			if (config1.SaveByteInterval != config2.SaveByteInterval) return false;
			if (config1.SectionNumber != config2.SectionNumber) return false;
			if (config1.UseHammingForNand != config2.SaveByteInterval) return false;
			return true;
		}
	}
}