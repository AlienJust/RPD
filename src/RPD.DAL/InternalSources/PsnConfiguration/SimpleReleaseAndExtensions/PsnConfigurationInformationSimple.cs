using RPD.DAL.PsnConfiguration.Contracts.Internal;

namespace RPD.DAL.PsnConfiguration.SimpleReleaseAndExtensions {
	class PsnConfigurationInformationSimple : IPsnConfigurationInformation {
		private readonly IUid _id;
		private readonly string _name;
		private readonly string _description;
		private readonly string _version;
		private readonly string _rpdId;
		public PsnConfigurationInformationSimple(IUid id, string name, string description, string version, string rpdId) {
			_id = id;
			_name = name;
			_description = description;
			_version = version;
			_rpdId = rpdId;
		}

		public IUid Id {
			get { return _id; }
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

		public override string ToString() {
			return _rpdId + " > " + _name + " (" + _description + ") v." + _version + " [" + _id + "]";
		}
	}
}