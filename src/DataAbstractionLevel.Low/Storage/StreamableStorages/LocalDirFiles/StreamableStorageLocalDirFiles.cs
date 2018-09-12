using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AlienJust.Support.Identy;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.StreamableData.Contracts;
using DataAbstractionLevel.Low.Storage.StreamableData.SingleFile;
using DataAbstractionLevel.Low.Storage.StreamableStorages.Contracts;

namespace DataAbstractionLevel.Low.Storage.StreamableStorages.LocalDirFiles
{
	class StreamableStorageLocalDirFiles : IStreamableDataStorage {
		private readonly string _directoryPath;
		private readonly string _fileMask;

		public StreamableStorageLocalDirFiles(string directoryPath, string fileMask) {
			_directoryPath = directoryPath;
			_fileMask = fileMask;
		}

		public IEnumerable<IStreamableDataWithId> Datas
		{
			get {
				return new DirectoryInfo(_fileMask).
					GetFiles("PSN.*.data.bin").
					Select(fi => (IStreamableDataWithId) new StreamableDataWithIdSimple(new IdentifierStringToLowerBased(fi.Name), new StreamedFile(fi.FullName)));
			}
		}

		public void Add(IIdentifier id, IStreamedData data) {
			var filename = Path.Combine(_directoryPath, id.IdentyString);
			if (File.Exists(filename))
			{
				throw new Exception("File allready exists " + filename);
			}
			using (var dst = File.Create(filename)) {
				using (var src = data.GetStreamForReading()) {
					src.CopyTo(dst);
				}
			}
		}

		public void Remove(IIdentifier id) {
			var filename = Path.Combine(_directoryPath, id.IdentyString);
			if (File.Exists(filename))
			{
				File.Delete(filename);
			}
			else throw new Exception("Cannot find file " + filename);
		}
	}
}
