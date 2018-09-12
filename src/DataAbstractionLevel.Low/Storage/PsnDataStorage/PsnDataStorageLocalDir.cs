using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AlienJust.Support.Identy;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.InternalKitchen.Streamable;
using DataAbstractionLevel.Low.Storage.Contracts;
using DataAbstractionLevel.Low.Storage.PsnData;
using DataAbstractionLevel.Low.Storage.StreamableData.Contracts;
using DataAbstractionLevel.Low.Storage.StreamableData.SingleFile;

namespace DataAbstractionLevel.Low.Storage.PsnDataStorage {
	public sealed class StreamReadableDataStorageLocalDir : IPsnDataStorage {
		private readonly string _path;
		public StreamReadableDataStorageLocalDir(string path)
		{
			_path = path;
		}

		public IEnumerable<IPsnData> PsnDatas {
			get {
				return new DirectoryInfo(_path).
					GetFiles("PSN.*.data.bin").
					Select(
						fi =>
						new PsnDataRelayStream(
							new StreamedFile(fi.FullName),
							new IdentifierStringToLowerBased(fi.Name.Substring(4, fi.Name.Length - 4 - ".data.bin".Length)/*Split('.')[1]*/))).ToList();
			}
		}

		public IStreamedData Add(IIdentifier id, IStreamedData data, Action<double> progressChangeAction)
		{
			var saveFilename = Path.Combine(_path, "PSN." + id + ".data.bin");
			if (File.Exists(saveFilename)) throw new Exception("Лог с такими данными уже сохранен в репозитории");
			
			using (var s = data.GetStreamForReading()) {
				using (var d = File.Create(saveFilename)) {
					s.CopyToWithProgress(d, progressChangeAction);
				}
			}
			return new StreamedFile(saveFilename);
		}

		public void Remove(IIdentifier id)
		{
			File.Delete(Path.Combine(_path, "PSN." + id + ".data.bin"));
		}


		public override string ToString() {
			return "PsnDataStorageLocalDir@" + _path;
		}
	}
}