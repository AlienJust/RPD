using System.Collections.Generic;

namespace DataAbstractionLevel.Low.InternalKitchen.Contracts {
	/// <summary>
	/// ������ � �����
	/// </summary>
	public interface IHashableObject {
		/// <summary>
		/// ��� �������
		/// </summary>
		IList<byte> Hash { get; }
	}
}