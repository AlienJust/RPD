using System;
using System.Collections.Generic;
using System.Linq;
using DataAbstractionLevel.Low.PsnConfig.Contracts;
using DataAbstractionLevel.Low.Storage.Contracts;
using RPD.DAL.DataExtraction.SimpleRelease;
using RPD.DAL.PsnDataExtraction.Contracts;

namespace RPD.DAL.PsnDataExtraction.BasedOnLowLevel {
	internal class PsnChannelTrendLoaderSimple : IPsnChannelTrendLoader
	{
		private readonly IPsnProtocolParameterConfiguration _signalConfiguration;
		private readonly IPsnProtocolCommandPartConfiguration _commandPartConfiguration;
		private readonly IPsnData _psnData;
		private readonly IPsnProtocolConfiguration _psnConfiguration;
		private readonly IPsnDataInformation _psnDataInformation;
		

		public PsnChannelTrendLoaderSimple(IPsnProtocolParameterConfiguration signalConfiguration, IPsnProtocolCommandPartConfiguration commandPartConfiguration, IPsnData psnData, IPsnProtocolConfiguration psnConfiguration, IPsnDataInformation psnDataInformation)
		{
			_signalConfiguration = signalConfiguration;
			_commandPartConfiguration = commandPartConfiguration;
			_psnData = psnData;
			_psnConfiguration = psnConfiguration;
			_psnDataInformation = psnDataInformation;
		}

		public List<IDataPoint> LoadTrend() {
			var beginTime = _psnDataInformation.BeginTime ?? (_psnDataInformation.SaveTime ?? DateTime.Now);
			List<IDataPoint> result = new List<IDataPoint>();
			_psnData.LoadTrend(_psnConfiguration, _commandPartConfiguration, _signalConfiguration, beginTime, dp => {
				result.Add(new DataPointSimple(dp.Value, dp.Time, dp.Valid, dp.DataPosition, _psnData));
			});
			return result;
		}

		public void FreeTrend() {
			_psnData.UnloadSomeTrend();
		}
	}
}