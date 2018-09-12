using System;

namespace RPD.DAL {
	/// <summary>
	/// Описание канала измерителя ПСН
	/// </summary>
	public interface IPsnChannel : ILazyTrendData {
		/// <summary>
		/// Название канала
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Канал используется
		/// </summary>
		bool IsEnabled { get; }

		/// <summary>
		/// Флаг того, что канал является входящим (Параметр, переданный от МК1)
		/// </summary>
		bool IsInput { get; }

		/// <summary>
		/// Является ли канал признаком аварии
		/// </summary>
		bool CanBeFaultSign { get; }

		/// <summary>
		/// Флаг, что пользователь отметил сигнал как FaultSign (Значение этого флага не изменится при попытке его задать у канала без взведенного флага CanBeFaultSign
		/// </summary>
		bool IsFaultSign { get; }

		/// <summary>
		/// Измеритель, которому принадлежит
		/// </summary>
		IPsnMeter OwnerPsnMeter { get; }

		/// <summary>
		/// Уникальный идентификатор канала
		/// </summary>
		Guid UnicId { get; }

		/// <summary>
		/// Идентификатор конфигурации канала
		/// </summary>
		Guid ConfigurationId { get; }
	}

	internal static class PsnChannelExtension {
		public static bool IsEqualTo(this IPsnChannel channel1, IPsnChannel channel2) {
			if (channel1.CanBeFaultSign != channel2.CanBeFaultSign) return false;
			//if (channel1.End != channel2.End) return false;
			if (channel1.IsEnabled != channel2.IsEnabled) return false;
			if (channel1.IsFaultSign != channel2.IsFaultSign) return false;
			if (channel1.IsInput != channel2.IsInput) return false;
			if (channel1.Name != channel2.Name) return false;
			//if (channel1.Start != channel2.Start) return false;
			if (channel1.Type != channel2.Type) return false;
			return true;
		}
	}
}
