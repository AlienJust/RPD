using System;
using System.Collections.Generic;
using DataAbstractionLevel.Low.PsnConfig.Contracts;
using DataAbstractionLevel.Low.PsnData.Contracts;

namespace DataAbstractionLevel.Low.PsnData {
	internal class PsnChannelDataHandler : IPsnChannelDataHandler {
		private readonly IPsnDataHandler _psnDataHandler;
		private readonly IPsnProtocolCommandPartConfiguration _psnCommandPart;
		private readonly IPsnProtocolParameterConfiguration _psnParameterConfig;
		private readonly IPsnProtocolConfiguration _configuration;

		public PsnChannelDataHandler(IPsnDataHandler dataHandler, IPsnProtocolCommandPartConfiguration psnCommandPart, IPsnProtocolParameterConfiguration psnParameterConfig, IPsnProtocolConfiguration configuration)
		{
			_psnDataHandler = dataHandler;
			_psnCommandPart = psnCommandPart;
			_psnParameterConfig = psnParameterConfig;
			_configuration = configuration;
		}

		public List<DataPoint> GetDataPoints(DateTime beginTime) {
			return _psnDataHandler.GetDataPoints(_configuration, _psnCommandPart, _psnParameterConfig, beginTime);
		}

		public void GetDataPoints(DateTime beginTime, Action<DataPoint> nextPointLoaded) {
			_psnDataHandler.GetDataPoints(_configuration, _psnCommandPart, _psnParameterConfig, beginTime, nextPointLoaded);
		}
	}
}