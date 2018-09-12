using System.IO;

namespace DataAbstractionLevel.Low.Storage.StreamableData.Contracts {
	/// <summary>
	/// ��������� ������ ��� ������
	/// </summary>
	public interface IStreamedDataWritable
	{
		/// <summary>
		/// ������� ����� ��� ������ ������ � ����
		/// </summary>
		/// <returns></returns>
		Stream GetStreamForWriting();
	}
}