using System;
using System.Collections.Generic;
using System.IO;
using AlienJust.Support.IO;
using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Common;
using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts;

namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.ReleaseNand {
	public sealed class SystemConfigurationBuilderFromBinaryFile : ISystemConfigurationBuilder {
		private const int FileSize = 106496;
		private const int PsnPredefinedFragmentStartInfosMaxCount = 100;
		private const int PsnLogPowerOnStartPagesMaxCount = 40;
		private const int FaultLogTableRecordsMax = 100;
		private const int MetersTableRecordsMax = 32;
		private const int PsnRegisterStatusMasksMax = 21;
		private const int DumpRulesMax = 100;

		private readonly string _fileName;

		public SystemConfigurationBuilderFromBinaryFile(string binaryFileName) {
			_fileName = binaryFileName;
		}
		public ISystemConfiguration BuildConfiguration() {
			var sysconf = new SystemConfigurationSimple();
			if (File.Exists(_fileName)) {
				var raw = File.ReadAllBytes(_fileName);
				if (raw.Length != FileSize) throw new Exception("SYSCONF.BIN file size must be " + FileSize);

				using (var br = new AdvancedBinaryReader(new MemoryStream(raw, false), false)) {
					//br.Read(sysconf.Raw, 0, sysconf.Raw.Length);
					br.BaseStream.Seek(0, SeekOrigin.Begin);

					//-----------------------------------------------------------0 (0)
					sysconf.DeviceAddress = br.ReadUInt16();
					sysconf.NetAddress = (int) br.ReadUInt32();

					var locomotiveAndSectionNumbers = br.ReadUInt16();
					sysconf.LocomotiveNumber = locomotiveAndSectionNumbers & 0x7FFF;
					sysconf.SectionNumber = (locomotiveAndSectionNumbers & 0x8000) > 0 ? 2 : 1;
					//br.BaseStream.Seek(8, SeekOrigin.Begin);
					sysconf.FirmwareVersion = br.ReadUInt16();
					sysconf.LastWrittenPageAddress = (int) br.ReadUInt32();
					sysconf.LastReadedBlockAddress = br.ReadUInt16();
					sysconf.BadBlocksCount = br.ReadUInt16();
					sysconf.LastWrittenPageNumber = br.ReadUInt16();
					sysconf.FirstWrittenAfterResetPageNumber = (int) br.ReadUInt32();
					sysconf.PsnLogStartPageNumber = (int) br.ReadUInt32();
					sysconf.ArrayDumpPsnStartPageNumber = (int) br.ReadUInt32();
					sysconf.FatOffsetFromPageZero = (int) br.ReadUInt32();
					sysconf.RpdPagesCountTransmittedToPsnLog = (int) br.ReadUInt32();
					sysconf.ConfigurationByte = br.ReadByte();

					//-----------------------------------------------------------1 (2048)
					br.BaseStream.Seek(2048, SeekOrigin.Begin);
					sysconf.FaultsCount = br.ReadByte();

					var rpdLogInfos = new IRpdDataInformation[FaultLogTableRecordsMax];
					var rpdLogInfoBuilder = new RpdDataInformationBuilderFromStream(br.BaseStream);
					for (int i = 0; i < FaultLogTableRecordsMax; ++i) {
						rpdLogInfos[i] = rpdLogInfoBuilder.Build();
					}
					sysconf.FaultDumpsTable = rpdLogInfos;

					//-----------------------------------------------------------2 (4096)
					br.BaseStream.Seek(4096, SeekOrigin.Begin);
					sysconf.MetersCount = br.ReadByte();
					var rpdMeters = new IRpdMeterSystemInformation[MetersTableRecordsMax];
					var rpdMeterInfoBuilder = new RpdMeterSystemInformationBuilderFromStream(br.BaseStream);
					for (int i = 0; i < MetersTableRecordsMax; ++i) {
						rpdMeters[i] = rpdMeterInfoBuilder.Build();
					}
					sysconf.MetersTable = rpdMeters;

					//4096 + 1217:
					br.BaseStream.Seek(4096 + 1217, SeekOrigin.Begin); // установим необходимое смещение
					var psnRegisterStatusMasks = new byte[PsnRegisterStatusMasksMax];
					br.Read(psnRegisterStatusMasks, 0, PsnRegisterStatusMasksMax);
					sysconf.PsnRegisterStatusMasks = psnRegisterStatusMasks;

					//4096 + 1238:
					br.BaseStream.Seek(4096 + 1238, SeekOrigin.Begin); // установим необходимое смещение
					var dumpRules = new IRpdDumpRule[DumpRulesMax];
					var rpdDumpRulesBuilder = new RpdDumpRuleBuilderFromStream(br.BaseStream);
					for (int i = 0; i < DumpRulesMax; ++i) {
						dumpRules[i] = rpdDumpRulesBuilder.Build();
					}
					sysconf.DumpRules = dumpRules;
					//------------------------------------------------------------3 (6144)

					const int offsetPage3 = 6144;
					const int offsetPowerOnStartPages = offsetPage3 + 2;
					const int offsetFixedStartPages = offsetPage3 + 500;

					br.BaseStream.Seek(offsetPage3, SeekOrigin.Begin);
					sysconf.CurrentPsnLogNumber = br.ReadUInt16();

					br.BaseStream.Seek(offsetPowerOnStartPages, SeekOrigin.Begin);
					var powerOnOffsetInfos = new List<IPsnDataFragmentInformation>();
					var psnLogFragmentInfoBuilder = new PsnDataFragmentInformationBuilderFromStream(br.BaseStream);
					for (int i = 0; i < PsnLogPowerOnStartPagesMaxCount; ++i) {
						var psnLogFragmentInfo = psnLogFragmentInfoBuilder.Build();
						if (psnLogFragmentInfo.StartOffset >= sysconf.PsnLogStartPageNumber) {
							powerOnOffsetInfos.Add(psnLogFragmentInfo);
						}
					}
					// Список инвертируется, т.к. перед записью элементы таблицы смещаются и последний элемент оказывается в начале таблицы
					powerOnOffsetInfos.Reverse();
					sysconf.PsnLogPowerUpFragmentInfos = powerOnOffsetInfos;

					br.BaseStream.Seek(offsetFixedStartPages, SeekOrigin.Begin); // Читаем фиксированные смещения:
					var predefinedFragmentInfos = new List<IPsnDataFragmentInformation>();
					for (int i = 0; i < PsnPredefinedFragmentStartInfosMaxCount; ++i) {
						predefinedFragmentInfos.Add(psnLogFragmentInfoBuilder.Build());
					}
					sysconf.PsnLogPredefinedFragmentInfos = predefinedFragmentInfos;
					br.BaseStream.Close();
				}
				return sysconf;
			}
			throw new Exception("Cannot find binary file, file " + _fileName + " is not exist");
		}
	}
}