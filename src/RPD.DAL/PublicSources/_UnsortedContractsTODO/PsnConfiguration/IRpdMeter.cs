using System.Collections.ObjectModel;

namespace RPD.DAL {
	/// <summary>
	/// Класс, описывающий измеритель (ИРВИ, УРАН и т.п.)
	/// </summary>
	public interface IRpdMeter {
		/// <summary>
		/// Некая информация, для удобной работы (не 5-ый измеритель 8-ой аварии, а измеритель давления двигателя, наприер)
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// Тип измерителя, предполагается доступ только из потоков пользовательского интерфейса
		/// </summary>
		RpdMeterType Type { get; set; }

		/// <summary>
		/// Некий номер измерителя
		/// </summary>
		int Address { get; set; }

		/// <summary>
		/// Список каналов, которые есть на измерителе
		/// </summary>
		ObservableCollection<IRpdChannel> Channels { get; }

		/// <summary>
		/// Авария, которой принадлежит измеритель
		/// </summary>
		IFaultLog OwnerFault { get; }
	}
}
