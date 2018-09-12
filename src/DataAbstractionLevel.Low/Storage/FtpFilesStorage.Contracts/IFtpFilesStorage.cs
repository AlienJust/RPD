namespace DataAbstractionLevel.Low.Storage.FtpFilesStorage.Contracts {
	public interface IFtpFilesStorage : IItemConatiner<IFtpFileInfo> {
		string FtpHost { get; }
		int FtpPort { get; }
		string Username { get; }
		string Password { get; }
		string Path { get; }
	}
}