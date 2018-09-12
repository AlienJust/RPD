using System.Collections.Generic;
using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Common;
using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts;
using RPD.DalRelease.Configuration.System;

namespace RPD.DAL.WholeDeviceConfiguration.RelayOnLowLevel {
	/*TODO: Actually this class is relay on high level, may be need to move it to another namespace*/
	internal sealed class SystemConfigurationBuilderFromHighLevelDeviceConfiguration : ISystemConfigurationBuilder {
		private readonly IDeviceConfiguration _deviceConfiguration;
		private readonly int _rpdFaultsCount;

		public SystemConfigurationBuilderFromHighLevelDeviceConfiguration(IDeviceConfiguration deviceConfiguration, int rpdFaultsCount) {
			_deviceConfiguration = deviceConfiguration;
			_rpdFaultsCount = rpdFaultsCount;
		}

		public ISystemConfiguration BuildConfiguration() {
			var systemConfiguration = new SystemConfigurationSimple();
			systemConfiguration.ArrayDumpPsnStartPageNumber = _deviceConfiguration.Diagnostic.ArrayDumpPsnStartPageNumber;
			systemConfiguration.BadBlocksCount = _deviceConfiguration.Diagnostic.BadBlocksCount;

			var configurationByte = _deviceConfiguration.UseHammingForNand ? 0x01 : 0x00;
			if (_deviceConfiguration.LogPsn) configurationByte |= 0x02;
			if (_deviceConfiguration.SaveByteInterval) configurationByte |= 0x04;
			if (_deviceConfiguration.ResetFaultsDump) configurationByte |= 0x08;

			systemConfiguration.ConfigurationByte = configurationByte;
			systemConfiguration.CurrentPsnLogNumber = _deviceConfiguration.Diagnostic.CurrentPsnLogNumber;
			systemConfiguration.DeviceAddress = _deviceConfiguration.Address;
			// TODO: systemConfiguration.DumpRules = _deviceConfiguration.
			systemConfiguration.FatOffsetFromPageZero = _deviceConfiguration.Diagnostic.FatOffsetFromPageZero;
			systemConfiguration.FaultsCount = _rpdFaultsCount;

			// TODO: where to get faultsDumpTable? :0
			var faultsDumpTable = new List<IRpdDataInformation>();
			// TODO: foreach (var rpdLogInfo in _deviceConfiguration.?) {}
			systemConfiguration.FaultDumpsTable = faultsDumpTable;


			systemConfiguration.FirmwareVersion = _deviceConfiguration.Diagnostic.FirmwareVersion;
			systemConfiguration.FirstWrittenAfterResetPageNumber = _deviceConfiguration.Diagnostic.FirstWrittenAfterResetPageNumber;
			systemConfiguration.LastReadedBlockAddress = _deviceConfiguration.Diagnostic.LastReadedBlockAddress;
			systemConfiguration.LastWrittenPageAddress = _deviceConfiguration.Diagnostic.LastWrittenPageAddress;
			systemConfiguration.LastWrittenPageNumber = _deviceConfiguration.Diagnostic.LastWrittenPageNumber;

			systemConfiguration.MetersCount = _deviceConfiguration.RpdMeters.Count;
			
			var rpdMeterInfos = new List<IRpdMeterSystemInformation>();
			foreach (var rpdMeter in _deviceConfiguration.RpdMeters) {
				var builder = new RpdMeterSystemInformationBuilderFromRpdMeterSystem(rpdMeter);
				var rpdMeterInfo = builder.Build();
				rpdMeterInfos.Add(rpdMeterInfo);
			}
			systemConfiguration.MetersTable = rpdMeterInfos;

			systemConfiguration.NetAddress = _deviceConfiguration.NetAddress;
			systemConfiguration.PsnLogStartPageNumber = _deviceConfiguration.Diagnostic.PsnLogStartPageNumber;

			// TODO: think about:
			systemConfiguration.PsnLogPowerUpFragmentInfos = new List<IPsnDataFragmentInformation>();
			systemConfiguration.PsnLogPredefinedFragmentInfos = new List<IPsnDataFragmentInformation>();
			systemConfiguration.PsnRegisterStatusMasks = new byte[21]; // TODO: 21 - это максимальное число обусловленное бинарным форматом!
			
			systemConfiguration.RpdPagesCountTransmittedToPsnLog = _deviceConfiguration.Diagnostic.RpdPagesCountTransmittedToPsnLog;

			return systemConfiguration;
		}
	}
}