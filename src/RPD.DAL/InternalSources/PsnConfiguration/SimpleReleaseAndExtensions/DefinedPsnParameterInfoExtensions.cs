namespace RPD.DAL.PsnConfiguration.SimpleReleaseAndExtensions {
	internal static class DefinedPsnParameterInfoExtensions
	{
		public static bool IsEqualTo(this IDefinedPsnParameterInfo info1, IDefinedPsnParameterInfo info2)
		{
			if (!((IPsnParameterConfiguration)info1).IsEqualTo(info2)) return false;
			if (info1.DefinedValue != info2.DefinedValue) return false;
			return true;
		}
	}
}