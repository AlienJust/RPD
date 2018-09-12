namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts
{
	/// <summary>
	/// Информация о логе РПД
	/// </summary>
	public interface IRpdDataInformation
	{
		/// <summary>
		/// Номер
		/// </summary>
		int Number { get; }

		/// <summary>
		/// Статус
		/// </summary>
		int Status { get; }

		/// <summary>
		/// Год
		/// </summary>
		int Year { get; }

		/// <summary>
		/// Месяц
		/// </summary>
		int Month { get; }

		/// <summary>
		/// День
		/// </summary>
		int Day { get; }

		/// <summary>
		/// Час
		/// </summary>
		int Hour { get; }

		/// <summary>
		/// Минута
		/// </summary>
		int Minute { get; }

		/// <summary>
		/// Секунда
		/// </summary>
		int Second { get; }

		/// <summary>
		/// Адрес страницы с описанием
		/// </summary>
		int DescriptionPageAddress { get; }

		/// <summary>
		/// Адрес последней записанной страницы 
		/// </summary>
		int LastWrittenPageAddress { get; }

		/// <summary>
		/// Авария была прочитана
		/// </summary>
		int FaultWasReaded { get; }

		/// <summary>
		/// Число плохих страниц
		/// </summary>
		int BadPageCounter { get; }
	}
}
