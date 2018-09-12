namespace DataAbstractionLevel.Low.PsnConfig.Contracts {
	internal static class PsnProtocolParameterConfigurationExtensions
	{
		public static bool IsEqualTo(this IPsnProtocolParameterConfiguration param1, IPsnProtocolParameterConfiguration param2)
		{
			if (param1.IsBitSignal != param2.IsBitSignal) return false;
			if (param1.Name != param2.Name) return false;
			return true;
		}
	}
}