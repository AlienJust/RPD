using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.Contracts;
using DataAbstractionLevel.Low.Storage.Shared;

namespace DataAbstractionLevel.Low.Storage.PsnDataInformationStorage
{
	public class PsnDataInformationStorageFtpFiles : IPsnDataInformationStorage
	{
		private readonly string _directoryPath;
		private readonly string _deviceInformationIdForAllInformations;


		public PsnDataInformationStorageFtpFiles(string directoryPath, string deviceInformationIdForAllInformations) {
			_directoryPath = directoryPath;
			_deviceInformationIdForAllInformations = deviceInformationIdForAllInformations;
		}

		public IEnumerable<IPsnDataInformation> PsnDataInformations {
			get {
				var files = new DirectoryInfo(_directoryPath).GetFiles("AVR*.log");
//#if DEBUG
				//Console.WriteLine("Files in dir: " + files.Length);
//#endif
				var result = new List<IPsnDataInformation>();

				foreach (var fi in files) {
					try {
						var parts = fi.Name.Split(' ', '.');
						if (parts.Length == 5 || parts.Length == 7) {
							var timeStr = parts[3];
							var time = DateTime.ParseExact(timeStr, "ddMMyyHHmmss", CultureInfo.InvariantCulture); // begin or end time?
							//result.Add(new PsnDataInformationSimple(fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length - 1), time, null, null, PsnDataFragmentType.LinkedToFault, false, _deviceInformationIdForAllInformations));
							result.Add(new PsnDataInformationSimple(fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length - 1), time, null, null, PsnDataFragmentType.FixedLength, false, _deviceInformationIdForAllInformations));
						}
					}
					catch //(Exception ex)
					{
//#if DEBUG
						//Console.WriteLine(ex);
//#endif
						continue;
					}
				}
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
