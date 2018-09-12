namespace RPD.DAL.Common.SimpleRelease {
	class ObjectWithIdSimple : IObjectWithId {
		private readonly IUid _id;
		public ObjectWithIdSimple(IUid id)
		{
			_id = id;
		}

		public IUid Id {
			get { return _id; }
		}
	}
}