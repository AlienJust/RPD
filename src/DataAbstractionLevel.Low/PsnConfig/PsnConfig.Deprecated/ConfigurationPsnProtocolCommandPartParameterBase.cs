using System.Collections.Generic;

namespace RPD.DAL.LowLevel.Storage.PsnConfig {
	abstract class ConfigurationPsnProtocolCommandPartParameterBase : IConfigurationPsnProtocolCommandPartParameter {
		private readonly IIdentifier _id;
		private readonly string _name;
		private readonly bool _isBitSignal;
		private readonly IIdentifier _idConfigurationPsnProtocolCommandPart;

		protected ConfigurationPsnProtocolCommandPartParameterBase(string id, string name, bool isBitSignal, IIdentifier idConfigurationPsnProtocolCommandPart)
		{
			_id = new IdString(id);
			_name = name;
			_isBitSignal = isBitSignal;
			_idConfigurationPsnProtocolCommandPart = idConfigurationPsnProtocolCommandPart;
		}

		public IIdentifier Id
		{
			get { return _id; }
		}

		public string Name {
			get { return _name; }
		}

		public bool IsBitSignal {
			get { return _isBitSignal; }
		}

		public abstract double GetValue(IList<byte> cmdPartContext, int startByte);

		public IIdentifier IdConfigurationPsnProtocolCommandPart
		{
			get { return _idConfigurationPsnProtocolCommandPart; }
		}
	}
}