using System;
using System.Collections.Generic;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.PsnConfig.Contracts;
using DataAbstractionLevel.Low.PsnData;
using DataAbstractionLevel.Low.Storage.StreamableData.Contracts;

namespace DataAbstractionLevel.Low.Storage.Contracts {
	/// <summary>
	/// Данные магистрали ПСНа
	/// </summary>
	public interface IPsnData : IStreamedData, IObjectWithIdentifier, IBackSearchData
	{
		/// <summary>
		/// Загрузить данные сигнала (синхронно, для асинхронной загрузки воспользуйтесь методом репозитория)
		/// </summary>
		/// <param name="configuration">Конфигурация ПСН</param>
		/// <param name="psnCommandPart">Часть команды ПСН</param>
		/// <param name="psnParameterConfig">Параметр ПСН</param>
		/// <param name="beginTime">Время отсчета (начала) лога</param>
		List<DataPoint> LoadTrend(
			IPsnProtocolConfiguration configuration, 
			IPsnProtocolCommandPartConfiguration psnCommandPart,
			IPsnProtocolParameterConfiguration psnParameterConfig, 
			DateTime beginTime);

		/// <summary>
		/// Загрузить данные сигнала (синхронно, для асинхронной загрузки воспользуйтесь методом репозитория)
		/// </summary>
		/// <param name="configuration">Конфигурация ПСН</param>
		/// <param name="psnCommandPart">Часть команды ПСН</param>
		/// <param name="psnParameterConfig">Параметр ПСН</param>
		/// <param name="beginTime">Время отсчета (начала) лога</param>
		/// <param name="nextPointLoaded">Загружена очереднач точка</param>
		void LoadTrend(
			IPsnProtocolConfiguration configuration,
			IPsnProtocolCommandPartConfiguration psnCommandPart,
			IPsnProtocolParameterConfiguration psnParameterConfig,
			DateTime beginTime, Action<DataPoint> nextPointLoaded);

		/// <summary>
		/// Сообщает о выгрузке каких-либо данных
		/// </summary>
		void UnloadSomeTrend();

		/// <summary>
		/// Постраничная информация, если имеется
		/// </summary>
		IPsnDataPaged PagesInformation { get; }
	}
}
