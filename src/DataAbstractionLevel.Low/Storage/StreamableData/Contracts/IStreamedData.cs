using System.IO;

namespace DataAbstractionLevel.Low.Storage.StreamableData.Contracts {
	/// <summary>
	/// ��������� ������ ��� ������
	/// </summary>
	public interface IStreamedData {
		/// <summary>
		/// ������� ����� ��� ��������� ������
		/// </summary>
		/// <returns>����� ��� ������ ������</returns>
		Stream GetStreamForReading();
	}
}