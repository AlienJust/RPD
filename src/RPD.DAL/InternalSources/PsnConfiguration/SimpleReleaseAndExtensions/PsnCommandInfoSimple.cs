namespace RPD.DAL.PsnConfiguration.SimpleReleaseAndExtensions {
	class PsnCommandInfoSimple : IPsnCommandInfo {
		private readonly string _name;
		private readonly IUid _id;
		public PsnCommandInfoSimple(string name, IUid id) {
			_name = name;
			_id = id;
		}

		public string Name {
			get { return _name; }
		}

		public IUid Id {
			get { return _id; }
		}
	}
}