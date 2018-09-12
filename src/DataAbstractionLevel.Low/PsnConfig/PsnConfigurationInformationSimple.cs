using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.PsnConfig.Contracts;

namespace DataAbstractionLevel.Low.PsnConfig {
	class PsnConfigurationInformationSimple : IPsnProtocolConfigurationInformtaion {
		private readonly string _name;
		private readonly string _description;
		private readonly string _version;
		private readonly IIdentifier _id;
		private readonly string _rpdId;


		public PsnConfigurationInformationSimple(string name, string description, string version, IIdentifier id, string rpdId)
		{
			_name = name;
			_description = description;
			_version = version;
			_id = id;
			_rpdId = rpdId;
		}

		public string Name {
			get { return _name; }
		}

		public string Description {
			get { return _description; }
		}

		public string Version {
			get { return _version; }
		}

		public string RpdId {
			get { return _rpdId; }
		}

		public IIdentifier Id
		{
			get { return _id; }
		}

		public override string ToString() {
			return _rpdId + " > " + _name + " (" + _description + ") v." + _version + " [" + _id + "]";
		}
	}
}