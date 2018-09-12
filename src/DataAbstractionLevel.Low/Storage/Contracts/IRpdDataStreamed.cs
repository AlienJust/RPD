using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.StreamableData.Contracts;

namespace DataAbstractionLevel.Low.Storage.Contracts {
	/// <summary>
	/// ������ ���������� ����
	/// </summary>
	public interface IRpdDataStreamed : IStreamedData, IObjectWithIdentifier
	{
		//// <summary>
		//// ��������� ������ ������� (���������, ��� ����������� �������� �������������� ������� �����������)
		//// </summary>
		//// <param name="configuration">������������ ���</param>
		//// <param name="rpdSignalAddress">����� ������� ���</param>
		//// <param name="beginTime">����� ������� (������) ����</param>
		//IEnumerable<IDataPoint> LoadTrend(IRpdConfiguration configuration, IRpdSignalAddress rpdSignalAddress, DateTime beginTime);

		/// <summary>
		/// �������� � �������� �����-���� ������
		/// </summary>
		void UnloadSomeTrend();

		/// <summary>
		/// ������������ ����������, ���� �������
		/// </summary>
		IRpdDataPaged PagesInformation { get; }
	}
}