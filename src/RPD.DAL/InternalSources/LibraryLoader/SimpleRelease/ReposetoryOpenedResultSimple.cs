using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.PsnConfig.Contracts;
using DataAbstractionLevel.Low.Storage.Contracts;
using RPD.DAL.LibraryLoader.Contracts.Internal;
using RPD.DAL.PsnConfiguration.Contracts.Internal;

namespace RPD.DAL.LibraryLoader.SimpleRelease {
	class ReposetoryOpenedResultSimple : IReposetoryOpenedResult
	{
		private readonly IPsnDataStorage _psnDataStorage;
		private readonly IDeviceInformationStorage _deviceInformationStorage;
		private readonly IPsnDataCustomConfigurationsStorage _psnDataCustomConfigurationsesStorage;
		private readonly IPsnDataInformationStorage _psnDataInformationStorage;
		private readonly IStorage<IPsnProtocolConfiguration> _psnConfigurationsStorage;

		public ReposetoryOpenedResultSimple(IPsnDataStorage psnDataStorage, IPsnDataInformationStorage psnDataInformationStorage, IDeviceInformationStorage deviceInformationStorage, IPsnDataCustomConfigurationsStorage psnDataCustomConfigurationsesStorage, IStorage<IPsnProtocolConfiguration> psnConfigurationsStorage)
		{
			_psnDataStorage = psnDataStorage;
			_psnDataInformationStorage = psnDataInformationStorage;
			_deviceInformationStorage = deviceInformationStorage;
			_psnDataCustomConfigurationsesStorage = psnDataCustomConfigurationsesStorage;
			_psnConfigurationsStorage = psnConfigurationsStorage;
		}

		public IPsnDataStorage PsnDataStorage
		{
			get { return _psnDataStorage; }
		}

		public IPsnDataInformationStorage PsnDataInformationStorage {
			get { return _psnDataInformationStorage; }
		}

		public IDeviceInformationStorage DeviceInformationStorage
		{
			get { return _deviceInformationStorage; }
		}

		public IPsnDataCustomConfigurationsStorage PsnDataCustomConfigurationsesStorage
		{
			get { return _psnDataCustomConfigurationsesStorage; }
		}

		public IStorage<IPsnProtocolConfiguration> PsnConfigurationsStorage
		{
			get { return _psnConfigurationsStorage; }
		}
	}
}