using System;
using System.Collections.Generic;
using System.Linq;
using AlienJust.Support.Identy;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.Contracts;
using DataAbstractionLevel.Low.Storage.PsnData;
using DataAbstractionLevel.Low.Storage.StreamableData;
using DataAbstractionLevel.Low.Storage.StreamableData.Contracts;
using Ionic.Zip;
using Ionic.Zlib;

namespace DataAbstractionLevel.Low.Storage.PsnDataStorage {
	public sealed class StreamReadableDataStorageZip : IPsnDataStorage {
		private readonly string _zipFilename;

		public StreamReadableDataStorageZip(string zipFilename)
		{
			_zipFilename = zipFilename;
		}

		public IEnumerable<IPsnData> PsnDatas {
			get {
				using (var zipFile = CreateZipFileInstance(_zipFilename))
				{

					var dataEnries = zipFile.Entries.Where(ze => ze.FileName.StartsWith("PSN.") && ze.FileName.EndsWith("data.bin"));
					return dataEnries.Select(
						de =>
						new PsnDataRelayStream(
							new StreamedZippedFile(_zipFilename, de.FileName),
							new IdentifierStringToLowerBased(de.FileName.Substring(4, de.FileName.Length - 4 - ".data.bin".Length)))
						).ToList();
				}
			}
		}

		public IStreamedData Add(IIdentifier id, IStreamedData sourceData, Action<double> progressChangeAction)
		{
			progressChangeAction(0.0);
			using (var zipFile = CreateZipFileInstance(_zipFilename))
			{
				var saveFilename = "PSN." + id + ".data.bin";
				if (zipFile.ContainsEntry(saveFilename)) throw new Exception("Ћог с такими данными уже сохранен в репозитории");
				progressChangeAction(25.0);
				using (var stream = sourceData.GetStreamForReading()) {
					progressChangeAction(50.0);
					zipFile.UpdateEntry(saveFilename, stream);
					progressChangeAction(75.0);
					zipFile.Save(); // Ќевозможно использовать вне блока using
				}
				progressChangeAction(100.0);
				return new PsnDataRelayStream(new StreamedZippedFile(_zipFilename, saveFilename), id);
			}
		}

		public void Remove(IIdentifier id)
		{
			using (var zipFile = CreateZipFileInstance(_zipFilename))
			{
				zipFile.RemoveEntry("PSN." + id + ".data.bin");
				zipFile.Save();
			}
		}

		public override string ToString()
		{
			return "ZippedDataStorage@" + _zipFilename;
		}

		private ZipFile CreateZipFileInstance(string filename)
		{
			return new ZipFile(filename) {CompressionMethod = CompressionMethod.BZip2, CompressionLevel = CompressionLevel.BestCompression};
		}
	}
}