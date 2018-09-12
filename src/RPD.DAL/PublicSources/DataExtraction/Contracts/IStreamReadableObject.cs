using System.IO;

namespace RPD.DAL {
	/// <summary>
	/// ��������� ������ ��� ������
	/// </summary>
	public interface IStreamReadableObject
	{
		/// <summary>
		/// ������� ����� ��� ��������� ������
		/// </summary>
		/// <returns>����� ��� ������ ������</returns>
		Stream GetStreamForReading();
	}
}