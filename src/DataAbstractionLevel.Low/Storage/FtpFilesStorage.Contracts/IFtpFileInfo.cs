namespace DataAbstractionLevel.Low.Storage.FtpFilesStorage.Contracts {
	public interface IFtpFileInfo {
		string Name { get; }
		string FullName { get; }
	}
}