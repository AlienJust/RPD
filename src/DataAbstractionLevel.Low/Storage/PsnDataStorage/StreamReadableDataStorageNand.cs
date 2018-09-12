using System;
using System.Collections.Generic;
using System.IO;
using AlienJust.Support.Identy;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.PsnData;
using DataAbstractionLevel.Low.Storage.Contracts;
using DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.ReleaseNand;
using DataAbstractionLevel.Low.Storage.PsnData;
using DataAbstractionLevel.Low.Storage.StreamableData.Contracts;
using DataAbstractionLevel.Low.Storage.StreamableData.FilePart;
using DataAbstractionLevel.Low.Storage.StreamableData.SingleFile;

namespace DataAbstractionLevel.Low.Storage.PsnDataStorage {
	public sealed class StreamReadableDataStorageNand : IPsnDataStorage
	{
		private readonly string _directoryPath;

		public StreamReadableDataStorageNand(string directoryPath)
		{
			_directoryPath = directoryPath;
			// TODO: move to DeviceInformationStorageNand
		}


		public IEnumerable<IPsnData> PsnDatas { get {
			var psnDatas = new List<IPsnData>();

			var systemConfigLoader = new SystemConfigurationBuilderFromBinaryFile(Path.Combine(_directoryPath, "SYSCONF.BIN"));
			var systemConfig = systemConfigLoader.BuildConfiguration();
			var psnFileInfo = new FileInfo(Path.Combine(_directoryPath, "PSN.BIN")); // Информация о файле лога магистрали ПСН
			if (psnFileInfo.Exists)
			{
				var fixedFragments = PsnBinParser.ApplyAdvancedFragmentationFixed(
					psnFileInfo.FullName,
					PsnBinParser.GetPsnPagesIntervalsFixed(
						psnFileInfo.FullName,
						systemConfig.PsnLogPredefinedFragmentInfos,
						systemConfig.LastWrittenPageAddress,
						systemConfig.PsnLogStartPageNumber));

				var powerFragments = PsnBinParser.ApplyAdvancedFragmentationStack(
					psnFileInfo.FullName,
					PsnBinParser.GetPsnPagesIntervalsStack(
						psnFileInfo.FullName,
						systemConfig.PsnLogPowerUpFragmentInfos,
						systemConfig.LastWrittenPageAddress,
						systemConfig.PsnLogStartPageNumber));

				foreach (PsnBinLogAdvancedLocationInfo t in fixedFragments) {
					var id1 = t.FirstPageInfo.Number.ToString("X4");
					var id2 = t.LastPageInfo.Number.ToString("X4");
					var id3 = systemConfig.NetAddress.ToString("X4");

					var beginTime = t.FirstPageInfo.Time.HasValue ? t.FirstPageInfo.Time.Value : t.FirstDatedPageInfo.HasValue ? t.FirstDatedPageInfo.Value.Time : null;
					var endTime = t.LastPageInfo.Time.HasValue ? t.LastPageInfo.Time : null;

					var id4 = beginTime?.Ticks.ToString("X8") ?? string.Empty;
					var id5 = endTime?.Ticks.ToString("X8") ?? string.Empty;

					var streamedData = new PsnDataRelayStream(
						new StreamReadableObjectRelaySubStream(
							new StreamedFile(psnFileInfo.FullName),
							PsnPageExtractorFactory.Extractor.PsnPageSize*t.FirstPageInfo.Number,
							PsnPageExtractorFactory.Extractor.PsnPageSize*(t.LastPageInfo.Number - t.FirstPageInfo.Number + 1)),
						new IdentifierStringToLowerBased(id1 + id2 + id3 + id4 + id5));
					// TODO: id as in dataInfo
					psnDatas.Add(streamedData);
				}

				foreach (PsnBinLogAdvancedLocationInfo t in powerFragments)
				{
					var id1 = t.FirstPageInfo.Number.ToString("X4");
					var id2 = t.LastPageInfo.Number.ToString("X4");
					var id3 = systemConfig.NetAddress.ToString("X4");

					var beginTime = t.FirstPageInfo.Time.HasValue ? t.FirstPageInfo.Time.Value : t.FirstDatedPageInfo.HasValue ? t.FirstDatedPageInfo.Value.Time : null;
					var endTime = t.LastPageInfo.Time.HasValue ? t.LastPageInfo.Time : null;

					var id4 = beginTime?.Ticks.ToString("X8") ?? string.Empty;
					var id5 = endTime?.Ticks.ToString("X8") ?? string.Empty;

					var streamedData = new PsnDataRelayStream(
						new StreamReadableObjectRelaySubStream(
							new StreamedFile(psnFileInfo.FullName),
							PsnPageExtractorFactory.Extractor.PsnPageSize * t.FirstPageInfo.Number,
							PsnPageExtractorFactory.Extractor.PsnPageSize * (t.LastPageInfo.Number - t.FirstPageInfo.Number + 1)),
						new IdentifierStringToLowerBased(id1 + id2 + id3 + id4 + id5));
					// TODO: id as in dataInfo
					psnDatas.Add(streamedData);
				}
			}
			return psnDatas;
		} }


		public IStreamedData Add(IIdentifier id, IStreamedData data, Action<double> progressChangeAction)
		{
			throw new Exception("На устройство нельзя добавлять логи");
		}

		public void Remove(IIdentifier id)
		{
			throw new Exception("С устройства нельзя удалять логи");
		}

		public override string ToString()
		{
			return "NandFlashStorage@" + _directoryPath;
		}
	}
}