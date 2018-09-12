
namespace RPD.Presentation.Settings {
	class SettingsPdo {
		public string LastSelectedFtpServerName { get; set; }
		public string LastExportFolder { get; set; }
		public string DockLayotFileName { get; set; }
		public string RepositoryPath { get; set; }
		public bool IsRepositoryPathSetted { get; set; }
		public bool UseMock { get; set; }
		public bool IsDebugMode { get; set; }
		public string DefaultDeviceConfigurationDir { get; set; }
		public string LastAddDataFolder { get; set; }
		public string LastRpdDataImageFile { get; set; }
		public string LastSelectedPsnConfigurationUid { get; set; }
		public bool IsDefaultColorsGenerated { get; set; }
	}
}
