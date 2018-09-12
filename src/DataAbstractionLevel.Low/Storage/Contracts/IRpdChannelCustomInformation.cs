namespace DataAbstractionLevel.Low.Storage.Contracts {
	public interface IRpdChannelCustomInformation
	{
		string Name { get; set; }
		int Number { get; set; }
		bool IsEnabled { get; set; }
		bool IsService { get; set; }
	}
}