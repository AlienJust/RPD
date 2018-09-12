using System;
using System.Collections.Generic;
using System.Linq;
using DataAbstractionLevel.Low.Storage.FtpFilesStorage.Contracts;

namespace DataAbstractionLevel.Low.Storage.FtpFilesStorage {
	public class FtpFilesStorageLazyCached : IFtpFilesStorage {
		private readonly Lazy<List<IFtpFileInfo>> _items;

		private readonly IFtpFilesStorage _originalStorage;
		public FtpFilesStorageLazyCached(IFtpFilesStorage originalStorage) {
			_originalStorage = originalStorage;
			_items = new Lazy<List<IFtpFileInfo>>(InitItems);
		}

		private List<IFtpFileInfo> InitItems()
		{
			return _originalStorage.Items.ToList();
		}

		public IEnumerable<IFtpFileInfo> Items {
			get { return _items.Value; }
		}

		public override string ToString()
		{
			return "lazycached@" + _originalStorage;
		}

		public string FtpHost {
			get { return _originalStorage.FtpHost; }
		}

		public int FtpPort {
			get { return _originalStorage.FtpPort; }
		}

		public string Username {
			get { return _originalStorage.Username; }
		}

		public string Password {
			get { return _originalStorage.Password; }
		}

		public string Path {
			get { return _originalStorage.Path; }
		}
	}
}