using System;
using System.Collections.Generic;
using System.Linq;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.Contracts;
using DataAbstractionLevel.Low.Storage.Shared;

namespace DataAbstractionLevel.Low.Storage.PsnDataInformationStorage
{
	public class PsnDataInformationStorageRelayMemoryCache : IPsnDataInformationStorage {
		private readonly IPsnDataInformationStorage _relayOnStorage;
		private readonly List<IPsnDataInformation> _psnDataInformations;

		public PsnDataInformationStorageRelayMemoryCache(IPsnDataInformationStorage relayOnStorage)
		{
			_relayOnStorage = relayOnStorage;
			_psnDataInformations = relayOnStorage.PsnDataInformations.ToList(); // TODO: check lazy way out
		}

		public IEnumerable<IPsnDataInformation> PsnDataInformations {
			get { return _psnDataInformations; }
		}

		public void Add(IIdentifier psnLogInformationId, DateTime? beginTime, DateTime? endTime, DateTime? saveTime, PsnDataFragmentType psnDataType, bool isLastDeviceLog, IIdentifier deviceInformationId)
		{
			_relayOnStorage.Add(psnLogInformationId, beginTime, endTime, saveTime, psnDataType, isLastDeviceLog, deviceInformationId);
			_psnDataInformations.Add(
				new PsnDataInformationSimple(
					psnLogInformationId.IdentyString,
					beginTime,
					endTime,
					saveTime,
					psnDataType,
					isLastDeviceLog,
					deviceInformationId.IdentyString));
		}

		public void Remove(IIdentifier psnLogInformationId)
		{
			_relayOnStorage.Remove(psnLogInformationId);

			var infosToRemove = _psnDataInformations.Where(i => i.Id.ToString() == psnLogInformationId.ToString()).ToList();
			foreach (var psnDataInformation in infosToRemove) {
				_psnDataInformations.Remove(psnDataInformation);
			}
		}
	}
}
