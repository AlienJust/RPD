using System.Collections.Generic;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.StreamableData.Contracts;

namespace DataAbstractionLevel.Low.Storage.StreamableStorages.Contracts {
	/// <summary>
	/// ��������� �����
	/// </summary>
	public interface IStreamableDataStorage {
		/// <summary>
		/// ������������ ���� ������ ����������
		/// </summary>
		IEnumerable<IStreamableDataWithId> Datas { get; }

		/// <summary>
		/// ��������� ��� ��� � ���������
		/// </summary>
		/// <param name="id">������������� ������</param>
		/// <param name="data">������</param>
		/// <returns>����� ����������� ������</returns>
		void Add(IIdentifier id, IStreamedData data);

		/// <summary>
		/// ������� ��� ��� �� ���������
		/// </summary>
		/// <param name="id">������������� ����</param>
		void Remove(IIdentifier id);
	}
}