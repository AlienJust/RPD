using System;
using System.Collections.Generic;
using AlienJust.Support.Identy.Contracts;
using DataAbstractionLevel.Low.Storage.StreamableData.Contracts;

namespace DataAbstractionLevel.Low.Storage.Contracts {
	/// <summary>
	/// ��������� �����
	/// </summary>
	public interface IPsnDataStorage {
		/// <summary>
		/// ������������ ���� ��� ����� ����������
		/// </summary>
		IEnumerable<IPsnData> PsnDatas { get; }

		/// <summary>
		/// ��������� ��� ��� � ���������
		/// </summary>
		/// <param name="id">������������� ������</param>
		/// <param name="data">������</param>
		/// <param name="progressChangedAction">�������� ��� ��������� ���������</param>
		/// <returns>����� ����������� ������</returns>
		IStreamedData Add(IIdentifier id, IStreamedData data, Action<double> progressChangedAction);

		/// <summary>
		/// ������� ��� ��� �� ���������
		/// </summary>
		/// <param name="id">������������� ����</param>
		void Remove(IIdentifier id);
	}
}