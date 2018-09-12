using System;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.Model.DataSeries;

namespace RPD.SciChartControl.ViewModel
{
	/// <summary>
	/// Модель представления графиков. Содержит дополнительные данные, которы ассоциируются с ChartSeries.
	/// </summary>
	internal interface IRpdChartSeriesViewModel {
		IChartSeriesViewModel ChartSeries { get; }

		/// <summary>
		/// Копия данных серии. Изначально null. Создаётся после вызова CloneDataSeriesIfNull().
		/// </summary>
		IDataSeries<DateTime, double> OridinalDataSeries { get; }

		string MathFunc { get; set; }
		bool IsMathFuncChanged { get; set; }

		bool IsOnMainYAxis { get; set; }

		bool CanMoveBetweenYAxes { get; }

		/// <summary>
		/// Создаёт копию данных серии.
		/// </summary>
		void CloneDataSeriesIfNull();
	}
}