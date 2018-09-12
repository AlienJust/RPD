using AlienJust.Support.Identy.Contracts;

namespace DataAbstractionLevel.Low.Storage.Contracts {
	internal interface IRpdDataCustomConfigrationWritable : IRpdDataCustomConfigration
	{
		void SetCustomLogName(string customLogName);
		void SetRpdConfigurationId(IIdentifier rpdConfigurationId);
	}
}