using System;
using System.Collections.Generic;
using DataAbstractionLevel.Low.PsnConfig.Contracts;
using RPD.DAL.Common.Contracts;
using RPD.DAL.PsnConfiguration.SimpleReleaseAndExtensions;

namespace RPD.DAL.PsnConfiguration.RelayOnLowLevel {
	internal class PsnProtocolConfigBuilderFromLowLevel : IBuilder<IPsnConfiguration> {
		private readonly IPsnProtocolConfiguration _psnInternalConfig;

		public PsnProtocolConfigBuilderFromLowLevel(IPsnProtocolConfiguration psnInternalConfig)
		{
			_psnInternalConfig = psnInternalConfig;
		}


		public IPsnConfiguration Build() {
			var meters = new List<IPsnMeterConfig>();
			foreach (var meterInfo in _psnInternalConfig.PsnDevices) {
				var channels = new List<IPsnChannelConfig>();
				foreach (var cmdPart in _psnInternalConfig.CommandParts) {
					if (cmdPart.Address != null)
					{
						if (Math.Abs(cmdPart.Address.DefinedValue - meterInfo.Address) < 0.0001) {
							foreach (var varParam in cmdPart.VarParams) {
								if (!varParam.Name.StartsWith("#")) {
									channels.Add(new PsnChannelConfigSimple(Guid.Parse(varParam.Id.IdentyString), varParam.Name));
								}
							}
						}
					}
				}
				meters.Add(new PsnMeterConfigSimple(meterInfo.Name, channels));
			}

			return new PsnConfigurationSimple(
				Guid.Parse(_psnInternalConfig.Information.Id.IdentyString)
				, int.Parse(_psnInternalConfig.Information.RpdId)
				,_psnInternalConfig.Information.Name
				, _psnInternalConfig.Information.Version
				, _psnInternalConfig.Information.Description
				, meters);
		}
	}
}