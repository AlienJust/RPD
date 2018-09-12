using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.StreamableData.Contracts;

namespace DataAbstractionLevel.Low.Storage.StreamableStorages.Contracts {
	/// <summary>
	/// Потоковые данные с идентификатором
	/// </summary>
	public interface IStreamableDataWithId : IObjectWithIdentifier {
		/// <summary>
		/// Потоковые данные
		/// </summary>
		IStreamedData Data { get; }
	}
}