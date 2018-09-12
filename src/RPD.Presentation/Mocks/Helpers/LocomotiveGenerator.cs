using System;
using System.Threading;
using System.Windows.Threading;
using RPD.DAL;
using RPD.Presentation.Mocks.DAL;

namespace RPD.Presentation.Mocks.Helpers {
	static class LocomotiveGenerator {
		private static Timer _timer;
		public static ILocomotive GenerateLocomotive(string name) {
			var loco = new LocomotiveMock { Name = name };

			var section = new SectionMock(loco);
			loco.Sections.Add(section);

			var log = GeneratePsnLog(section);
			//log.LogIntegrity = PsnLogIntegrity.PagesFlowError;
			section.Psns.Add(log);
			section.Psns.Add(GeneratePsnLog(section));
			section.Psns.Add(GeneratePsnLog(section));
			section.Psns.Add(GeneratePsnLog(section));
			section.Psns.Add(GeneratePsnLog(section));
			section.Psns.Add(GeneratePsnLog(section));
			section.Psns.Add(GeneratePsnLog(section));
			section.Psns.Add(GeneratePsnLog(section));
			section.Psns.Add(GeneratePsnLog(section));

			var disp = Dispatcher.CurrentDispatcher;
			_timer = new Timer(state => {
				disp.Invoke(new Action(() => {
					var t = (PsnLogMock)section.Psns[0];
					t.LogIntegrity = PsnLogIntegrity.PagesFlowError;
				}));
			}, null, 10000, 5000);


			return loco;
		}

		private static PsnLogMock GeneratePsnLog(SectionMock section) {
			var psnLog = new PsnLogMock(section, PsnLogType.PowerDepended,
					new PsnConfigurationMock("1.0", "Буровая", "Буровая прошивка", Guid.NewGuid()));

			psnLog.Meters.Add(GenerateMeter(psnLog.BeginTime.Value, "Измеритель 1"));
			psnLog.Meters.Add(GenerateMeter(psnLog.BeginTime.Value, "Измеритель 2"));
			psnLog.Meters.Add(GenerateMeter(psnLog.BeginTime.Value, "Измеритель 3"));
			psnLog.Meters.Add(GenerateMeter(psnLog.BeginTime.Value, "Измеритель 4"));
			psnLog.Meters.Add(GenerateMeter(psnLog.BeginTime.Value, "Измеритель 5"));
			psnLog.Meters.Add(GenerateMeter(psnLog.BeginTime.Value, "Измеритель 6"));


			return psnLog;
		}

		private static IPsnMeter GenerateMeter(DateTime beginTime, string name) {
			var meter = new PsnMeterMock() { Name = name };
			GenerateChannels(meter, beginTime);
			return meter;
		}

