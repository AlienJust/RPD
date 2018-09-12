namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts {
	static class InterfaceRpdMeterInfoExtensions {
		public static byte CalculateCrc(this IRpdMeterSystemInformation rpdMeterSystemInformation) {
			int crc1 = ((rpdMeterSystemInformation.Address + rpdMeterSystemInformation.Type + (rpdMeterSystemInformation.ChannelsMask & 0x00FF) + ((rpdMeterSystemInformation.ChannelsMask & 0xFF00) >> 8) + rpdMeterSystemInformation.ChannelsDumpedCount) & 0x000000FF);
			var crc2 = (byte)~crc1;
			return crc2;
		}
	}
}