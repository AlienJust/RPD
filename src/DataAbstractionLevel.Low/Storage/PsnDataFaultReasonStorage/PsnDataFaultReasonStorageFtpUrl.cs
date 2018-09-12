using System;
using System.Collections.Generic;
using System.Globalization;
using AlienJust.Support.Identy;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.Contracts;
using DataAbstractionLevel.Low.Storage.FtpFilesStorage.Contracts;
using DataAbstractionLevel.Low.Storage.PsnDataFaultReason;
using DataAbstractionLevel.Low.Storage.StreamableData;

namespace DataAbstractionLevel.Low.Storage.PsnDataFaultReasonStorage {
	public sealed class PsnDataFaultReasonStorageFtpUrl : IStorage<IPsnDataFaultReason> {
		private readonly IFtpFilesStorage _ftpFilesStorage;
		private readonly int _deviceNumber;
		
		private const string FtpFileExtension = ".log";

		public PsnDataFaultReasonStorageFtpUrl(IFtpFilesStorage ftpFilesStorage, int deviceNumber) {
			_ftpFilesStorage = ftpFilesStorage;
			_deviceNumber = deviceNumber;
		}

		public override string ToString() {
			return "PsnDataFaultReasonStorageFtpUrl@" + _deviceNumber + "@" + _ftpFilesStorage;
		}

		public void Add(IPsnDataFaultReason item) {
			throw new Exception("ќпераци€ добавлени€ данных не поддерживаетс€ данным хранилищем");
		}

		public void Remove(IPsnDataFaultReason item) {
			throw new Exception("Ќельз€ удал€ть данные из данного хранилища");
		}

		public void Update(IIdentifier id, IPsnDataFaultReason item) {
			throw new NotImplementedException();
		}

		public IEnumerable<IPsnDataFaultReason> StoredItems {
			 get {
				 var result = new List<IPsnDataFaultReason>();
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
										new PsnDataFaultReasonStorageFtpStream(
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
	}
}