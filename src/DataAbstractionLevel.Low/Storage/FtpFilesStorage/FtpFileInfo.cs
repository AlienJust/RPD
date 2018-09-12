using DataAbstractionLevel.Low.Storage.FtpFilesStorage.Contracts;

namespace DataAbstractionLevel.Low.Storage.FtpFilesStorage {
	internal class FtpFileInfo : IFtpFileInfo {
		private readonly string _name;
		private readonly string _fullName;

		public FtpFileInfo(string name, string fullName) {
			_name = name;
			_fullName = fullName;
		}

		public string Name {
			get { return _name; }
		}

		public string FullName {
			get { return _fullName; }
		}
	}
}