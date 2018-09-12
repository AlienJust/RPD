namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts {
	internal static class PsnDataFragmentInfoExtensions {
		public static bool IsEqualTo(this IPsnDataFragmentInformation info1, IPsnDataFragmentInformation info2)
		{
			if (info1.StartOffset != info2.StartOffset) return false;
			if (info1.StartTime != info2.StartTime) return false;
			return true;
		}
	}
}