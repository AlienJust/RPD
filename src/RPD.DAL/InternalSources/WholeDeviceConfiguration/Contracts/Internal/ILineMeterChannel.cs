namespace RPD.DAL.WholeDeviceConfiguration.Contracts.Internal {
	internal interface ILineMeterChannel
	{
		int Number { get; }
		int Name { get; }
		bool IsEnabled { get; }
		bool IsService { get; }
	}
}