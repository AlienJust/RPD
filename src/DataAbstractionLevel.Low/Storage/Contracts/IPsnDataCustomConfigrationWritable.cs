using AlienJust.Support.Identy.Contracts;

namespace DataAbstractionLevel.Low.Storage.Contracts {
	public interface IPsnDataCustomConfigrationWritable : IPsnDataCustomConfigration
	{
		void SetCustomLogName(string customLogName);
		void SetPsnConfigurationId(IIdentifier psnConfigurationId);
	}
}