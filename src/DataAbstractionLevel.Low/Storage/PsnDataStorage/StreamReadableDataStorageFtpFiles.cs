using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AlienJust.Support.Identy;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.Contracts;
using DataAbstractionLevel.Low.Storage.PsnData;
using DataAbstractionLevel.Low.Storage.StreamableData.Contracts;
using DataAbstractionLevel.Low.Storage.StreamableData.SingleFile;

namespace DataAbstractionLevel.Low.Storage.PsnDataStorage {
	public sealed class StreamReadableDataStorageFtpFiles : IPsnDataStorage {
		private readonly string _path;
		public StreamReadableDataStorageFtpFiles(string path)
		{
			_path = path;
		}

		public IEnumerable<IPsnData> PsnDatas {
			get {
				return new DirectoryInfo(_path).
					GetFiles("AVR*.log").
					Select(
						fi =>
						new PsnDataRelayStream(
							new StreamedFile(fi.FullName),
							new IdentifierStringToLowerBased(fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length - 1)))).ToList();
			}
		}

		public IStreamedData Add(IIdentifier id, IStreamedData data, Action<double> progressChangeAction)
		{
			throw new Exception("ќпераци€ добавлени€ данных не поддерживаетс€ данным хранилищем");
		}

		public void Remove(IIdentifier id)
		{
			//File.Delete(Path.Combine(_path, id.ToString()));
			throw new Exception("Ќельз€ удал€ть данные из данного хранилища");
		}


		public override string ToString() {
			return "PsnDataStorageFtpFiles@" + _path;
		}
	}
}