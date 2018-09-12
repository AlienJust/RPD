using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RPD.DAL;
using RPD.EventArgs;

namespace RPD.Presentation.Mocks.DAL {
	class SignalMock : ISignal {
		public SignalMock(TrendType type) {
			Type = type;

		}

		public void LoadTrendAsync(Action<OnCompleteEventArgs> onComplete) {
			throw new NotImplementedException();
		}

		public void FreeTrend() {
			throw new NotImplementedException();
		}

		public List<IDataPoint> Trend { get; private set; }
		public TrendType Type { get; }
		public bool IsTrendLoaded { get; private set; }
		public ObservableCollection<IRpdChannel> Channels { get; set; }
		public string MathOperation { get; set; }
		public string Name { get; set; }
		public IFaultLog OwnerFault { get; private set; }
	}
}
