namespace DataAbstractionLevel.Low.PsnConfig.Contracts {
	internal static class DefinedPsnParameterInfoExtensions {
		public static bool IsEqualTo(this IPsnProtocolParameterDefinedConfiguration info1, IPsnProtocolParameterDefinedConfiguration info2)
		{
			if (!((IPsnProtocolParameterConfiguration)info1).IsEqualTo(info2)) return false;
			if (info1.DefinedValue != info2.DefinedValue) return false;
			return true;
		}
	}
}