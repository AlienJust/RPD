namespace RPD.DAL {
	internal static class PsnParameterExtensions
	{
		public static bool IsEqualTo(this IPsnParameterConfiguration param1, IPsnParameterConfiguration param2)
		{
			if (param1.IsBitSignal != param2.IsBitSignal) return false;
			if (param1.Name != param2.Name) return false;
			return true;
		}
	}
}