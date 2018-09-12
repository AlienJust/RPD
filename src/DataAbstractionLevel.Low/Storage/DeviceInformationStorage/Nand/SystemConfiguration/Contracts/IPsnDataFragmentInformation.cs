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

	internal static class PsnDataFragmentInfoExtensions {
		public static bool IsEqualTo(this IPsnDataFragmentInformation info1, IPsnDataFragmentInformation info2)
		{
			if (info1.StartOffset != info2.StartOffset) return false;
			if (info1.StartTime != info2.StartTime) return false;
			return true;
		}
	}
}