using System.Collections.ObjectModel;

namespace RPD.DAL.RepostirorySystem.SimpleReleaseAndExtensions {
	/// <summary>
	/// Пока что очень простая реализация, не знающая ничего ни о _ui, ни о _bw
	/// </summary>
	class PsnMeterSimple : IPsnMeter {
		private readonly string _name;
		private readonly ObservableCollection<IPsnChannel> _channels;

		public PsnMeterSimple(string name, ObservableCollection<IPsnChannel> channels) {
			_name = name;
			_channels = channels;
		}

		public string Name {
			get { return _name; }
		}

		public ObservableCollection<IPsnChannel> Channels {
			get { return _channels; }
		}
	}
}