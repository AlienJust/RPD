using AlienJust.Support.Identy;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.Contracts;

namespace DataAbstractionLevel.Low.Storage.Shared {
	class RpdDataCustomConfigurationSimple : IRpdDataCustomConfigrationWritable
	{
		private readonly IIdentifier _id;
		private IIdentifier _rpdConfigurationId;
		private string _customLogName;

		public RpdDataCustomConfigurationSimple(IIdentifier id, IIdentifier rpdConfigurationId, string customLogName)
		{
			_id = new IdentifierStringToLowerBased(id.IdentyString);
			_rpdConfigurationId = new IdentifierStringToLowerBased(rpdConfigurationId.IdentyString);
			_customLogName = customLogName;
		}

		public IIdentifier Id
		{
			get { return _id; }
		}

		public IIdentifier RpdConfigurationId
		{
			get { return _rpdConfigurationId; }
		}

		public string CustomLogName
		{
			get { return _customLogName; }
		}

		public void SetCustomLogName(string customLogName)
		{
			_customLogName = customLogName;
		}

		public void SetRpdConfigurationId(IIdentifier rpdConfigurationId)
		{
			_rpdConfigurationId = rpdConfigurationId;
		}
	}
}