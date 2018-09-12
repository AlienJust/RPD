using System.Collections.Generic;
using DataAbstractionLevel.Low.Storage.Contracts;
using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand;
using RPD.DAL.RpdConfiguration;

namespace RPD.DAL.WholeDeviceConfiguration.RelayOnLowLevel {
	/*TODO: Actually this class is relay on high level, may be need to move it to another namespace*/
	internal class CustomConfigurationBuilderFromHighDeviceConfig : ICustomConfigurationBuilder {
		private readonly IDeviceConfiguration _devConf;

		public CustomConfigurationBuilderFromHighDeviceConfig(IDeviceConfiguration configuration) {
			_devConf = configuration;
		}

		public ICustomConfiguration BuildConfiguration() {
			var result =
				new CustomConfigurationSimple {
					LocomotiveName = _devConf.LocomotiveName,
					SectionNumber = _devConf.SectionNumber,
					PsnVersion = 11
				};

			var meterInfos = new List<IRpdMeterCustomInfo>();
			foreach (var meter in _devConf.RpdMeters) {
				var meterInfo = new RpdMeterCustomInfoSimple();
				meterInfo.Address = meter.Address;
				meterInfo.Name = meter.Name;
				meterInfo.MeterType = new RpdProtocolMeterTypeBuilderFromHighLevel(meter.Type).Build();
				var channelInfos = new List<IRpdChannelCustomInformation>();
				foreach (var rpdChannel in meter.Channels) {
					var channelInfo =
						new RpdChannelCustomInformationSimple {
							IsEnabled = rpdChannel.IsEnabled,
							IsService = rpdChannel.IsService,
							Name = rpdChannel.Name,
							Number = rpdChannel.Number
						};
					channelInfos.Add(channelInfo);
				}
				meterInfo.ChannelInfos = channelInfos;
				meterInfos.Add(meterInfo);
			}
			result.RpdMetersCustomInfos = meterInfos;
			return result;
		}
	}
}
