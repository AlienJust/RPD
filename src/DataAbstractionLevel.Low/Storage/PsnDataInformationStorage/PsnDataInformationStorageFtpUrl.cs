using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.FtpClient;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.Contracts;
using DataAbstractionLevel.Low.Storage.FtpFilesStorage.Contracts;
using DataAbstractionLevel.Low.Storage.Shared;

namespace DataAbstractionLevel.Low.Storage.PsnDataInformationStorage
{
	public class PsnDataInformationStorageFtpUrl : IPsnDataInformationStorage
	{
		private readonly IFtpFilesStorage _ftpFilesStorage;
		private readonly int _deviceNumber;
		private readonly string _deviceInformationIdForAllInformations;


		public PsnDataInformationStorageFtpUrl(IFtpFilesStorage ftpFilesStorage, int deviceNumber, string deviceInformationIdForAllInformations) {
			_ftpFilesStorage = ftpFilesStorage;
			_deviceNumber = deviceNumber;

			_deviceInformationIdForAllInformations = deviceInformationIdForAllInformations;
		}

		public IEnumerable<IPsnDataInformation> PsnDataInformations {
			get {
				var result = new List<IPsnDataInformation>();
				Console.WriteLine("Connecting");
				Console.WriteLine("Connected!");
				foreach (var ftpListItem in _ftpFilesStorage.Items) {
					try {

						//Console.WriteLine(ftpListItem.Name);
						//Console.WriteLine(ftpListItem.FullName);
						if (ftpListItem.Name.StartsWith("AVR") && ftpListItem.Name.EndsWith(".log")) {
							var parts = ftpListItem.Name.Split(' ', '.');
							if (parts.Length == 5 || parts.Length == 8) {
								if (int.Parse(parts[1]) != _deviceNumber) continue;
								var timeStr = parts[3];
								var time = DateTime.ParseExact(timeStr, "ddMMyyHHmmss", CultureInfo.InvariantCulture); // begin or end time?
								//result.Add(new PsnDataInformationSimple(ftpListItem.Name.Substring(0, ftpListItem.Name.Length - "log".Length - 1), time, null, null, PsnDataFragmentType.FixedLength, false, _deviceInformationIdForAllInformations));
								result.Add(new PsnDataInformationSimple(ftpListItem.Name.Substring(0, ftpListItem.Name.Length - "log".Length - 1), time, null, null, PsnDataFragmentType.FixedLength, false, _deviceInformationIdForAllInformations));
							}
						}

					}
					catch /*(Exception ex)*/ {
						continue;
					}
				}

				Console.WriteLine("PsnDataInformations taken");
				return result;
			}
		}

		public void Add(IIdentifier psnLogInformationId, DateTime? beginTime, DateTime? endTime, DateTime? saveTime, PsnDataFragmentType psnDataType, bool isLastDeviceLog, IIdentifier deviceInformationId)
		{
			throw new NotImplementedException();
		}

		public void Remove(IIdentifier psnLogInformationId)
		{
			throw new NotImplementedException(); // TODO: on beeing implemented will also remove psnData
		}
	}
}
