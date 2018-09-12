using System.Collections.Generic;
using System.Linq;

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

	/// <summary>
	/// ���������� ��� ������ � IHashableObject
	/// </summary>
	public static class HashableObjectExt {
		/// <summary>
		/// ��������� ��������� �� ����
		/// </summary>
		/// <param name="source">������ 1</param>
		/// <param name="secondObject">������ 2</param>
		/// <returns>������ ��� ���������� �����</returns>
		public static bool IsEqualByHash(this IHashableObject source, IHashableObject secondObject) {
			if (source.Hash.Count != secondObject.Hash.Count) {
				return false;
			}
			return !source.Hash.Where((t, i) => t != secondObject.Hash[i]).Any();
		}

		/// <summary>
		/// ���������� ��� � ��������� ����
		/// </summary>
		/// <param name="bytes">���</param>
		/// <param name="seporator">����������� ������</param>
		/// <returns>������-������������� ����</returns>
		public static string ToHumanString(this IList<byte> bytes, string seporator) {
			var result = bytes.Aggregate(string.Empty, (current, b) => current + (b.ToString("X2") + seporator));
			
			// remove last seporator and returns:
			return result.Substring(0, result.Length - seporator.Length);
		}
	}
}