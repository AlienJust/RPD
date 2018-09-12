using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.Visuals.Annotations;
using GalaSoft.MvvmLight.Command;
using RPD.EventArgs;
using RPD.SciChartControl;

namespace RPD.Presentation.Contracts.ViewModels {
	/// <summary>
	/// Модель представления графика.
	/// </summary>
	public interface ISciChartViewModel {
		ObservableCollection<IChartSeriesViewModel> AnalogSeries { get; }

		ObservableCollection<ISeriesAdditionalData> AnalogSeriesAdditionalData { get; set; }

		ObservableCollection<IChartSeriesViewModel> DiscreteSeries { get; }

		ObservableCollection<ISeriesAdditionalData> DiscreteSeriesAdditionalData { get; set; }

		ObservableCollection<IAnnotation> Annotations { get; }

		RelayCommand<object> MouseRightButtonUpCommand { get; set; }

		/// <summary>
		/// Добавить тренд на график. Тренд будет добавлен на аналоговый (AnalogSeries) или дискретный (DiscreteSeries) график, 
		/// в зависимости от типа данных. Данные тренда загружаются асинхронно.
		/// </summary>
		/// <param name="trendViewModel">Модель предсталвения тренда.</param>
		/// <param name="onComplete">Вызывается после загрузки данных тренда и добавления его на график.</param>
		void AddTrend(ITrendViewModel trendViewModel, Action<OnCompleteEventArgs> onComplete);
		void RemoveTrend(ITrendViewModel trendViewModel);

		/// <summary>
		/// Вызывается когда у серии изменился цвет.
		/// </summary>
		/// <param name="changedSeries">Серия</param>
		/// <param name="color">новый цвет</param>
		void SeriesColorChanged(IChartSeriesViewModel changedSeries, Color color);
	}
}