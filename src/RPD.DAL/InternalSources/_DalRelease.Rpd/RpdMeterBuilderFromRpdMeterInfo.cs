using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts;
using RPD.DAL;
using RPD.DalRelease.Rpd.Contracts;

namespace RPD.DalRelease.Rpd {
	internal class RpdMeterBuilderFromRpdMeterInfo : IRpdMeterBuilder {
		private readonly IRpdMeterSystemInformation _rpdMeterSystemInformation;
		private readonly string _rpdMeterName;
		public RpdMeterBuilderFromRpdMeterInfo(IRpdMeterSystemInformation rpdMeterSystemInformation, string rpdMeterName) {
			_rpdMeterSystemInformation = rpdMeterSystemInformation;
			_rpdMeterName = rpdMeterName;
		}
		public RpdMeter Build() {
			var meter = new RpdMeter(null, _rpdMeterSystemInformation.Address, _rpdMeterName);
			switch (_rpdMeterSystemInformation.Type)//при изменении типа, класс RpdMeter автоматически генерирует новый список каналов с определенными DumpConditions (ConnectionPointIndex = 0)
			{
				case 0:
					meter.Type = RpdMeterType.Uran;
					break;
				case 1:
					meter.Type = RpdMeterType.Irvi;
					break;
				case 2:
					meter.Type = RpdMeterType.Radar;
					break;
			}
			for (int i = 0; i < meter.Channels.Count; ++i)
			{
				meter.Channels[i].IsEnabled = ((_rpdMeterSystemInformation.ChannelsMask & (0x01 << i)) != 0x00); // По маске каналов определяется разрешён ли канал
				if (_rpdMeterSystemInformation.ChannelsDumpRulesCodes[i] != 0)
				{
					meter.Channels[i].DumpCondition.IsUsed = true;
					meter.Channels[i].DumpCondition.ConnectionPointIndex = _rpdMeterSystemInformation.ChannelsDumpRulesCodes[i];
				}
			}
			return meter;
		}
	}
}