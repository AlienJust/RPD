using RPD.Presentation.Contracts.Model;

namespace RPD.Presentation.Model {
	/// <summary>
	/// Хранит название локомотива и номер секции для устройства с известным номером DeviceNumber.
	/// </summary>
	class DeviceInfo : IDeviceInfo {
		public int DeviceNumber { get; set; }
		public int SectionNumber { get; set; }
		public string LocomotiveName { get; set; }
	}
}
