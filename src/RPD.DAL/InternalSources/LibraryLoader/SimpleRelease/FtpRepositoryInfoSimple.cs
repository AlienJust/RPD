namespace RPD.DAL.LibraryLoader.SimpleRelease {
	class FtpRepositoryInfoSimple : IFtpRepositoryInfo {
		public string FtpHost { get; set; }
		public int FtpPort { get; set; }
		public string FtpUsername { get; set; }
		public string FtpPassword { get; set; }
		public int DeviceNumber { get; set; }
	}
}