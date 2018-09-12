using System.Collections.Generic;
using System.Net;
using System.Net.FtpClient;
using DataAbstractionLevel.Low.Storage.FtpFilesStorage.Contracts;

namespace DataAbstractionLevel.Low.Storage.FtpFilesStorage {
	public class FtpFilesStorageSimple : IFtpFilesStorage {
		private readonly string _ftpHost;
		private readonly int _ftpPort;
		private readonly string _username;
		private readonly string _password;
		private readonly string _path;

		public FtpFilesStorageSimple(string ftpHost, int ftpPort, string username, string password, string path) {
			_ftpHost = ftpHost;
			_ftpPort = ftpPort;
			_username = username;
			_password = password;
			_path = path;
		}

		public IEnumerable<IFtpFileInfo> Items {
			get {
				var result = new List<IFtpFileInfo>();
				using (var conn = new FtpClient())
				{
					conn.Host = _ftpHost;
					conn.Port = _ftpPort;
					conn.Credentials = new NetworkCredential(_username, _password);
					var items = conn.GetListing(_path);
					foreach (var ftpListItem in items)
					{
						try
						{
							if (ftpListItem.Type == FtpFileSystemObjectType.File)
							{
								result.Add(new FtpFileInfo(ftpListItem.Name, ftpListItem.FullName));
							}
						}
						catch /*(Exception ex)*/ {
							continue;
						}
					}
				}
				return result;
			}
		}

		public override string ToString()
		{
			return "ftpfilesstorage@" + _ftpHost + ":" + _ftpPort;
		}

		public string FtpHost {
			get { return _ftpHost; }
		}

		public int FtpPort {
			get { return _ftpPort; }
		}

		public string Username {
			get { return _username; }
		}

		public string Password {
			get { return _password; }
		}

		public string Path {
			get { return _path; }
		}
	}
}