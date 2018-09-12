using System;

namespace RPD.DAL.PsnConfiguration.SimpleReleaseAndExtensions {
	class PsnChannelConfigSimple: IPsnChannelConfig {
		private readonly Guid _id;
		private readonly string _name;
		public PsnChannelConfigSimple(Guid id, string name) {
			_id = id;
			_name = name;
		}

		public Guid Id {
			get { return _id; }
		}

		public string Name {
			get { return _name; }
		}
	}
}