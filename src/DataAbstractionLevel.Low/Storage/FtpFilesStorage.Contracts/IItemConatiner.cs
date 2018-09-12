using System.Collections.Generic;

namespace DataAbstractionLevel.Low.Storage.FtpFilesStorage.Contracts {
	public interface IItemConatiner<out T> {
		IEnumerable<T> Items { get; }
	}
}