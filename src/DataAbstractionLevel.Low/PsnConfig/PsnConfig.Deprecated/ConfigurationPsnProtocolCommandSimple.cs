namespace RPD.DAL.LowLevel.Storage.PsnConfig {
	class ConfigurationPsnProtocolCommandSimple : IConfigurationPsnProtocolCommand {
		private readonly IIdentifier _id;
		private readonly string _name;
		private readonly IIdentifier _idConfigurationPsnProtocol;
		public ConfigurationPsnProtocolCommandSimple(IIdentifier id, string name, IIdentifier idConfigurationPsnProtocol)
		{
			_id = id;
			_name = name;
			_idConfigurationPsnProtocol = idConfigurationPsnProtocol;
		}

		public IIdentifier Id
		{
			get { return _id; }
		}

		public string Name {
			get { return _name; }
		}

		public IIdentifier IdConfigurationPsnProtocol
		{
			get { return _idConfigurationPsnProtocol; }
		}
	}
}