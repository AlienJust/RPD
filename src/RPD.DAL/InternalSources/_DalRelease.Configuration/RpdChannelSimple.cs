using System;
using System.Collections.Generic;
using RPD.DAL;
using RPD.EventArgs;

namespace RPD.DalRelease.Configuration {
	class RpdChannelSimple : IRpdChannel {
		public List<IDataPoint> Trend {
			get { throw new NotImplementedException(); }
		}

		public DateTime Start { get { throw new NotImplementedException(); } }
		public DateTime End { get { throw new NotImplementedException(); } }

		public TrendType Type { get { throw new NotImplementedException(); } }

		public bool IsTrendLoaded { get { throw new NotImplementedException(); } }
		public void LoadTrendAsync(Action<OnCompleteEventArgs> onComplete) {
			throw new NotImplementedException();
		}

		public void FreeTrend() {
			throw new NotImplementedException();
		}

		public string Name { get; set; }
		public int Number { get; set; }
		public bool IsEnabled { get; set; }
		public bool IsService { get; set; }
		public IDataPoint CurrentValue { get; set; }
		public IRpdMeter OwnerMeter { get; set; }
		public IDumpCondition DumpCondition { get; set; }
	}
}