		private static void GenerateChannels(PsnMeterMock meter, DateTime beginTime) {

			meter.Channels.Add(new PsnChannelMock(meter, TrendType.Analogue, beginTime, GeneratedDataType.Spiral) {
				Name = "Очень очень длинное название сигнала на каком-то измерителе",
				ConfigurationId = Guid.Parse("{ED036E5C-EEF6-42a8-9637-5469A6C7BB76}")
			});

			meter.Channels.Add(new PsnChannelMock(meter, TrendType.Analogue, beginTime, GeneratedDataType.SquirlyWave) {
				Name = "Squirly Wave",
				ConfigurationId = Guid.Parse("{70B400B6-1B97-4d08-A9FB-66E3EC9901C0}")
			});

			meter.Channels.Add(new PsnChannelMock(meter, TrendType.Analogue, beginTime, GeneratedDataType.Eeg) {
				Name = "EEG",
				ConfigurationId = Guid.Parse("{C8E032B0-367B-4536-9E71-EE3AC0FA7A18}")
			});

			meter.Channels.Add(new PsnChannelMock(meter, TrendType.Analogue, beginTime, GeneratedDataType.ClusteredPoints) {
				Name = "Clustered Points",
				ConfigurationId = Guid.Parse("{9B3D3CAA-8A85-4887-9EEB-ABC4D9A52E27}")
			});

			meter.Channels.Add(new PsnChannelMock(meter, TrendType.Analogue, beginTime, GeneratedDataType.DampedSinewave) {
				Name = "Damped Sinewave",
				ConfigurationId = Guid.Parse("{F980A507-4227-4695-B167-3825B8BC94BB}")
			});

			meter.Channels.Add(new PsnChannelMock(meter, TrendType.Analogue, beginTime, GeneratedDataType.ExponentialCurve) {
				Name = "Exponential Curve",
				ConfigurationId = Guid.Parse("{EB204464-D23E-4140-AF60-5308A4EC1ED9}")
			});

			meter.Channels.Add(new PsnChannelMock(meter, TrendType.Analogue, beginTime, GeneratedDataType.FourierSeries) {
				Name = "Fourier Series",
				ConfigurationId = Guid.Parse("{72C8F4F0-7ACF-4c02-BE28-F682A0BCCADB}")
			});

			meter.Channels.Add(new PsnChannelMock(meter, TrendType.Analogue, beginTime, GeneratedDataType.FourierSeriesZoomed) {
				Name = "Fourier Series Zoomed",
				ConfigurationId = Guid.Parse("{FDF7D1D8-F71A-4e42-9E01-5FBCCB09A66D}")
			});

			meter.Channels.Add(new PsnChannelMock(meter, TrendType.Analogue, beginTime, GeneratedDataType.LissajousCurve) {
				Name = "Lissajous Curve",
				ConfigurationId = Guid.Parse("{A64383D1-BBA9-4ba2-9425-52011B944CCF}")
			});

			meter.Channels.Add(new PsnChannelMock(meter, TrendType.Analogue, beginTime, GeneratedDataType.RandomDoubleSeries) {
				Name = "Random Double Series",
				ConfigurationId = Guid.Parse("{08E0DAFC-106A-4b7c-B6C6-EE6FF111FE99}")
			});

			meter.Channels.Add(new PsnChannelMock(meter, TrendType.Analogue, beginTime, GeneratedDataType.Sinewave) {
				Name = "Sinewave",
				ConfigurationId = Guid.Parse("{97B9CEC0-A125-4354-AF67-49DAD3A51639}")
			});

			meter.Channels.Add(new PsnChannelMock(meter, TrendType.Analogue, beginTime, GeneratedDataType.StraightLine) {
				Name = "Straight Line",
				ConfigurationId = Guid.Parse("{458C685B-8B96-4106-AAD0-643A8527191F}")
			});

			meter.Channels.Add(new PsnChannelMock(meter, TrendType.Discrete, beginTime, GeneratedDataType.Sinewave) {
				Name = "Дискретный с очень очень длинным название больше не знаю что писать 1",
				ConfigurationId = Guid.Parse("{E5EC473A-BD96-4367-AE66-83B8BAB18283}")
			});

			meter.Channels.Add(new PsnChannelMock(meter, TrendType.Discrete, beginTime, GeneratedDataType.SquirlyWave) {
				Name = "Дискретный 2",
				ConfigurationId = Guid.Parse("{918A93AF-942B-42a0-9B78-21F02048B967}")
			});

			meter.Channels.Add(new PsnChannelMock(meter, TrendType.Discrete, beginTime, GeneratedDataType.Spiral) {
				Name = "Дискретный 3",
				ConfigurationId = Guid.Parse("{896AACD5-3B9D-442f-B122-8E047714FC35}")
			});

			meter.Channels.Add(new PsnChannelMock(meter, TrendType.Discrete, beginTime, GeneratedDataType.RandomDoubleSeries) {
				Name = "Дискретный 4",
				ConfigurationId = Guid.Parse("{8F6E2436-516C-4287-9D6F-A4FE5C3B9452}")
			});

			meter.Channels.Add(new PsnChannelMock(meter, TrendType.Discrete, beginTime, GeneratedDataType.Sinewave) {
				Name = "Дискретный 5",
				ConfigurationId = Guid.Parse("{63AD181F-422E-4748-8752-27DA66BA9E6C}")
			});

			meter.Channels.Add(new PsnChannelMock(meter, TrendType.Discrete, beginTime, GeneratedDataType.SquirlyWave) {
				Name = "Дискретный 6",
				ConfigurationId = Guid.Parse("{FAC82852-2E1E-4dda-A4CA-9DD3F7129993}")
			});

			meter.Channels.Add(new PsnChannelMock(meter, TrendType.Discrete, beginTime, GeneratedDataType.Spiral) {
				Name = "Дискретный 7",
				ConfigurationId = Guid.Parse("{80FC86B0-CF07-4094-8E02-67B22410DE9D}")
			});

			meter.Channels.Add(new PsnChannelMock(meter, TrendType.Discrete, beginTime, GeneratedDataType.RandomDoubleSeries) {
				Name = "Дискретный 8",
				ConfigurationId = Guid.Parse("{EBD0EA6C-4882-4d7b-8180-95E4FDAE9F13}")
			});

			meter.Channels.Add(new PsnChannelMock(meter, TrendType.Discrete, beginTime, GeneratedDataType.RandomDoubleSeries) {
				Name = "Дискретный 9",
				ConfigurationId = Guid.Parse("{94890C3A-FEA2-4303-862B-F0E9DA2BC816}")
			});

			meter.Channels.Add(new PsnChannelMock(meter, TrendType.Discrete, beginTime, GeneratedDataType.DampedSinewave) {
				Name = "Дискретный 10",
				ConfigurationId = Guid.Parse("{0761F0C3-33A1-4be0-B652-C4054E054152}")
			});

			meter.Channels.Add(new PsnChannelMock(meter, TrendType.Discrete, beginTime, GeneratedDataType.FourierSeries) {
				Name = "Дискретный 11",
				ConfigurationId = Guid.Parse("{3A7E16EB-554E-4ff3-8ACC-B99D39F7AF68}")
			});

		}
	}
}
