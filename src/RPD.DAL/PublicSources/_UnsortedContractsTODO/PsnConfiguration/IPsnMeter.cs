using System.Collections.ObjectModel;

namespace RPD.DAL {
	/// <summary>
	/// Прибор/устройство/измеритель
	/// </summary>
	public interface IPsnMeter {
		/// <summary>
		/// Название измерителя
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Список каналов, которые есть на измерителе
		/// </summary>
		ObservableCollection<IPsnChannel> Channels { get; }
	}
}
