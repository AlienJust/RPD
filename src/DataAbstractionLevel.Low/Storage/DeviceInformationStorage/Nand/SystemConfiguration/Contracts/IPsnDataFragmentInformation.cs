using System;

namespace DataAbstractionLevel.Low.Storage.DeviceInformationStorage.Nand.SystemConfiguration.Contracts {
	/// <summary>
	/// Информация о фрагменте лога ПСН
	/// </summary>
	public interface IPsnDataFragmentInformation {
		/// <summary>
		/// Смещение начала
		/// </summary>
		int StartOffset { get; }

		/// <summary>
		/// Начало фрагмента лога
		/// </summary>
		DateTime? StartTime { get; }
	}
}