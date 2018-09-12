using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.PsnConfig.Contracts;
using DataAbstractionLevel.Low.Storage.Contracts;

namespace RPD.DAL.LibraryLoader.Contracts.Internal {
	interface IReposetoryOpenedResult
	{
		IPsnDataStorage PsnDataStorage { get; }
		IPsnDataInformationStorage PsnDataInformationStorage { get; }
		IDeviceInformationStorage DeviceInformationStorage { get; }
		IPsnDataCustomConfigurationsStorage PsnDataCustomConfigurationsesStorage { get; }
		IStorage<IPsnProtocolConfiguration> PsnConfigurationsStorage { get; }
	}
}