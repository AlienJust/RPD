using Dnv.Utils.Settings;

namespace RPD.Presentation.Settings {
	public interface IApplicationSettings : IApplicationSettingsBase {
		string DockLayotFileName { get; set; }
		string RepositoryPath { get; set; }
		bool IsRepositoryPathSetted { get; set; }
		bool UseMock { get; set; }
		bool IsDebugMode { get; set; }
		string DefaultDeviceConfigurationDir { get; set; }

		bool IsDefaultColorsGenerated { get; set; }

		/// <summary>
		/// Последняя папка, из которой добавляли данные.
		/// </summary>
		string LastAddDataFolder { get; set; }

		/// <summary>
		/// Последний файл из которого импортировали данные.
		/// </summary>
		string LastRpdDataImageFile { get; set; }

		//string LastSelectedPsnConfigurationUid { get; set; }

		/// <summary>
		/// Директория, в которую последний раз экспортировали данные.
		/// </summary>
		string LastExportFolder { get; set; }

		string LastSelectedFtpServerName { get; set; }

		string SelectionMasksFullFilePath { get; }

		string ColorsStorageFullFilePath { get; }

		string FtpDeviceInfoFullFilePath { get; }
		string DeviceNumberToPsnConfigFullFilePath { get; }

		string WorkspacesFullFilePath { get; }
	}

	class ApplicationSettings : ApplicationSettingsBase<SettingsPdo>, IApplicationSettings {
		public ApplicationSettings() : base("rpd.settings.json") {
		}

		#region Implementation of IApplicationSettings

		public string DockLayotFileName {
			get => SettingsPdo.DockLayotFileName;
			set => SettingsPdo.DockLayotFileName = value;
		}

		public string RepositoryPath {
			get => SettingsPdo.RepositoryPath;
			set => SettingsPdo.RepositoryPath = value;
		}

		public bool IsRepositoryPathSetted {
			get => SettingsPdo.IsRepositoryPathSetted;
			set => SettingsPdo.IsRepositoryPathSetted = value;
		}

		public bool UseMock {
			get => SettingsPdo.UseMock;
			set => SettingsPdo.UseMock = value;
		}

		public bool IsDebugMode {
			get => SettingsPdo.IsDebugMode;
			set => SettingsPdo.IsDebugMode = value;
		}

		public string DefaultDeviceConfigurationDir {
			get => SettingsPdo.DefaultDeviceConfigurationDir;
			set => SettingsPdo.DefaultDeviceConfigurationDir = value;
		}

		public bool IsDefaultColorsGenerated {
			get => SettingsPdo.IsDefaultColorsGenerated;
			set => SettingsPdo.IsDefaultColorsGenerated = value;
		}

		public string LastAddDataFolder {
			get => SettingsPdo.LastAddDataFolder;
			set => SettingsPdo.LastAddDataFolder = value;
		}

		public string LastRpdDataImageFile {
			get => SettingsPdo.LastRpdDataImageFile;
			set => SettingsPdo.LastRpdDataImageFile = value;
		}

		public string LastSelectedPsnConfigurationUid {
			get => SettingsPdo.LastSelectedPsnConfigurationUid;
			set => SettingsPdo.LastSelectedPsnConfigurationUid = value;
		}

		public string LastExportFolder {
			get => SettingsPdo.LastExportFolder;
			set => SettingsPdo.LastExportFolder = value;
		}

		public string LastSelectedFtpServerName {
			get => SettingsPdo.LastSelectedFtpServerName;
			set => SettingsPdo.LastSelectedFtpServerName = value;
		}

		public string SelectionMasksFullFilePath => LocalApplicationDataPath + "\\selection-masks.json";

		public string ColorsStorageFullFilePath => LocalApplicationDataPath + "\\trends-colors-byuid.json";

		public string FtpDeviceInfoFullFilePath => LocalApplicationDataPath + "\\ftp-devices-info.json";

		public string DeviceNumberToPsnConfigFullFilePath => LocalApplicationDataPath + "\\device-psnconfig.json";

		public string WorkspacesFullFilePath => LocalApplicationDataPath + "\\workspaces.json";

		#endregion
	}
}