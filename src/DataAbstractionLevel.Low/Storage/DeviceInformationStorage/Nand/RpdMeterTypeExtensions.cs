using DataAbstractionLevel.Low.Storage.Contracts;

namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand {
	internal static class RpdMeterTypeExtensions {
		public static string ToXmlString(this RpdProtocolMeterType t) {
			switch (t) {
				case RpdProtocolMeterType.Radar:
					return "RADAR";
				case RpdProtocolMeterType.Uran:
					return "URAN";
				case RpdProtocolMeterType.Irvi:
					return "IRVI";
				default:
					return "Unknown";
			}
		}
	}
}