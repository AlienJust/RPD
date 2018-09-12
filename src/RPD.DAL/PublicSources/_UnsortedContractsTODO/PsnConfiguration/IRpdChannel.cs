
namespace RPD.DAL {
	/// <summary>
	/// Описывает 1 канал измерителя
	/// </summary>
	public interface IRpdChannel : ILazyTrendData //<IDataPoint>
	{
		/// <summary>
		/// Название канала
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// Некий номер канала
		/// </summary>
		int Number { get; }

		/// <summary>
		/// Флаг предварительного конфигурирования канала (true - для канала была задана конфигурация, false - канал не конфигурировался)
		/// этот флаг записывается в таблицу измерителей, как бит двухбайтового числа таблицы (2 байта = 16 битовых каналов)
		/// </summary>
		bool IsEnabled { get; set; }

		/// <summary>
		/// Флаг того, что канал является служебным (например, у урана это каналы 8 и 16)
		/// </summary>
		bool IsService { get; }

		/// <summary>
		/// Текущее значение (которое предается в заголовке аварии), хз, всего скорее это не точка
		/// </summary>
		IDataPoint CurrentValue { get; }

		/// <summary>
		/// Измеритель, которому принадлежит
		/// </summary>
		IRpdMeter OwnerMeter { get; }

		/// <summary>
		/// Условие канала, при срабатывании которого производится дамп (нужно в основном для конфигурации)
		/// </summary>
		IDumpCondition DumpCondition { get; }
	}

	internal static class RpdChannelExtensions {
		public static bool IsEqualTo(this IRpdChannel channel1, IRpdChannel channel2) {
			if (!channel1.DumpCondition.IsEqualTo(channel2.DumpCondition)) return false;
			//if (channel1.End != channel2.End) return false;
			if (channel1.IsEnabled != channel2.IsEnabled) return false;
			if (channel1.IsService != channel2.IsService) return false;
			if (channel1.Name != channel2.Name) return false;
			if (channel1.Number != channel2.Number) return false;
			//if (channel1.Start != channel2.Start) return false;
			//if (channel1.Type != channel2.Type) return false;
			return true;
		}
	}
}
