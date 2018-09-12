namespace RPD.DAL.Common.SimpleRelease {
	internal class UidString : IUid {
		private readonly string _id;

		public UidString(string id) {
			_id = id;
		}

		public string UnicString {
			get { return _id; }
		}

		public override string ToString() {
			return _id;
		}
	}
}