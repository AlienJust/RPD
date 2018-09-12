using System;
using System.Collections.Generic;
using System.IO;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.PsnConfig.Contracts;
using DataAbstractionLevel.Low.PsnData;
using DataAbstractionLevel.Low.PsnData.Contracts;
using DataAbstractionLevel.Low.PsnData.PsnBinHandlerMapBased;
using DataAbstractionLevel.Low.Storage.Contracts;
using DataAbstractionLevel.Low.Storage.StreamableData.Contracts;

namespace DataAbstractionLevel.Low.Storage.PsnData {
	internal sealed class PsnDataRelayStream : IPsnData {
		private readonly IStreamedData _relayOnStream;
		private readonly IPsnDataHandlerWithResources _psnDataHandler;
		private readonly IPsnDataHandlerRetrieveCmdPartByPos _psnDataHandlerBack;
		private int _loadedTrendsCount;

		public PsnDataRelayStream(IStreamedData relayOnStream, IIdentifier id)
		{
			_relayOnStream = relayOnStream;
			Id = id;

			var handler = new PsnBinHandlerMappedLite(GetStreamForReading);
			_psnDataHandler = handler;
			_psnDataHandlerBack = handler;

			PagesInformation = new PsnDataPagedHandlerRelay(handler);
			_loadedTrendsCount = 0;
		}


		public List<DataPoint> LoadTrend(IPsnProtocolConfiguration configuration, IPsnProtocolCommandPartConfiguration psnCommandPart, IPsnProtocolParameterConfiguration psnParameterConfig, DateTime beginTime)
		{
			if (_loadedTrendsCount == 0) _psnDataHandler.FillResources(configuration, beginTime, configuration.CycleCommandParts, configuration.CommandParts);

			var result = new PsnChannelDataHandler(_psnDataHandler, psnCommandPart, psnParameterConfig, configuration).GetDataPoints(beginTime);
			_loadedTrendsCount++;

			return result;
		}

		public void LoadTrend(IPsnProtocolConfiguration configuration, IPsnProtocolCommandPartConfiguration psnCommandPart, IPsnProtocolParameterConfiguration psnParameterConfig, DateTime beginTime, Action<DataPoint> nextPointLoaded) {
			if (_loadedTrendsCount == 0) _psnDataHandler.FillResources(configuration, beginTime, configuration.CycleCommandParts, configuration.CommandParts);

			new PsnChannelDataHandler(_psnDataHandler, psnCommandPart, psnParameterConfig, configuration).GetDataPoints(beginTime, nextPointLoaded);
			_loadedTrendsCount++;
		}

		public void UnloadSomeTrend() {
			_loadedTrendsCount--;
			if (_loadedTrendsCount <= 0) {
				FreeResources();
			}
		}

		public IPsnDataPaged PagesInformation { get; }

		private void FreeResources() {
			_loadedTrendsCount = 0;
			_psnDataHandler.FreeResources();
		}
		
		public override string ToString() {
			string result = string.Empty;
			try {
				result += "Data filename=" + _relayOnStream + Environment.NewLine;
			}
			catch (Exception ex) {
				result = ex.ToString();
			}
			return result;
		}

		public Stream GetStreamForReading() {
			var stream = _relayOnStream.GetStreamForReading();
			return stream;
		}

		public IIdentifier Id { get; }


		public byte[] GetCommandPartBytes(int dataPosition)
		{
			return _psnDataHandlerBack.GetCommandPartBytes(dataPosition);
		}
	}
}