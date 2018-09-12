using System;
using System.Collections.Generic;
using System.Globalization;
using AlienJust.Support.Identy;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.Contracts;
using DataAbstractionLevel.Low.Storage.FtpFilesStorage.Contracts;
using DataAbstractionLevel.Low.Storage.PsnData;
using DataAbstractionLevel.Low.Storage.StreamableData;
using DataAbstractionLevel.Low.Storage.StreamableData.Contracts;
using DataAbstractionLevel.Low.Storage.StreamableData.FtpFile;

namespace DataAbstractionLevel.Low.Storage.PsnDataStorage {
	public sealed class StreamReadableDataStorageFtpUrl : IPsnDataStorage {
		private readonly IFtpFilesStorage _ftpFilesStorage;
		private readonly int _deviceNumber;
		private const string FtpFileExtension = ".log";

		public StreamReadableDataStorageFtpUrl(IFtpFilesStorage ftpFilesStorage, int deviceNumber) {
			_ftpFilesStorage = ftpFilesStorage;
			_deviceNumber = deviceNumber;
		}

		public IEnumerable<IPsnData> PsnDatas {
			get {
				var result = new List<IPsnData>();
					foreach (var ftpListItem in _ftpFilesStorage.Items)
					{
						try {
							if (ftpListItem.Name.StartsWith("AVR") && ftpListItem.Name.EndsWith(FtpFileExtension)) {
								var parts = ftpListItem.Name.Split(' ', '.');
								if (parts.Length == 5 || parts.Length == 8) {
									if (int.Parse(parts[1]) != _deviceNumber) continue;
									var timeStr = parts[3];
									var time = DateTime.ParseExact(timeStr, "ddMMyyHHmmss", CultureInfo.InvariantCulture); // begin or end time?
									result.Add(
										new PsnDataRelayStream(
											new StreamedFtpFile(_ftpFilesStorage.FtpHost, _ftpFilesStorage.FtpPort, _ftpFilesStorage.Username, _ftpFilesStorage.Password, ftpListItem.FullName),
											new IdentifierStringToLowerBased(ftpListItem.Name.Substring(0, ftpListItem.Name.Length - FtpFileExtension.Length))));
								}
							}
						}
						catch /*(Exception ex)*/ {
							continue;
						}
					}
				
				return result;
			}
		}

		public IStreamedData Add(IIdentifier id, IStreamedData data, Action<double> progressChangeAction)
		{
			throw new Exception("ќпераци€ добавлени€ данных не поддерживаетс€ данным хранилищем");
		}

		public void Remove(IIdentifier id)
		{
			//File.Delete(Path.Combine(_ftpHost, id.ToString()));
			throw new Exception("Ќельз€ удал€ть данные из данного хранилища");
		}

		public override string ToString() {
			return "PsnDataStorageFtpUrl@" + _deviceNumber + "@" + _ftpFilesStorage;
		}
	}
}