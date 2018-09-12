using System;
using System.Collections.Generic;
using DataAbstractionLevel.Low.PsnConfig.Contracts;
using DataAbstractionLevel.Low.PsnData;
using DataAbstractionLevel.Low.Storage.Contracts;
using RPD.DAL.DataExtraction.SimpleRelease;
using RPD.DAL.PsnDataExtraction.Contracts;

namespace RPD.DAL.PsnDataExtraction.BasedOnLowLevel {
	internal class PsnChannelTrendLoaderMerged : IPsnChannelTrendLoader
	{
		private readonly IPsnMergedParameter _mergedParam;
		private readonly IPsnData _psnData;
		private readonly IPsnProtocolConfiguration _psnConfiguration;
		private readonly IPsnDataInformation _psnDataInformation;
		private int _loadedTrendsCount;

		public PsnChannelTrendLoaderMerged(IPsnMergedParameter mergedParam, IPsnData psnData, IPsnProtocolConfiguration psnConfiguration, IPsnDataInformation psnDataInformation)
		{
			_mergedParam = mergedParam;
			_psnData = psnData;
			_psnConfiguration = psnConfiguration;
			_psnDataInformation = psnDataInformation;
			_loadedTrendsCount = 0;
		}


		public List<IDataPoint> LoadTrend()
		{
			var beginTime = _psnDataInformation.BeginTime ?? (_psnDataInformation.SaveTime ?? DateTime.Now);
			
			//var namedTrends = new Dictionary<string, List<DataAbstractionLevel.Low.PsnData.DataPoint>>();
			var advancedTrends = new Dictionary<DateTime, Dictionary<string, DataPoint>>();
			foreach (var part in _mergedParam.Parts)
			{
				IPsnMergedParameterPart part1 = part;
				_psnConfiguration.ForeachPsnParameterConfig((cmdPartConfig, parameterConfig) => {
					if (part1.RealParameterId.IdentyString == parameterConfig.Id.IdentyString) {
						var trend = _psnData.LoadTrend(_psnConfiguration, cmdPartConfig, parameterConfig, beginTime);
						//namedTrends.Add(part1.ExpressionName, trend);
						foreach (var dataPoint in trend) {
							if (!advancedTrends.ContainsKey(dataPoint.Time))
								advancedTrends.Add(dataPoint.Time, new Dictionary<string, DataPoint>());
							if (!advancedTrends[dataPoint.Time].ContainsKey(part1.ExpressionName)) // а если такое время уже есть?
								advancedTrends[dataPoint.Time].Add(part1.ExpressionName, dataPoint);
						}
						_loadedTrendsCount++;
						return true; // only one first match is checked
					}
					return false; // continue search
				});
			}

			var result = new List<IDataPoint>();
			// TODO: redo merge algorythm below:
			 
			//var maxPointsTrend = namedTrends.First(nt=>nt.Value.Count == namedTrends.Max(t => t.Value.Count));
			//var remainingTrends = namedTrends.Where(nt => nt.Key != maxPointsTrend.Key).ToList();
			var expression = new NCalc.Expression(_mergedParam.Expression);
			expression.EvaluateFunction += (name, args) => {
				if (name == "PositiveOrZero") {
					var paramValue = (double) args.Parameters[0].Evaluate();
					args.Result = paramValue > 0 ? paramValue : 0.0;
				}
				else if (name == "NegativeOrZero")
				{
					var paramValue = (double)args.Parameters[0].Evaluate();
					args.Result = paramValue < 0 ? Math.Abs(paramValue) : 0.0;
				}
			};
			/*
			foreach (var point in maxPointsTrend.Value) {
				expression.Parameters.Clear();
				expression.Parameters.Add(maxPointsTrend.Key, point.Value);
				try {
					foreach (var trend in remainingTrends) {
						var pt = trend.Value.First(p => p.Time == point.Time); // can throw
						expression.Parameters.Add(trend.Key, pt.Value);
					}

					var resultValue = (double) expression.Evaluate(); // can throw
					result.Add(new DataPointSimple(resultValue, point.Time, true, point.DataPosition));
				}
				catch {
					// Evaluation failed, not enough params or bad expressio text
					continue;
				}
			}*/

			foreach (var timeAndPoints in advancedTrends) {
				try {
					expression.Parameters.Clear();
					foreach (var nameAndPoint in timeAndPoints.Value)
					{
						expression.Parameters.Add(nameAndPoint.Key, nameAndPoint.Value.Value);
					}
					var resultValue = (double) expression.Evaluate();
					result.Add(new DataPointSimple(resultValue, timeAndPoints.Key, true, 0, _psnData));
				}
				catch {
					continue;
				}
			}

			if (!_mergedParam.IsMsIntegrated) return result; // TODO: split integration ability

			var integratedResult = new List<IDataPoint>();
			double intgratedValue = 0.0;

			if (result.Count > 1) {
				for (int i = 1; i < result.Count; ++i) {
					var prevPoint = result[i - 1];
					var curPoint = result[i];

					integratedResult.Add(new DataPointSimple(intgratedValue, prevPoint.Time, prevPoint.Valid, prevPoint.DataPosition, _psnData));

					var deltaTime = curPoint.Time - prevPoint.Time;
					intgratedValue += prevPoint.Value*deltaTime.TotalMilliseconds;
				}
			}
			return integratedResult;
		}

		public void FreeTrend()
		{
			while (_loadedTrendsCount > 0) {
				_psnData.UnloadSomeTrend();
				_loadedTrendsCount--;
			}
		}
	}
}