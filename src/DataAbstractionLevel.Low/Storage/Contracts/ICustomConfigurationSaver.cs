namespace DataAbstractionLevel.Low.Storage.Contracts {
	public interface ICustomConfigurationSaver
	{
		void SaveConfiguration(ICustomConfiguration configuration);
	}
}