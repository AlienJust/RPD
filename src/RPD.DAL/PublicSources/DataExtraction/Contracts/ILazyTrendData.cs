using System;
using System.Collections.Generic;

using RPD.EventArgs;

namespace RPD.DAL
{
	/// <summary>
	/// Тренд данных
	/// </summary>
	public interface ILazyTrendData //<IDataPoint>
	{
		/// <summary>
		/// Список точек сигнала, основанный на данных канала. Возможна такая ситуация, что разные каналы имеют разное временное разрешение и, как следствие,
		/// разное число точек, поэтому всего скорее в сигналах будет присутствовать неравномерное временное распределение точек
		/// </summary>
		List<IDataPoint> Trend { get; }

		/// <summary>
		/// Тип тренда (аналоговый/дискретный/...)
		/// </summary>
		TrendType Type { get; }

		/// <summary>
		/// Поднимается при явном вызове LoadTrend()
		/// </summary>
		bool IsTrendLoaded { get; }

		/// <summary>
		/// Загрузить тренд асинхронно
		/// </summary>
		void LoadTrendAsync(Action<OnCompleteEventArgs> onComplete);

		/// <summary>
		/// Освободить память (Trend.Clear() etc)
		/// </summary>
		void FreeTrend();
	}
}
