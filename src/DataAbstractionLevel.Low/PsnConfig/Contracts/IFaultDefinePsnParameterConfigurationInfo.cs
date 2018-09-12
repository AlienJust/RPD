namespace DataAbstractionLevel.Low.PsnConfig.Contracts {
	public interface IFaultDefinePsnParameterConfigurationInfo : IPsnProtocolParameterConfigurationVariable
	{
		/// <summary>
		/// Номер аварийного байта конфигурации
		/// </summary>
		int RpdConfPosByte { get; }

		/// <summary>
		/// Номер аварийного бита конфигурации
		/// </summary>
		int RpdConfPosBit { get; }
	}
}