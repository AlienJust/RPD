using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.StreamableData.Contracts;

namespace DataAbstractionLevel.Low.Storage.StreamableStorages.Contracts {
	/// <summary>
	/// ��������� ������ � ���������������
	/// </summary>
	public interface IStreamableDataWithId : IObjectWithIdentifier {
		/// <summary>
		/// ��������� ������
		/// </summary>
		IStreamedData Data { get; }
	}
}