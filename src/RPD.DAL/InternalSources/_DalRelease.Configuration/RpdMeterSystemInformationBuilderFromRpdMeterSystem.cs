using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Common;
using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts;
using RPD.DAL;
using RPD.DalRelease.Configuration.System.Contracts;

namespace RPD.DalRelease.Configuration.System {
	internal class RpdMeterSystemInformationBuilderFromRpdMeterSystem : IRpdMeterSystemInformationBuilder {
		private const int MaxChannels = 16;
		private readonly IRpdMeter _meter;

		public RpdMeterSystemInformationBuilderFromRpdMeterSystem(IRpdMeter meter) {
			_meter = meter;
		}
		public IRpdMeterSystemInformation Build() {
			var rpdMeterInfo = new RpdMeterSystemInformationSimple {Address = (byte) _meter.Address};
			switch (_meter.Type)
			{
				case RpdMeterType.Uran:
					rpdMeterInfo.Type = 0;
					break;
				case RpdMeterType.Irvi:
					rpdMeterInfo.Type = 1;
					break;
				case RpdMeterType.Radar:
					rpdMeterInfo.Type = 2;
					break;
			}
			rpdMeterInfo.Status = 0;
			rpdMeterInfo.LinkErrorCounter = 0;
			rpdMeterInfo.ChannelsCount = 0;
			rpdMeterInfo.ChannelsMask = 0;
			rpdMeterInfo.ChannelsMaskFromMeter = 0;

			rpdMeterInfo.ChannelsDumpRulesCodes = new byte[MaxChannels];

			for (int i = 0; i < _meter.Channels.Count; ++i)//в этом цикле считаем маску разрешенных каналов и номера правил из таблицы правил регистрации и контроля
			{
				if (_meter.Channels[i].IsEnabled)
				{
					rpdMeterInfo.ChannelsCount++; // TODO: как быть со служебными каналами?
					rpdMeterInfo.ChannelsMask += (ushort)(0x01 << i);// Math.Pow(2, i); //если канал разрешен - заряжаем единицу в маске :)
				}
				//выставляем номер условия канала, если оно используются для разрешенного канала:
				if (_meter.Channels[i].IsEnabled)
				{

					if (_meter.Channels[i].DumpCondition != null && _meter.Channels[i].DumpCondition.IsUsed)
						rpdMeterInfo.ChannelsDumpRulesCodes[i] = (byte)(_meter.Channels[i].DumpCondition.ConnectionPointIndex);
					else
						rpdMeterInfo.ChannelsDumpRulesCodes[i] = 0; //а если не используется - пишем 0
				}
				else
				{
					rpdMeterInfo.ChannelsDumpRulesCodes[i] = 0;
				}
			}
			rpdMeterInfo.ChannelsDumpedCount = rpdMeterInfo.ChannelsCount;

			rpdMeterInfo.HigherReadedFaultNumber = 0;
			rpdMeterInfo.ReadedFaultsCount = 0;
			rpdMeterInfo.NumberOfFaultDumpsForMeter = 0;
			rpdMeterInfo.PageLine = 0;
			rpdMeterInfo.PageLinesCountPerFault = 0;
			rpdMeterInfo.Crc = 0;
			return rpdMeterInfo;
		}
	}
}