using System;
using System.IO;
using AlienJust.Support.IO;
using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts;

namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.ReleaseNand {
	public sealed class SystemConfigurationSaverBinary : ISystemConfigurationSaver {
		private const int FileSize = 106496;
		private const int PsnPredefinedFragmentStartInfosMaxCount = 100;
		private const int PsnLogPowerOnStartPagesMaxCount = 40;
		private const int FaultLogTableRecordsMax = 100;
		private const int MetersTableRecordsMax = 32;
		private const int PsnRegisterStatusMasksMax = 21;
		private const int DumpRulesMax = 100;

		private readonly string _fileName;
		private readonly bool _createFileIfNotExist;
		
		public SystemConfigurationSaverBinary(string binaryFileName, bool createFileIfNotExist)
		{
			_fileName = binaryFileName;
			_createFileIfNotExist = createFileIfNotExist;
		}

		public void SaveConfiguration(ISystemConfiguration configuration) {
			if (!File.Exists(_fileName)) {
				if (!_createFileIfNotExist)
					throw new Exception("File " + _fileName + " is not exist!");
				using (var stream = File.Create(_fileName)) {
					var buffer = new byte[] {0};
					for (int i = 0; i < FileSize; ++i) {
						stream.Write(buffer, 0, buffer.Length);
					}
					stream.Close();
				}
			}
			using (var bw = new AdvancedBinaryWriter(new FileStream(_fileName, FileMode.Open, FileAccess.Write), false)) {

				// Page #0 / 0000
				bw.Write((ushort) configuration.DeviceAddress);
				bw.Write((uint) configuration.NetAddress);

				bw.Write((ushort) (configuration.LocomotiveNumber + (configuration.SectionNumber > 1 ? 0x8000 : 0)));
				//bw.BaseStream.Seek(8, SeekOrigin.Begin);
				
				bw.Write((ushort) configuration.FirmwareVersion);
				bw.Write((uint) configuration.LastWrittenPageAddress);
				bw.Write((ushort) configuration.LastReadedBlockAddress);
				bw.Write((ushort) configuration.BadBlocksCount);
				bw.Write((ushort) configuration.LastWrittenPageNumber);
				bw.Write((uint) configuration.FirstWrittenAfterResetPageNumber);
				bw.Write((uint) configuration.PsnLogStartPageNumber);
				bw.Write((uint) configuration.ArrayDumpPsnStartPageNumber);
				bw.Write((uint) configuration.FatOffsetFromPageZero);
				bw.Write((uint) configuration.RpdPagesCountTransmittedToPsnLog);
				bw.Write((byte) configuration.ConfigurationByte);
				
				// Page #1 / 2048
				bw.BaseStream.Seek(2048, SeekOrigin.Begin);
				bw.Write((byte)configuration.FaultsCount);
				if (configuration.FaultDumpsTable.Count > FaultLogTableRecordsMax)
					throw new Exception("Rpd meters count is not " + FaultLogTableRecordsMax);
				var rpdLogInfoSaver = new RpdDataInformationSaverToStream(bw.BaseStream);
				foreach (var rpdLogInfo in configuration.FaultDumpsTable) {
					rpdLogInfoSaver.Save(rpdLogInfo);
				}

				// Page #2 / 4096
				bw.BaseStream.Seek(4096, SeekOrigin.Begin);
				bw.Write((byte)configuration.MetersCount);

				// “аблица измерителей:
				var rpdMeterInfoSaver = new RpdMeterSystemInformationSaverToStream(bw.BaseStream);
				if (configuration.MetersTable.Count > MetersTableRecordsMax)
					throw new Exception("Rpd meters count is more than " + MetersTableRecordsMax);
				foreach (var rpdMeterInfo in configuration.MetersTable) {
					rpdMeterInfoSaver.Save(rpdMeterInfo);
				}

				//4096 + 1217:
				bw.BaseStream.Seek(4096 + 1217, SeekOrigin.Begin); // установим необходимое смещение

				//TODO: ------------------------------------------------
				//if (configuration.PsnRegisterStatusMasks.Count != PsnRegisterStatusMasksMax)
				//throw new Exception("Psn register status masks count is not " + PsnRegisterStatusMasksMax);
				//bw.Write(configuration.PsnRegisterStatusMasks.ToArray(), 0, PsnRegisterStatusMasksMax);
				//------------------------------------------------------
				//4096 + 1238:
				//TODO: ------------------------------------------------
				//bw.BaseStream.Seek(4096 + 1238, SeekOrigin.Begin); // установим необходимое смещение
				//var rpdDumpRuleSaver = new RpdDumpRuleSaverToStream(bw.BaseStream);
				//if (configuration.DumpRules.Count != DumpRulesMax)
				//throw new Exception("Rpd dump rules count is not " + PsnRegisterStatusMasksMax);
				//foreach (var rpdDumpRule in configuration.DumpRules) {
				//rpdDumpRuleSaver.Save(rpdDumpRule);
				//}
				//------------------------------------------------------

				// Page #3 (6144)
				//TODO: ------------------------------------------------
				/*
				const int offsetPage3 = 6144;
				const int offsetPowerOnStartPages = offsetPage3 + 2;
				const int offsetFixedStartPages = offsetPage3 + 500;
				bw.BaseStream.Seek(offsetPage3, SeekOrigin.Begin);
				bw.Write((ushort)configuration.CurrentPsnLogNumber);
				
				
				if (configuration.PsnLogPowerUpFragmentInfos.Count > PsnLogPowerOnStartPagesMaxCount)
					throw new Exception("Psn log fragments info on power up must not be more than " + PsnLogPowerOnStartPagesMaxCount);
				
				bw.BaseStream.Seek(offsetPowerOnStartPages, SeekOrigin.Begin);
				var psnLogFragmentInfoSaver = new PsnLogFragmentInfoSaverToStream(bw.BaseStream);
				foreach (var psnLogPowerUpFragmentInfo in configuration.PsnLogPowerUpFragmentInfos) {
					psnLogFragmentInfoSaver.Save(psnLogPowerUpFragmentInfo);
				}
				*/

				// PsnLogPredefinedFragmentInfos are not saving
				/*if (configuration.PsnLogPredefinedFragmentInfos.Count > PsnPredefinedFragmentStartInfosMaxCount)
					throw new Exception("Psn log fragments info fixed length must not be more than " + PsnPredefinedFragmentStartInfosMaxCount);

				bw.BaseStream.Seek(offsetFixedStartPages, SeekOrigin.Begin);
				foreach (var psnLogPredefinedFragmentInfo in configuration.PsnLogPredefinedFragmentInfos) {
					psnLogFragmentInfoSaver.Save(psnLogPredefinedFragmentInfo);
				}*/
				//-----------------------------------------------------


				// TODO: 5 страница - копи€ второй:
				// Page #5 / 10240
				bw.BaseStream.Seek(10240, SeekOrigin.Begin);
				bw.Write((byte)configuration.MetersCount);

				// “аблица измерителей:
				var rpdMeterInfoSaverPage5 = new RpdMeterSystemInformationSaverToStream(bw.BaseStream);
				if (configuration.MetersTable.Count > MetersTableRecordsMax)
					throw new Exception("RPD.DAL число измерителей больше чем " + MetersTableRecordsMax);
				foreach (var rpdMeterInfo in configuration.MetersTable) {
					rpdMeterInfoSaverPage5.Save(rpdMeterInfo);
				}

				bw.BaseStream.Close();
			}
		}
	}
}