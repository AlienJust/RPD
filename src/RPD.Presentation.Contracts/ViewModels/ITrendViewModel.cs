using System;
using System.Collections.ObjectModel;
using RPD.DAL;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;

namespace RPD.Presentation.Contracts.ViewModels {
	public interface ITrendViewModel {
		/// <summary>
		/// Список аварий имеющих отношение к этому тренду.
		/// </summary>
		ObservableCollection<IFaultViewModel> Faults { get; }

		/// <summary>
		/// Находится ли тренд на графике.
		/// </summary>
		bool IsOnPlot { get; set; }

		bool IsEnabled { get; set; }

		/// <summary>
		/// Признак того, что идёт процесс загрузки тренда.
		/// </summary>
		bool IsTrendLoading { get; set; }

		/// <summary>
		/// Имя тренда.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Тайтл тренда (может состоять из комбинации названия измерителя и названия сигнала).
		/// </summary>
		string Title { get; }


		System.Windows.Media.Color Color { get; set; }

		/// <summary>
		/// Данные тренда.
		/// </summary>
		ILazyTrendData TrendData { get; }

		/// <summary>
		/// Уникальный идентификатор тренда в архиве.
		/// </summary>
		Guid Uid { get; }

		/// <summary>
		/// Уникальный идентификатор ТИПА тренда. Идентифицирует сигнал на измерителе. Для каналов ПСН это IPsnChannelConfig.Id.
		/// </summary>
		Guid ConfigGuid { get; }

		/// <summary>
		/// Тип тренда.
		/// </summary>
		TrendChartType TrendChartType { get; set; }
	}

	public enum TrendChartType {
		Rpd,        /// Тренд данных РПД.
		Psn,        /// Тренд данных ПСН.
		PsnFault,   /// Тренд данных ПСН при логе аварии.
		Signal
	}
}
