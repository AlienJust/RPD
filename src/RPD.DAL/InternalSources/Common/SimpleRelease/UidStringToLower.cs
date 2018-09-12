namespace RPD.DAL.Common.SimpleRelease {
	internal class UidStringToLower : IUid {
		private readonly string _id;

		public UidStringToLower(string id) {
			_id = id.ToLower();
		}

		public string UnicString {
			get { return _id; }
		}

		public override string ToString() {
			return _id;
		}
	}
}