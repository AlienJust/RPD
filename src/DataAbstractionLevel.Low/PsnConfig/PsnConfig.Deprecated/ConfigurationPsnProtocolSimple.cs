namespace RPD.DAL.LowLevel.Storage.PsnConfig {
	class ConfigurationPsnProtocolSimple : IConfigurationPsnProtocol {
		private readonly IIdentifier _id;
		private readonly string _name;
		private readonly string _version;
		private readonly string _description;
		private readonly int _rpdId;


		public ConfigurationPsnProtocolSimple(IIdentifier id, string name, string version, string description, int rpdId)
		{
			_id = id;
			_name = name;
			_version = version;
			_description = description;
			_rpdId = rpdId;
		}

		public IIdentifier Id
		{
			get { return _id; }
		}

		public string Name {
			get { return _name; }
		}

		public string Version {
			get { return _version; }
		}

		public string Description {
			get { return _description; }
		}

		public int RpdId {
			get { return _rpdId; }
		}
	}
}